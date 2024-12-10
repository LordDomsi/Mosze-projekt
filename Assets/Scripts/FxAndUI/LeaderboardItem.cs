using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI score;

    public void Init(string name, int score, int rank)
    {
        this.name.SetText(name);
        this.score.SetText(score.ToString());
        this.rank.SetText(rank.ToString() + ".");
    }
}
