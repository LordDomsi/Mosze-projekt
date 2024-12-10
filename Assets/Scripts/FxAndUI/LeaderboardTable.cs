using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardTable : MonoBehaviour
{
    [SerializeField] private Transform EntryTemplate;
    [SerializeField] private Transform EntryContainer;

    private List<Transform> items = new List<Transform>();
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SaveManager.Instance.loaded == true);
        RefreshLeaderboard(); //adatok betöltése után frissíti a leaderboardot
    }

    public void RefreshLeaderboard()//frissíti a leaderboardot
    {
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i].gameObject); //elõször törli az elemeket
        }

        var leaderboardSorted = SaveManager.Instance.saveData.leaderboardData;
        leaderboardSorted.Sort((a, b) => b.score.CompareTo(a.score)); // elrendezi sorba az új adatokat

        for (int i = 0; i < leaderboardSorted.Count; i++) //létrehozza az új elemeket
        {
            Transform transform = Instantiate(EntryTemplate, EntryContainer);

            transform.GetComponent<LeaderboardItem>().Init(leaderboardSorted[i].name, leaderboardSorted[i].score, i + 1);
            transform.gameObject.SetActive(true);
            items.Add(transform);
        }
    }
}
