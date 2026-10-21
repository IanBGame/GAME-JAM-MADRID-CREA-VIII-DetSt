using UnityEngine;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private LocalLeaderboard leaderboard;
    [SerializeField] private GameObject rowPrefab;      
    [SerializeField] private Transform rowsContainer;  

    private List<GameObject> activeRows = new List<GameObject>();

    // Llamar al volver al menu, donde esta el leaderboard
    public void UpdateLeaderboardVisuals()
    {
        foreach (GameObject row in activeRows)
        {
            Destroy(row);
        }
        activeRows.Clear();

        List<LeaderboardEntry> scores = leaderboard.LoadLeaderboard();

        for (int i = 0; i < scores.Count; i++)
        {
            GameObject newRowObj = Instantiate(rowPrefab, rowsContainer);
            activeRows.Add(newRowObj);

            LeaderboardRow rowScript = newRowObj.GetComponent<LeaderboardRow>();
            if (rowScript != null)
            {
                rowScript.SetRowData(i + 1, scores[i].playerName, scores[i].score);
            }
        }
    }
}

