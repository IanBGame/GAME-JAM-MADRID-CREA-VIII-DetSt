using UnityEngine;

public class MenuManger : MonoBehaviour
{
    [SerializeField] private LocalLeaderboard leaderboard; 
    [SerializeField] private LeaderboardUI leaderboardUI;
    
    void Start()
    {
        leaderboardUI.UpdateLeaderboardVisuals();
    }
}
