using System;
using System.Collections.Generic;

[Serializable]
public class LeaderboardEntry //Para guardar un "Leaderoard" local
{
    public string playerName;
    public int score;

    public LeaderboardEntry(string name, int newScore)
    {
        playerName = name;
        score = newScore;
    }
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}


