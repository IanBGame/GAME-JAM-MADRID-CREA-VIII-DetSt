"""
Fiesta Remix - SFX v2 (Rhythm Heaven-style). 8 sounds: one GOOD + one FAIL per minigame.
Style targets measured from reference material: dry (no reverb), fast attack,
-20 dB within ~100-400 ms, energy centred ~2-4.5 kHz, cartoon pitch glides
(up = good, down = bad), physical object sounds instead of musical jingles.
"""
import os, sys, json, subprocess
import numpy as np
from scipy.signal import butter, sosfilt
from scipy.io import wavfile

OUT = sys.argv[1] if len(sys.argv) > 1 else "out_sfx_v2"
os.makedirs(OUT, exist_ok=True)
SR = 44100
rng = np.random.default_rng(4)

def tv(d): return np.arange(int(d * SR)) / SR
def noise(d): return rng.uniform(-1, 1, int(d * SR))
_c = {}
def flt(x, kind, f, order=2):
    k = (kind, f, order)
    if k not in _c: _c[k] = butter(order, f, kind, fs=SR, output="sos")
    return sosfilt(_c[k], x)
def bp(x, lo, hi): return flt(x, "bandpass", (lo, hi))
def lp(x, f): return flt(x, "lowpass", f)
def hp(x, f): return flt(x, "highpass", f)
def env(d, att=0.001, tau=0.05):
    t = tv(d); return np.minimum(t / att, 1) * np.exp(-t / tau)
def mix(*parts):
    n = max(int(s * SR) + len(x) for x, s, g in parts); y = np.zeros(n)
    for x, s, g in parts:
        i = int(s * SR); y[i:i + len(x)] += x * g
    return y
def glide(f0, f1, d, curve=1.0):
    t = tv(d); u = (t / d) ** curve
    return f0 * (f1 / f0) ** u

# --- modal synthesis: physical objects as sums of damped partials
def modal(f, ratios, gains, taus, d, jitter=0.0):
    t = tv(d); y = np.zeros(len(t))
    for r, g, tau in zip(ratios, gains, taus):
        y += g * np.sin(2 * np.pi * f * r * (1 + jitter * rng.normal()) * t + rng.random() * 6) * np.exp(-t / tau)
    return y * np.minimum(t / 0.0004, 1)

# --- cartoon voice-ish tone: harmonics shaped by moving vowel formants
VOW = {"u": (320, 800, 2300), "o": (480, 900, 2450), "a": (760, 1250, 2550), "e": (480, 1900, 2650),
       "i": (300, 2250, 3050), "uh": (560, 1100, 2450)}
def chirp(f0, f1, d, v0="u", v1="a", tau=0.08, curve=0.6, att=0.004, buzz=1.0):
    t = tv(d); f = glide(f0, f1, d, curve); ph = 2 * np.pi * np.cumsum(f) / SR
    u = (t / d) ** 0.7
    F = [(1 - u) * a + u * b for a, b in zip(VOW[v0], VOW[v1])]
    y = np.zeros(len(t))
    for k in range(1, int(7000 / max(f0, f1))):
        fk = k * f
        g = sum(w / (1 + ((fk - Fc) / (bw)) ** 2) for Fc, bw, w in zip(F, (90, 120, 170), (1.0, 0.7, 0.35)))
        y += g * np.sin(k * ph) * (buzz if k > 1 else 1)
    e = np.minimum(t / att, 1) * np.exp(-t / tau)
    return y * e / (np.abs(y * e).max() + 1e-9)

def boing(f0, f1, d, wob=0.18, wobf=17, tau=0.18):
    """spring: fast vibrato that dies away while the pitch droops"""
    t = tv(d); f = glide(f0, f1, d, 0.8) * (1 + wob * np.exp(-t / 0.12) * np.sin(2 * np.pi * wobf * t))
    ph = 2 * np.pi * np.cumsum(f) / SR
    y = np.sin(ph) + 0.5 * np.sin(2 * ph) + 0.25 * np.sin(3 * ph) + 0.12 * np.sin(5 * ph)
    return y * np.minimum(t / 0.002, 1) * np.exp(-t / tau)

def snap(d=0.12, lo=1800, tau=0.006):
    return hp(noise(d), lo) * env(d, 0.0003, tau)

def squish(d=0.22):
    """wet: noise through a formant that slides down, with bubbly crackle"""
    t = tv(d); src = lp(noise(d), 4000); y = np.zeros(len(t)); hop = 256; win = np.hanning(hop * 2)
    for i in range(0, len(t), hop):
        c = 1600 * (280 / 1600) ** (i / len(t))
        seg = bp(src[i:i + hop * 2], c / 1.35, c * 1.35); seg = seg[: len(win)] * win[: len(seg)]
        y[i:i + len(seg)] += seg[: len(t) - i]
    for _ in range(14):
        st = rng.uniform(0, d * 0.6); fb = rng.uniform(500, 1400)
        b = np.sin(2 * np.pi * np.cumsum(glide(fb, fb * 1.8, 0.012)) / SR) * env(0.012, 0.0005, 0.004)
        i = int(st * SR); y[i:i + len(b)] += b[: len(y) - i] * 0.25
    return y * np.minimum(t / 0.003, 1) * np.exp(-t / 0.07)

def whoosh(d, f0, f1, q=1.5):
    src = noise(d + 0.05); n = int(d * SR); y = np.zeros(n); hop = 256; win = np.hanning(hop * 2)
    for i in range(0, n, hop):
        c = f0 * (f1 / f0) ** (i / n)
        seg = bp(src[i:i + hop * 2], c / q, min(c * q, 18000)); seg = seg[: len(win)] * win[: len(seg)]
        y[i:i + len(seg)] += seg[: n - i]
    return y * np.sin(np.pi * np.linspace(0, 1, n)) ** 0.7

def bubble(f0, d=0.06, rise=2.4, tau=0.02):
    return np.sin(2 * np.pi * np.cumsum(glide(f0, f0 * rise, d, 0.5)) / SR) * env(d, 0.0008, tau)

def thump(f=110, d=0.15, tau=0.04):
    t = tv(d); fr = f + 90 * np.exp(-t / 0.01)
    return np.sin(2 * np.pi * np.cumsum(fr) / SR) * env(d, 0.0005, tau)

WOOD = ([1, 2.57, 4.1], [1, 0.5, 0.25], [0.035, 0.014, 0.007])
METAL_HI = ([1, 2.76, 5.40, 8.93], [1, 0.6, 0.35, 0.2], [0.17, 0.11, 0.06, 0.03])     # triangle/bell-ish
TIN = ([1, 1.59, 2.38, 3.9, 5.1], [1, 0.8, 0.6, 0.4, 0.25], [0.09, 0.07, 0.05, 0.03, 0.02])

LIST = []
def save(name, y, use, peak=-1.0):
    y = hp(np.asarray(y, float), 60)
    a = np.abs(y); idx = np.where(a > a.max() * 10 ** (-55 / 20))[0]; y = y[: idx[-1] + 200]
    n = int(0.006 * SR); y[-n:] *= np.linspace(1, 0, n)
    y *= 10 ** (peak / 20) / np.abs(y).max()
    wavfile.write(f"{OUT}/{name}.wav", SR, (y * 32767).astype(np.int16))
    subprocess.run(["ffmpeg", "-loglevel", "error", "-y", "-i", f"{OUT}/{name}.wav", "-c:a", "libvorbis", "-q:a", "7",
                    f"{OUT}/{name}.ogg"], check=True)
    e = np.convolve(np.abs(y), np.ones(132) / 132, "same"); p = np.argmax(e)
    after = np.where(e[p:] < e.max() * 0.1)[0]; d20 = after[0] / SR if len(after) else len(y) / SR
    X = np.abs(np.fft.rfft(y[:8192] * np.hanning(len(y[:8192])))); f = np.fft.rfftfreq(len(y[:8192]), 1 / SR)
    cen = (X * f).sum() / X.sum()
    LIST.append({"file": name, "seconds": round(len(y) / SR, 3), "use": use})
    print(f"{name:26s} len {len(y)/SR:4.2f}s  -20dB@{d20*1000:4.0f}ms  centroid {cen:5.0f}Hz")

# 1. VERDULERA - good: yank the veg out of the soil: scrape, cork-pop, upward 'bwip!'
y = mix((bp(noise(0.03), 1500, 6000) * env(0.03, 0.002, 0.008), 0, 0.5),
        (snap(0.05, 2500, 0.004), 0.018, 1.0),
        (np.sin(2 * np.pi * np.cumsum(glide(1500, 700, 0.03)) / SR) * env(0.03, 0.0003, 0.008), 0.018, 0.8),
        (chirp(520, 1050, 0.2, "u", "i", tau=0.055, curve=0.45), 0.022, 0.75),
        (modal(1250, *WOOD, 0.1), 0.02, 0.35))
save("verdulera_good", y, "Grabbed a good vegetable: soil scrape, cork-pop, upward 'bwip!'")

# 2. VERDULERA - fail: grabbed a rotten one: squelch + deflated 'bwuhh'
y = mix((squish(0.26), 0, 1.0), (thump(120, 0.15, 0.03), 0, 0.5),
        (chirp(420, 210, 0.32, "a", "u", tau=0.14, curve=1.3), 0.03, 0.6))
save("verdulera_fail", y, "Grabbed a rotten vegetable: wet squelch + deflated 'bwuhh'")

# 3. LAVANDERAS - good: one tug. Sheet SNAP with a tiny 'hup' (short: fine for mashing)
y = mix((snap(0.08, 2200, 0.005), 0, 1.0), (bp(noise(0.09), 400, 2600) * env(0.09, 0.0006, 0.025), 0, 0.8),
        (thump(140, 0.08, 0.018), 0, 0.45), (chirp(600, 780, 0.09, "u", "a", tau=0.03, curve=0.5), 0.004, 0.35))
save("lavanderas_good", y, "Each successful tug: crisp sheet snap + tiny 'hup' (short, mash-friendly)")

# 4. LAVANDERAS - fail: you get yanked / slip: whoosh, slap-thud, sad spring droop
y = mix((whoosh(0.14, 1500, 450, 1.6), 0, 0.3), (bp(noise(0.06), 500, 4500) * env(0.06, 0.0004, 0.01), 0.12, 1.0), (thump(110, 0.2, 0.05), 0.12, 0.8),
        (boing(420, 210, 0.4, 0.2, 16, 0.14), 0.13, 0.6))
save("lavanderas_fail", y, "Opponent yanks you / you lose ground: whoosh, slap-thud, drooping boing")

# 5. BARQUILLERA - good: wheel stops on a high number: ratchet clack + bright TING with an up-glint
y = mix((modal(1900, *WOOD, 0.08), 0, 0.6), (snap(0.03, 3000, 0.003), 0, 0.6),
        (modal(2200, *METAL_HI, 0.45), 0.012, 0.75),
        (np.sin(2 * np.pi * np.cumsum(glide(2400, 3300, 0.12, 0.4)) / SR) * env(0.12, 0.001, 0.05), 0.012, 0.25))
save("barquillera_good", y, "Wheel lands on a high number: wooden clack + bright ting")

# 6. BARQUILLERA - fail: dull CLONK then a wobbly spring 'bwoiiing' down
y = mix((modal(470, *TIN, 0.25), 0, 0.8), (modal(1100, *WOOD, 0.08), 0, 0.5), (snap(0.03, 2000, 0.004), 0, 0.5),
        (boing(640, 300, 0.5, 0.22, 18, 0.17), 0.03, 0.6))
save("barquillera_fail", y, "Wheel lands on a low number: dull clonk + wobbly 'bwoing'")

# 7. AGUADOR - good: drop into the cup: 'bloop' + glass tink
y = mix((snap(0.02, 3000, 0.002), 0, 0.3), (bubble(620, 0.07, 2.6, 0.022), 0.004, 1.0),
        (modal(3150, [1, 2.32, 4.25], [1, 0.4, 0.2], [0.12, 0.05, 0.025], 0.25), 0.012, 0.4),
        (bubble(980, 0.05, 2.0, 0.012), 0.05, 0.35))
save("aguador_good", y, "Correct button / order served: water 'bloop' + glass tink")

# 8. AGUADOR - fail: tin cup clatter, splash, droopy 'bwuh'
y = mix((modal(540, *TIN, 0.3), 0, 0.8), (modal(560, *TIN, 0.3), 0.045, 0.45), (modal(530, *TIN, 0.3), 0.08, 0.25),
        (bp(noise(0.25), 500, 5000) * env(0.25, 0.004, 0.06), 0.02, 0.6),
        *[(bubble(rng.uniform(1300, 2600), 0.03, 1.6, 0.008), 0.03 + rng.uniform(0, 0.15), 0.15) for _ in range(8)],
        (chirp(300, 160, 0.3, "o", "u", tau=0.12, curve=1.2), 0.06, 0.4))
save("aguador_fail", y, "Wrong button: tin cup clatters, water spills, droopy 'bwuh'")

# preview: each good then its fail
order = ["verdulera", "lavanderas", "barquillera", "aguador"]
parts = []
for g in order:
    for k in ("good", "fail"):
        _, w = wavfile.read(f"{OUT}/{g}_{k}.wav"); parts += [w.astype(float) / 32767, np.zeros(int(0.45 * SR))]
    parts.append(np.zeros(int(0.6 * SR)))
r = np.concatenate(parts)
wavfile.write("/tmp/claude-0/-home-claude/ca1cff18-dd47-5d53-95f8-1eb52825e089/scratchpad/prev.wav", SR, (r * 32767).astype(np.int16))
subprocess.run(["ffmpeg", "-loglevel", "error", "-y", "-i", "/tmp/claude-0/-home-claude/ca1cff18-dd47-5d53-95f8-1eb52825e089/scratchpad/prev.wav",
                "-b:a", "192k", f"{OUT}/sfx_v2_preview.mp3"], check=True)
json.dump(LIST, open(f"{OUT}/sound_list.json", "w"), indent=1)
