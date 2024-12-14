using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public static StatsUI Instance {  get; private set; }

    [SerializeField] private TextMeshProUGUI attackDamageText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI movementSpeedText;
    [SerializeField] private TextMeshProUGUI maxShieldText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateStats()
    {
        attackDamageText.text = PlayerMovement.Instance.GetPlayerDamage().ToString();
        attackSpeedText.text = (1+(1-(PlayerMovement.Instance.GetPlayerShootSpeed()/PlayerMovement.Instance.GetPlayerDefaultShootSpeed()))).ToString();
        movementSpeedText.text = PlayerMovement.Instance.GetPlayerSpeed().ToString();
        maxShieldText.text = PlayerMovement.Instance.GetPlayerMaxShield().ToString();
    }
}
