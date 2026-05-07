using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerCamera;

    //private Movement movement;

    void Start()
    {
        //movement = GetComponentInChildren<Movement>();

        if (photonView.IsMine)
        {
            transform.GetComponentInChildren<Movement>().enabled = true;

            if (playerCamera != null)
            {
                //playerCamera.gameObject.SetActive(true);
                playerCamera.GetComponent<Camera>().enabled = true;
            }
        }
        else
        {
            //movement.enabled = false;
            transform.GetComponentInChildren<Movement>().enabled = false;

            if (playerCamera != null)
            {
                //playerCamera.gameObject.SetActive(false);
                playerCamera.GetComponent<Camera>().enabled = false;
            }
        }
    }
}