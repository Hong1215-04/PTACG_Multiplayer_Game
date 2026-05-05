using UnityEngine;
using Photon.Pun;

public class CameraSwaping : MonoBehaviour
{
    [SerializeField] KeyCode SwapKey;
    private CamaraMovement CamMove;
    [SerializeField] float Cooldown;

    bool canDo;
    private float time;
    private PhotonView photonView;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canDo = true;
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
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
            if (canDo) 
            {
                CamaraMovement[] AllCamera = FindObjectsByType<CamaraMovement>(FindObjectsSortMode.None);

                CamaraMovement othercamera = null;

                foreach (CamaraMovement c in AllCamera)
                {
                    if (c.gameObject != this.gameObject)
                    {
                        othercamera = c;
                        break;
                    }
                }

                if (othercamera == null) return;

                CamMove = this.GetComponentInParent<CamaraMovement>();

                Vector3 thisCameraPos = CamMove.CameraPosition.position;
                Vector3 otherCameraPos = othercamera.CameraPosition.position;
                
                // Broadcast camera swap event to all players via RPC
                if (photonView != null)
                {
                    photonView.RPC("RPC_SwapCameras", RpcTarget.AllBuffered, thisCameraPos, otherCameraPos);
                }
                else
                {
                    ExecuteCameraSwap(thisCameraPos, otherCameraPos);
                }

                canDo = false;
                time = 0f;
            }
        }
    }
    
    [PunRPC]
    void RPC_SwapCameras(Vector3 pos1, Vector3 pos2)
    {
        ExecuteCameraSwap(pos1, pos2);
    }
    
    void ExecuteCameraSwap(Vector3 pos1, Vector3 pos2)
    {
        CamaraMovement[] AllCamera = FindObjectsByType<CamaraMovement>(FindObjectsSortMode.None);
        if (AllCamera.Length >= 2)
        {
            AllCamera[0].CameraPosition.position = pos2;
            AllCamera[1].CameraPosition.position = pos1;
        }
    }
}
