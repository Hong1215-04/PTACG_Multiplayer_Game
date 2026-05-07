using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] Camera playerCamera;

    //private Movement movement;

    void Start()
    {
        //movement = GetComponentInChildren<Movement>();

        if (photonView.IsMine)
        {
            transform.GetComponentInChildren<Movement>().enabled = true;

            if (playerCamera != null)
            {

                playerCamera.gameObject.SetActive(true);
            }
        }
        else
        {
            //movement.enabled = false;
            transform.GetComponentInChildren<Movement>().enabled = false;

            if (playerCamera != null)
            {
                playerCamera.gameObject.SetActive(false);
            }
        }
    }
}