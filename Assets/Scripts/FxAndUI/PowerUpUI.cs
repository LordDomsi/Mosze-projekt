using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    [SerializeField] private Image PowerUpSprite;

    private bool displayingTriShot = false;

    private void Start()
    {
        PlayerMovement.Instance.OnThreeWayPowerUpPickup += PlayerMovement_OnThreeWayPowerUpPickup;
        this.gameObject.SetActive(false);
    }

    private void PlayerMovement_OnThreeWayPowerUpPickup(object sender, PlayerMovement.OnPowerupPickupEventArgs e)
    {
        StartPowerUpUIDisplay(e.powerUpTimeLimit);
    }

    public void StartPowerUpUIDisplay(float animLength) //vizuális timer megjelenítése powerupnál
    {
        this.gameObject.SetActive(true);
        PopupManager.Instance.StartPowerUpUIAnimOpen();
        PowerUpSprite.fillAmount = 1f;
        if(displayingTriShot ) StopAllCoroutines();
        StartCoroutine(DisplayTimer(animLength));
    }

    private IEnumerator DisplayTimer(float animLength)
    {
        displayingTriShot = true;
        float time = animLength;
        while (time > 0)
        {
            time -= Time.deltaTime;

            PowerUpSprite.fillAmount = time/animLength;

            yield return null;
        }
        PopupManager.Instance.StartPowerUpUIAnimClose();
        yield return new WaitForSeconds(PopupManager.Instance.powerUpUIAnimSpeed);
        displayingTriShot = false;
        this.gameObject.SetActive(false);

    }

    private void OnDestroy()
    {
        PlayerMovement.Instance.OnThreeWayPowerUpPickup -= PlayerMovement_OnThreeWayPowerUpPickup;
    }
}
