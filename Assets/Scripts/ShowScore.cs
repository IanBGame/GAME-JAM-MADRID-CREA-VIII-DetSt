using UnityEngine;
using TMPro;

public class ShowScore : MonoBehaviour
{
    private int aplastarScore;
    private int pañueloScore;
    private int ruletaScore;
    private int totalScore;

    [SerializeField] private GameManager gameManager;
    //gameManager = GameObject.Find("GameManager");

    [SerializeField] private TMP_Text[] scoreTexts;

    private void Awake()
    {
        SetScores();
        SetTexts();
    }

    private void SetScores()
    {
        aplastarScore = gameManager.GetAplastarScore();
        pañueloScore = gameManager.GetPañueloScore();
        ruletaScore = gameManager.GetRuletaScore();
        totalScore = gameManager.totalScore;
    }

    private void SetTexts()
    {
        scoreTexts[0].text = "Puntuación de la primera ronda: " + aplastarScore;
        scoreTexts[0].text = "Puntuación de la segunda ronda: " + pañueloScore;
        scoreTexts[0].text = "Puntuación de la primera ronda: " + ruletaScore;
        scoreTexts[0].text = "Puntuación de la primera ronda: " + totalScore;
    }

}