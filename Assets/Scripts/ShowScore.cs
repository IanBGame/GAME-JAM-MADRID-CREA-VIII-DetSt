using UnityEngine;
using TMPro;

public class ShowScore : MonoBehaviour
{
    private int aplastarScore;
    private int panueloScore;
    private int ruletaScore;
    private int aguadorScore;
    private int total;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private TMP_Text[] scoreTexts;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        SetScores();
        SetTexts();
    }

    private void SetScores()
    {
        gameManager.GetTotalScore();
        aplastarScore = gameManager.aplastarScore;
        panueloScore = gameManager.panueloScore;
        ruletaScore = gameManager.ruletaScore;
        aguadorScore = gameManager.aguadorScore;
        total = gameManager.totalScore;
    }

    private void SetTexts()
    {
        scoreTexts[0].text = "Puntuacion de la primera ronda: " + aplastarScore;
        scoreTexts[1].text = "Puntuacion de la segunda ronda: " + panueloScore;
        scoreTexts[2].text = "Puntuacion de la tercera ronda: " + ruletaScore;
        scoreTexts[3].text = "Puntuacion de la cuarta ronda: " + aguadorScore;
        scoreTexts[4].text = "Puntuacion total: " + total;
    }

}