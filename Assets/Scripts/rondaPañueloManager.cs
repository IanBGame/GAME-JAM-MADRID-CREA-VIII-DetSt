using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using TMPro;

public class rondaPañueloManager : MonoBehaviour
{
    private int letterNumber;
    private int pressedNumber;
    private int currentScore;

    private float pressedTimer;

    public bool isInRound;

    [Header("¡¡¡EL VALOR SE REDUCE POR 50 CADA SEGUNDO!!!")]

    [SerializeField] private float minTimer = 200;
    [SerializeField] private float maxTimer = 500;

    [SerializeField] TMP_Text textoTecla;

    private GameManager gameManager;

    private void FixedUpdate() //ESTO CORRE 50 VECES POR SEGUNDO O CADA 0.02 SEGUNDOS PARA QUE AJUSTEIS LOS VALORES RESPECTIVAMENTE PORFA
    {
        if(isInRound)
        {
            pressedTimer--;
        }
        
    }

    private void RandomizeLetter()
    {
        //Se randomiza (1-4) el numero letterNumber y luego dependiendo del numero que sea
        letterNumber = Random.Range(1, 5);
        //D-F-J-K
        //Se randomiza también pressedTimer (4-10 secs max?)
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

    private void HasHitInputRight()
    {
        gameManager.AddScoreMedium();
    }

    private void HasHitInputWrong()
    {
        gameManager.SubtractScore();
    }

}
