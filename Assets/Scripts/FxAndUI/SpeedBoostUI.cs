using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBoostUI : MonoBehaviour
{
    [SerializeField] private Image PowerUpSprite;

    private bool displayingSpeedBoost = false;   

    private void Start()
    {
        PlayerMovement.Instance.OnSpeedBoostPickup += PlayerMovement_OnSpeedBoostPickup;
        this.gameObject.SetActive(false);
    }

    private void PlayerMovement_OnSpeedBoostPickup(object sender, PlayerMovement.OnPowerupPickupEventArgs e)
    {
        StartPowerUpUIDisplay(e.powerUpTimeLimit);
    }

    public void StartPowerUpUIDisplay(float animLength)
    {
        this.gameObject.SetActive(true);
        PopupManager.Instance.StartSpeedBoostUIAnimOpen();
        PowerUpSprite.fillAmount = 1f;
        if(displayingSpeedBoost) StopAllCoroutines();
        StartCoroutine(DisplayTimer(animLength));
    }

    private IEnumerator DisplayTimer(float animLength)
    {
        displayingSpeedBoost = true;
        float time = animLength;
        while (time > 0)
        {
            time -= Time.deltaTime;

            PowerUpSprite.fillAmount = time / animLength;

            yield return null;
        }
        PopupManager.Instance.StartSpeedBoostUIAnimClose();
        yield return new WaitForSeconds(PopupManager.Instance.powerUpUIAnimSpeed);
        displayingSpeedBoost = false;
        this.gameObject.SetActive(false);

    }

    private void OnDestroy()
    {
        PlayerMovement.Instance.OnThreeWayPowerUpPickup -= PlayerMovement_OnSpeedBoostPickup;
    }
}
