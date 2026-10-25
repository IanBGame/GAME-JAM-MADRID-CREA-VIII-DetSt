using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int totalScore;
    public int tempScore;

    public int aplastarScore;
    public int panueloScore;
    public int ruletaScore;
    public int aguadorScore;

    public int scoreLowAdd;
    public int scoreMediumAdd;
    public int scoreHighAdd;
    public int scoreSubtract;
    public int scoreSubtractBig;

    [SerializeField] private string rondaAplastarScene = "RondaAplastar";
    [SerializeField] private string rondaPanueloScene = "RondaPanuelo";
    [SerializeField] private string rondaRuletaScene = "RondaRuleta";
    [SerializeField] private string rondaAguadoresScene = "RondaAguadores";
    [SerializeField] private string creditos = "creditos";
    [SerializeField] private string scoreScene = "ScoreScene";

    [SerializeField] private LocalLeaderboard leaderboard; //Necesario para guardar el score en el leaderboard al finalizar la partida

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

    public void ResetScores()
    {
        aplastarScore = 0;
        panueloScore = 0;
        ruletaScore = 0;
        aguadorScore = 0;
        totalScore = 0;
    }

    public int GetScore()
    {
        return totalScore;
    }

    public void ResetTempScore()
    {
        tempScore = 0;
    }

    public int GetAplastarScore()
    {
        aplastarScore = tempScore;
        return aplastarScore;
    }

    public int GetPanueloScore()
    {
        panueloScore = tempScore;
        return panueloScore;
    }

    public int GetRuletaScore()
    {
        ruletaScore = tempScore;
        return ruletaScore;
    }

    public int GetAguadorScore()
    {
        return aguadorScore;
    }

    public void SaveAguadorScore()
    {
        aguadorScore = tempScore;
    }

    public void GetTotalScore()
    {
        totalScore = aguadorScore + panueloScore + ruletaScore + aplastarScore;
    }

    public void AddScoreLow()
    {
        tempScore += scoreLowAdd;
    }

    public void AddScoreMedium()
    {
        tempScore += scoreMediumAdd;
    }

    public void AddScoreHigh()
    {
        tempScore += scoreHighAdd;
    }
    
    public void SubtractScore()
    {
        tempScore -= scoreSubtract;
        if (tempScore < 0)
        {
            tempScore = 0;
        }
    }

    public void SubtractScoreBig()
    {
        tempScore -= scoreSubtractBig;
        if (tempScore < 0)
        {
            tempScore = 0;
        }
    }

    public void GoToRondaAplastar()
    {
        SceneManager.LoadScene(rondaAplastarScene);
    }

    public void GoToRondaPanuelo()
    {
        SceneManager.LoadScene(rondaPanueloScene);
    }

    public void GoToRondaRuleta()
    {
        SceneManager.LoadScene(rondaRuletaScene);
    }

    public void GoToRondaAguadores()
    {
        SceneManager.LoadScene(rondaAguadoresScene);
    }
    public void GoToCreditos()
    {
        SceneManager.LoadScene(creditos);
    }

    public void GoToScoreScene()
    {
        SceneManager.LoadScene(scoreScene);
        leaderboard.AddEntry("Implementar hueco para nombre", totalScore);
    }
}
