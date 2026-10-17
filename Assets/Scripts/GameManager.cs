using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int totalScore;

    public int aplastarScore;
    public int pañueloScore;
    public int ruletaScore;

    [SerializeField] private string rondaAplastarScene = "RondaAplastar";
    [SerializeField] private string rondaPañueloScene = "RondaPañuelo";
    [SerializeField] private string rondaRuletaScene = "RondaRuleta";
    [SerializeField] private string scoreScene = "ScoreScene";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnRondaAplastarEnd()
    {
        aplastarScore = totalScore;
    }

    public void OnRondaPañueloEnd()
    {
        pañueloScore = totalScore - aplastarScore;
    }

    public void OnRondaRuletaEnd()
    {
        ruletaScore = totalScore - aplastarScore - pañueloScore;
    }

    public void AddScoreLow()
    {
        totalScore += 5;
    }

    public void AddScoreMedium()
    {
        totalScore += 10;
    }

    public void AddScoreHigh()
    {
        totalScore += 15;
    }

    public void AddScoreVeryHigh()
    {
        totalScore += 20;
    }

    public void AddScorePerfect()
    {
        totalScore += 25;
    }

    public void GoToRondaAplastar()
    {
        SceneManager.LoadScene(rondaAplastarScene);
    }

    public void GoToRondaPañuelo()
    {
        SceneManager.LoadScene(rondaPañueloScene);
    }

    public void GoToRondaRuleta()
    {
        SceneManager.LoadScene(rondaRuletaScene);
    }

    public void GoToScoreScene()
    {
        SceneManager.LoadScene(scoreScene);
    }
}
