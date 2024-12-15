using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedBoostMultiplierUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI multiplierUI;
    private void Start()
    {
        PlayerMovement.Instance.OnSpeedBoostStart += PlayerMovement_OnSpeedBoostStart;
        PlayerMovement.Instance.OnSpeedBoostEnd += PlayerMovement_OnSpeedBoostEnd;
        multiplierUI.gameObject.SetActive(false);
    }

    private void PlayerMovement_OnSpeedBoostStart(object sender, System.EventArgs e)
    {
        multiplierUI.gameObject.SetActive(true);
    }

    private void PlayerMovement_OnSpeedBoostEnd(object sender, System.EventArgs e)
    {
        multiplierUI.gameObject.SetActive(false);
    }

    

    private void OnDestroy()
    {
        PlayerMovement.Instance.OnSpeedBoostStart -= PlayerMovement_OnSpeedBoostStart;
        PlayerMovement.Instance.OnSpeedBoostEnd -= PlayerMovement_OnSpeedBoostEnd;
    }
}
