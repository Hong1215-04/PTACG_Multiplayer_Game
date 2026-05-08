using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    //public GameObject PlayerMesh;
    [SerializeField] GameObject playerCamera;
    [SerializeField] KeyCode SwapKey;
    [SerializeField] float Cooldown;

    bool canDo;
    private float time;
    public bool swap;

    //private Movement movement;

    void Start()
    {
        swap = false;
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

    void Swapping()
    {
        if (!swap)
        {
            if (this.gameObject.layer == LayerMask.NameToLayer("Player1"))
            {
                playerCamera.GetComponent<Camera>().enabled = false;
                swap = true;
            }
            else if (this.gameObject.layer == LayerMask.NameToLayer("Player2"))
            {
                playerCamera.GetComponent<Camera>().enabled = true;
                swap = true;
            }
        }
        if (swap)
        {
            if (this.gameObject.layer == LayerMask.NameToLayer("Player1"))
            {
                playerCamera.GetComponent<Camera>().enabled = true;
                swap = true;
            }
            else if (this.gameObject.layer == LayerMask.NameToLayer("Player2"))
            {
                playerCamera.GetComponent<Camera>().enabled = false;
                swap = true;
            }
        }
    }

    private void Update()
    {
        if (!canDo)
        {
            time += Time.deltaTime;
        }

        if (time > Cooldown)
        {
            canDo = true;
        }
        if (Input.GetKeyDown(SwapKey))
        {
            Swapping();
        }
    }
}