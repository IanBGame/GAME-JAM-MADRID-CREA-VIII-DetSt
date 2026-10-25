using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class rondaPañueloManager : MonoBehaviour
{
    [SerializeField] private GameObject objetoReglas;

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite sprD;
    [SerializeField] private Sprite sprF;
    [SerializeField] private Sprite sprJ;
    [SerializeField] private Sprite sprK;

    private int letterNumber;
    private int pressedNumber;
    private int currentScore;
    private float scoreTimer = 0f;

    private float pressedTimer;
    private float roundTimer;

    public bool isInRound;

    [SerializeField] private float minTimer = 2f;
    [SerializeField] private float maxTimer = 5f;

    [SerializeField] private float roundDuration = 30f;

    [SerializeField] private TextMeshProUGUI textoTemporizador;
    private GameManager gameManager;

    private void Start()
    {
        
        gameManager = FindFirstObjectByType<GameManager>();
        if (objetoReglas != null)
        {
            StartCoroutine(RutinaQuitarReglas(5.0f));
        }
        playerAnimator.SetBool("diegoTira", false);
        playerAnimator.SetBool("manuTira", false);
    }

    private void Update()
    {

        if (textoTemporizador != null)
        {
            textoTemporizador.text = Mathf.Ceil(roundTimer).ToString() + "s";
        }

        if (!isInRound)
            return;

        pressedTimer -= Time.deltaTime;
        roundTimer -= Time.deltaTime;

        if(isInRound)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 1f) 
            {
                QuitarPuntosPorTiempo();
                scoreTimer = 0f; 
            }
        }

        if (roundTimer <= 0)
        {
            EndRound();
            return;
        }

        if (pressedTimer <= 0)
        {
            RandomizeLetter();
            SetLetterText();
        }

        CheckInput();
    }

    private void QuitarPuntosPorTiempo()
    {     
        if (gameManager != null)
        {
            playerAnimator.SetBool("manuTira", true);
            playerAnimator.SetBool("diegoTira", false);
            CameraShake2D.Instance.Shake(0.15f, 0.3f);
            gameManager.SubtractScore();
        }
    }

    private void StartRound()
    {
        isInRound = true;
        roundTimer = roundDuration;
        scoreTimer = 0f;

        RandomizeLetter();
        SetLetterText();
    }

    private void RandomizeLetter()
    {
        int previousLetter = letterNumber;

        do
        {
            letterNumber = Random.Range(1, 5);
        }
        while (letterNumber == previousLetter);
        //Se randomiza (1-4) el numero letterNumber
        //D-F-J-K
        pressedTimer = Random.Range(minTimer, maxTimer);
    }

    private void SetLetterText()
    {
        if(letterNumber == 1 && isInRound)
        {
            spriteRenderer.sprite = sprD;
        }
        else if(letterNumber == 2 && isInRound)
        {
            spriteRenderer.sprite = sprF;
        }
        else if(letterNumber == 3 && isInRound)
        {
            spriteRenderer.sprite = sprJ;
        }
        else if(letterNumber == 4 && isInRound)
        {
            spriteRenderer.sprite = sprK;
        }
    }
    
    private void CheckInput()
    {
        pressedNumber = 0;

        Keyboard keyboard = Keyboard.current;

        if(keyboard == null)
        {
            return;
        }

        if (keyboard.dKey.wasPressedThisFrame)
        {
            pressedNumber = 1;
        }
        else if (keyboard.fKey.wasPressedThisFrame)
        {
            pressedNumber = 2;
        }
        else if (keyboard.jKey.wasPressedThisFrame)
        {
            pressedNumber = 3;
        }
        else if (keyboard.kKey.wasPressedThisFrame)
        {
            pressedNumber = 4;
        }

        if(pressedNumber != 0)
        {
            if(pressedNumber == letterNumber)
            {
                HasHitInputRight();
            }
            else
            {
                HasHitInputWrong();
            }
        }
    }

    private void HasHitInputRight()
    {
        playerAnimator.SetBool("diegoTira", true);
        playerAnimator.SetBool("manuTira", false);
        CameraShake2D.Instance.Shake(0.15f, 0.3f);
        gameManager.AddScoreLow();
    }

    private void HasHitInputWrong()
    {
        playerAnimator.SetBool("manuTira", true);
        playerAnimator.SetBool("diegoTira", false);
        CameraShake2D.Instance.Shake(0.15f, 0.3f);
        gameManager.SubtractScore();
    }

    private void EndRound()
    {
        isInRound = false;

        SetLetterText();

        gameManager.GetPanueloScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private System.Collections.IEnumerator RutinaQuitarReglas(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        
        if (objetoReglas != null)
        {
            objetoReglas.SetActive(false);
        }
        StartRound();
    }

}
