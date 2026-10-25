using UnityEngine;
using System.Collections.Generic;

public class LocalLeaderboard : MonoBehaviour
{
    public static LocalLeaderboard instance;
    
    private const string SaveKey = "LocalLeaderboardData";
    [SerializeField] private int maxEntries = 50; 

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

    //Cargar leaderboard 
    public List<LeaderboardEntry> LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(SaveKey))
        {
            string json = PlayerPrefs.GetString(SaveKey);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
            return data.entries;
        }

        return new List<LeaderboardEntry>();
    }

    public void AddEntry(string name, int score)
    {
        List<LeaderboardEntry> currentEntries = LoadLeaderboard();

        currentEntries.Add(new LeaderboardEntry(name, score));

        currentEntries.Sort((a, b) => b.score.CompareTo(a.score));

        if (currentEntries.Count > maxEntries)
        {
            currentEntries.RemoveRange(maxEntries, currentEntries.Count - maxEntries);
        }

        LeaderboardData dataToSave = new LeaderboardData { entries = currentEntries };
        string json = JsonUtility.ToJson(dataToSave);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }
    
    //Opcional para borrar leaderboard
    public void ClearLeaderboard()
    {
        PlayerPrefs.DeleteKey(SaveKey);
    }
}
