using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int totalScore;

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

    public int GetScore()
    {
        return totalScore;
    }

    public int GetAplastarScore()
    {
        aplastarScore = totalScore;
        return aplastarScore;
    }

    public int GetPanueloScore()
    {
        panueloScore = totalScore - aplastarScore;
        return panueloScore;
    }

    public int GetRuletaScore()
    {
        ruletaScore = totalScore - aplastarScore - panueloScore;
        return ruletaScore;
    }
    public int GetAguadorScore()
    {
        aguadorScore = totalScore - aplastarScore - panueloScore - ruletaScore;
        return aguadorScore;
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

    public void SubtractScoreBig()
    {
        totalScore -= scoreSubtractBig;
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
