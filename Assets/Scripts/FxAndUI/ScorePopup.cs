using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    public static ScorePopup Instance;
    [SerializeField] private GameObject popupPrefab;
    private void Awake()
    {
        Instance = this;
    }

    public void Popup(Transform position, int score)
    {
        GameObject newPopup = Instantiate(popupPrefab, position.position, Quaternion.identity);
        newPopup.GetComponent<ScorePopupDisplay>().score = score;
    }
}
