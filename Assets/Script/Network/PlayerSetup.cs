using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    //public GameObject PlayerMesh;
    [SerializeField] GameObject playerCamera;

    //private Movement movement;

    void Start()
    {
        // 从子物体找 PhotonView
        PhotonView pv = GetComponentInChildren<PhotonView>();
        
        if (pv.IsMine)
        {
            // GetComponentInChildren<Movement>().enabled = true;
            if (playerCamera != null)
                playerCamera.GetComponent<Camera>().enabled = true;
        }
        else
        {
            // GetComponentInChildren<Movement>().enabled = false;
            if (playerCamera != null)
            {
                if (this.gameObject.layer == LayerMask.NameToLayer("Player2"))
                {
                    playerCamera.GetComponent<Camera>().enabled = false;
                }
            }  
        }
    }

    void Swapping(Transform Anotherplayer)
    {
        
    }
}