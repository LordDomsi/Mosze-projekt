using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    [SerializeField] private Image PowerUpSprite;

    private void Start()
    {
        PlayerMovement.Instance.OnThreeWayPowerUpPickup += PlayerMovement_OnThreeWayPowerUpPickup;
        this.gameObject.SetActive(false);
    }

    private void PlayerMovement_OnThreeWayPowerUpPickup(object sender, PlayerMovement.OnPowerupPickupEventArgs e)
    {
        StartPowerUpUIDisplay(e.powerUpTimeLimit);
    }

    public void StartPowerUpUIDisplay(float animLength) //vizu�lis timer megjelen�t�se powerupn�l
    {
        this.gameObject.SetActive(true);
        PopupManager.Instance.StartPowerUpUIAnimOpen();
        PowerUpSprite.fillAmount = 1f;
        StartCoroutine(DisplayTimer(animLength));
    }

    private IEnumerator DisplayTimer(float animLength)
    {
        float time = animLength;
        while (time > 0)
        {
            time -= Time.deltaTime;

            PowerUpSprite.fillAmount = time/animLength;

            yield return null;
        }
        PopupManager.Instance.StartPowerUpUIAnimClose();
        yield return new WaitForSeconds(PopupManager.Instance.powerUpUIAnimSpeed);
        this.gameObject.SetActive(false);

    }

    private void OnDestroy()
    {
        PlayerMovement.Instance.OnThreeWayPowerUpPickup -= PlayerMovement_OnThreeWayPowerUpPickup;
    }
}
