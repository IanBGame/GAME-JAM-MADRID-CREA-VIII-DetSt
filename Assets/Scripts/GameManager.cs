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

    public int scoreLowAdd;
    public int scoreMediumAdd;
    public int scoreHighAdd;
    public int scoreSubtract;

    [SerializeField] private string rondaAplastarScene = "RondaAplastar";
    [SerializeField] private string rondaPañueloScene = "RondaPañuelo";
    [SerializeField] private string rondaRuletaScene = "RondaRuleta";
    [SerializeField] private string rondaAguadoresScene = "RondaAguadores";
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

    public int GetAplastarScore()
    {
        aplastarScore = totalScore;
        return aplastarScore;
    }

    public int GetPañueloScore()
    {
        pañueloScore = totalScore - aplastarScore;
        return pañueloScore;
    }

    public int GetRuletaScore()
    {
        ruletaScore = totalScore - aplastarScore - pañueloScore;
        return ruletaScore;
    }

    public void AddScoreLow()
    {
        totalScore += scoreLowAdd;
    }

    public void AddScoreMedium()
    {
        totalScore += scoreMediumAdd;
    }

    public void AddScoreHigh()
    {
        totalScore += scoreHighAdd;
    }
    
    public void SubtractScore()
    {
        totalScore -= scoreSubtract;
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
    public void GoToRondaAguadores()
    {
        SceneManager.LoadScene(rondaAguadoresScene);
    }

    public void GoToScoreScene()
    {
        SceneManager.LoadScene(scoreScene);
    }
}
