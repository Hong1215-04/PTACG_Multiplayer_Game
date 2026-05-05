using UnityEngine;
using Photon.Pun;

public class PlayerSwaping : MonoBehaviour
{
    [SerializeField] KeyCode SwapKey;
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
                Movement[] AllPlayer = FindObjectsByType<Movement>(FindObjectsSortMode.None);

                Movement otherplayer = null;

                foreach (Movement m in AllPlayer)
                {
                    if (m.gameObject != this.gameObject)
                    {
                        otherplayer = m;
                        break;
                    }
                }

                if (otherplayer == null) return;

                Vector3 thisPlayerPos = transform.position;
                Vector3 otherPlayerPos = otherplayer.transform.position;
                
                // Broadcast swap event to all players via RPC
                if (photonView != null)
                {
                    photonView.RPC("RPC_SwapPositions", RpcTarget.AllBuffered, thisPlayerPos, otherPlayerPos);
                }
                else
                {
                    ExecuteSwap(thisPlayerPos, otherPlayerPos);
                }

                canDo = false;
                time = 0f;
            }
        }
    }
    
    [PunRPC]
    void RPC_SwapPositions(Vector3 pos1, Vector3 pos2)
    {
        ExecuteSwap(pos1, pos2);
    }
    
    void ExecuteSwap(Vector3 pos1, Vector3 pos2)
    {
        Movement[] AllPlayer = FindObjectsByType<Movement>(FindObjectsSortMode.None);
        if (AllPlayer.Length >= 2)
        {
            AllPlayer[0].transform.position = pos2;
            AllPlayer[1].transform.position = pos1;
        }
    }
}
