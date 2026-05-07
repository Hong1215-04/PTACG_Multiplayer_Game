using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    public Camera playerCamera;

    private Movement movement;

    void Start()
    {
        movement = GetComponentInChildren<Movement>();

        if (photonView.IsMine)
        {
            movement.enabled = true;

            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(true);
            }
        }
        else
        {
            movement.enabled = false;

            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(false);
            }
        }
    }
}