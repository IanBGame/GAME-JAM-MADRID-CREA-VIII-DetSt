using UnityEngine;
using TMPro;

public class ShowScore : MonoBehaviour
{
    private int aplastarScore;
    private int panueloScore;
    private int ruletaScore;
    private int total;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private TMP_Text[] scoreTexts;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Awake()
    {
        SetScores();
        SetTexts();
    }

    private void SetScores()
    {
        aplastarScore = gameManager.GetAplastarScore();
        panueloScore = gameManager.GetPanueloScore();
        ruletaScore = gameManager.GetRuletaScore();
        total = gameManager.totalScore;
    }

    private void SetTexts()
    {
        scoreTexts[0].text = "Puntuacion de la primera ronda: " + aplastarScore;
        scoreTexts[0].text = "Puntuacion de la segunda ronda: " + panueloScore;
        scoreTexts[0].text = "Puntuacion de la primera ronda: " + ruletaScore;
        scoreTexts[0].text = "Puntuacion de la primera ronda: " + total;
    }

}