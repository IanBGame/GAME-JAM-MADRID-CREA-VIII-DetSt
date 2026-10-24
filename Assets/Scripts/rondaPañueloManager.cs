using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class rondaPañueloManager : MonoBehaviour
{
    [SerializeField] private GameObject objetoReglas;

    private int letterNumber;
    private int pressedNumber;
    private int currentScore;

    private float pressedTimer;
    private float roundTimer;

    public bool isInRound;

    [SerializeField] private float minTimer = 4f;
    [SerializeField] private float maxTimer = 10f;

    [SerializeField] private float roundDuration = 30f;

    [SerializeField] TMP_Text textoTecla;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (objetoReglas != null)
        {
            StartCoroutine(RutinaQuitarReglas(5.0f));
        }
    }

    private void Update()
    {
        if (!isInRound)
            return;

        pressedTimer -= Time.deltaTime;
        roundTimer -= Time.deltaTime;

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

    private void StartRound()
    {
        isInRound = true;
        roundTimer = roundDuration;

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
            textoTecla.text = "PRESIONA LA D";
        }
        else if(letterNumber == 2 && isInRound)
        {
            textoTecla.text = "PRESIONA LA F";
        }
        else if(letterNumber == 3 && isInRound)
        {
            textoTecla.text = "PRESIONA LA J";
        }
        else if(letterNumber == 4 && isInRound)
        {
            textoTecla.text = "PRESIONA LA K";
        }

        if(!isInRound)
        {
            textoTecla.text = "";
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
        gameManager.AddScoreLow();
    }

    private void HasHitInputWrong()
    {
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
