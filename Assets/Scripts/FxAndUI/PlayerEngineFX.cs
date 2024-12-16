using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngineFX : MonoBehaviour
{
    [SerializeField] private GameObject LeftEngine;
    [SerializeField] private GameObject RightEngine;

    private PlayerMovement playerMovement;

    [SerializeField] private AudioSource engineSource;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.OnForwardPressed += PlayerMovement_OnForwardPressed;
        playerMovement.OnForwardStopped += PlayerMovement_OnForwardStopped;
        PlayerHealthManager.Instance.OnPlayerDeath += PlayerHealthManager_OnPlayerDeath;

        LeftEngine.SetActive(false);
        RightEngine.SetActive(false);
        if(engineSource.isPlaying) engineSource.Stop();
    }

    private void PlayerHealthManager_OnPlayerDeath(object sender, System.EventArgs e)
    {
        LeftEngine.SetActive(false);
        RightEngine.SetActive(false);
        engineSource.Stop();
    }

    private void PlayerMovement_OnForwardStopped(object sender, System.EventArgs e)
    {

        LeftEngine.SetActive(false);
        RightEngine.SetActive(false);
        engineSource.Stop();
    }

    //ha player elõre megy akkor megjeleníti az engine effektet és lejátsza a hangeffektet
    private void PlayerMovement_OnForwardPressed(object sender, System.EventArgs e)
    {
        if (!DialogueBoxUI.Instance.displayingText && !GameOverUI.Instance.gameOver)
        {
            LeftEngine.SetActive(true);
            RightEngine.SetActive(true);
            if (!engineSource.isPlaying) engineSource.Play();
        }
        else
        {
            engineSource.Stop();
        }
    }

    private void OnDestroy()
    {
        playerMovement.OnForwardPressed -= PlayerMovement_OnForwardPressed;
        playerMovement.OnForwardStopped -= PlayerMovement_OnForwardStopped;
    }
}
