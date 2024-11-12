using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngineFX : MonoBehaviour
{
    [SerializeField] private GameObject LeftEngine;
    [SerializeField] private GameObject RightEngine;

    private PlayerMovement playerMovement;

    private bool forward = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.OnForwardPressed += PlayerMovement_OnForwardPressed;
        playerMovement.OnForwardStopped += PlayerMovement_OnForwardStopped;
        playerMovement.OnTurnLeft += PlayerMovement_OnTurnLeft;
        playerMovement.OnTurnRight += PlayerMovement_OnTurnRight;
        playerMovement.OnStopTurn += PlayerMovement_OnStopTurn;

        LeftEngine.SetActive(false);
        RightEngine.SetActive(false);
    }

    private void PlayerMovement_OnStopTurn(object sender, System.EventArgs e)
    {
        if (!forward)
        {
            LeftEngine.SetActive(false);
            RightEngine.SetActive(false);
        }
    }

    private void PlayerMovement_OnTurnRight(object sender, System.EventArgs e)
    {
        if (!forward)
        {
            LeftEngine.SetActive(true);
        }
    }

    private void PlayerMovement_OnTurnLeft(object sender, System.EventArgs e)
    {
        if (!forward)
        {
            RightEngine.SetActive(true);
        }
    }

    private void PlayerMovement_OnForwardStopped(object sender, System.EventArgs e)
    {
        LeftEngine.SetActive(false);
        RightEngine.SetActive(false);
        forward = false;
    }

    private void PlayerMovement_OnForwardPressed(object sender, System.EventArgs e)
    {
        LeftEngine.SetActive(true);
        RightEngine.SetActive(true);
        forward = true;
    }
}
