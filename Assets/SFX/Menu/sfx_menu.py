"""Fiesta Remix - menu SFX (Rhythm Heaven-style, original synthesis). Reuses helpers from sfx_v2.py."""
import os, sys, subprocess, json
import numpy as np
from scipy.io import wavfile
OUT = sys.argv[1] if len(sys.argv) > 1 else "out_sfx_menu"
src = open(os.path.join(os.path.dirname(os.path.abspath(__file__)), "sfx_v2.py")).read()
exec(src[src.index("import os"):src.index("# 1. VERDULERA")].replace('OUT = sys.argv[1] if len(sys.argv) > 1 else "out_sfx_v2"', ""))
os.makedirs(OUT, exist_ok=True)

def tone(f0, f1, d, tau, harm=(1, 0.35, 0.12)):
    t = tv(d); ph = 2 * np.pi * np.cumsum(glide(f0, f1, d, 0.4)) / SR
    return sum(g * np.sin((k + 1) * ph) for k, g in enumerate(harm)) * env(d, 0.0008, tau)

# move: tiny tuned wooden 'tok' with a flick up
y = mix((modal(1320, *WOOD, 0.06), 0, 0.8), (tone(1320, 1760, 0.05, 0.014), 0.002, 0.5), (snap(0.02, 3500, 0.002), 0, 0.25))
save("ui_move", y, "Cursor moves between options (very short tuned tok)", -7)
# select: cartoon 'pon!' up-glide + clack + glass tink
y = mix((snap(0.03, 2500, 0.003), 0, 0.5), (modal(1760, *WOOD, 0.07), 0, 0.5),
        (chirp(560, 940, 0.16, "u", "a", tau=0.05, curve=0.4), 0.005, 0.8),
        (modal(2640, [1, 2.32, 4.25], [1, 0.4, 0.2], [0.09, 0.04, 0.02], 0.2), 0.03, 0.3))
save("ui_select", y, "Confirm a menu option: 'pon!' up-glide", -3)
# back: same family, gliding down and softer
y = mix((modal(990, *WOOD, 0.07), 0, 0.6), (chirp(720, 430, 0.16, "a", "u", tau=0.05, curve=0.8), 0.004, 0.75))
save("ui_back", y, "Back / cancel: soft down-glide 'bwoo'", -5)
# start: two-step 'pa-PAN!' up chirps + bright ting
y = mix((bp(noise(0.04), 800, 5000) * env(0.04, 0.0004, 0.004), 0, 0.35), (modal(1320, *WOOD, 0.08), 0, 0.7),
        (chirp(440, 660, 0.09, "u", "a", tau=0.035, curve=0.4), 0.0, 0.55),
        (bp(noise(0.04), 800, 5000) * env(0.04, 0.0004, 0.004), 0.1, 0.4), (modal(1760, *WOOD, 0.08), 0.1, 0.7),
        (chirp(660, 1320, 0.22, "o", "i", tau=0.07, curve=0.35), 0.1, 0.85),
        (modal(2640, *METAL_HI, 0.4), 0.11, 0.25))
save("ui_start", y, "Start the remix: two-step 'pa-PAN!' + ting", -1)

parts = []
for it in LIST:
    _, w = wavfile.read(f"{OUT}/{it['file']}.wav"); parts += [w.astype(float) / 32767, np.zeros(int(0.5 * SR))]
r = np.concatenate(parts)
wavfile.write(f"{OUT}/_p.wav", SR, (r * 32767).astype(np.int16))
subprocess.run(["ffmpeg", "-loglevel", "error", "-y", "-i", f"{OUT}/_p.wav", "-b:a", "192k", f"{OUT}/menu_sfx_preview.mp3"], check=True)
os.remove(f"{OUT}/_p.wav")
json.dump(LIST, open(f"{OUT}/sound_list.json", "w"), indent=1)
