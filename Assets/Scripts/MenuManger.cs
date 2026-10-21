using UnityEngine;

public class MenuManger : MonoBehaviour
{
    [SerializeField] private LocalLeaderboard leaderboard; //Necesario para cargar leaderboard en menu
    [SerializeField] private LeaderboardUI leaderboardUI;
    
    public void StartGame()
    {
        //Hacer codigo de empezar juego, hacer que vuelva al menu despues
    }
    public void OpenCredits()
    {
        //Hacer codigo de abrir creditos, hacer que vuelva al menu despues
    }
    
    void Start()
    {
        leaderboardUI.UpdateLeaderboardVisuals();
    }
}
