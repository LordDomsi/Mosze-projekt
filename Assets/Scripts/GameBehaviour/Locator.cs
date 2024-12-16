using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locator : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) //locator felv�tele amikor a player hozz��r
    {
        if (collision.gameObject.tag == "Player")
        {
            LocatorSpawner.Instance.Pickup();
            AudioManager.Instance.PlaySFX(AudioManager.SFX_enum.PICKUP_LOCATOR);
            Destroy(this.gameObject);
        }
    }
}
