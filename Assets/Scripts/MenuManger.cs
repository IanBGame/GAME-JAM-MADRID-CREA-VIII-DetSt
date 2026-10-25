using UnityEngine;

public class MenuManger : MonoBehaviour
{
    [SerializeField] private LocalLeaderboard leaderboard; 
    [SerializeField] private LeaderboardUI leaderboardUI;
    [SerializeField] private GameManager gameManager;
    
    void Start()
    {
        leaderboardUI.UpdateLeaderboardVisuals();
        gameManager.ResetScores();
    }
}
