using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject playerCamera1;
    public GameObject playerCamera2;

    //private Movement movement;

    void Start()
    {
        // 从子物体找 PhotonView
        PhotonView pv = GetComponentInChildren<PhotonView>();
        
        if (pv.IsMine)
        {
            GetComponentInChildren<Movement>().enabled = true;
            
            playerCamera2.GetComponent<Camera>().enabled = true;
        }
        else
        {
            GetComponentInChildren<Movement>().enabled = false;
            playerCamera1.GetComponent<Camera>().enabled = false;
            //if (playerCamera != null)
                //playerCamera.GetComponent<Camera>().enabled = false;
        }
    }
}