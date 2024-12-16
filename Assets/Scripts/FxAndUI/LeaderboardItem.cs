using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI score;

    public void Init(string name, int score, int rank) // leaderboard elemek frissítése
    {
        this.userName.SetText(name);
        this.score.SetText(score.ToString());
        this.rank.SetText(rank.ToString() + ".");
    }
}
