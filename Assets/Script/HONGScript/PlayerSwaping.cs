using UnityEngine;
using Photon.Pun;

public class PlayerSwaping : MonoBehaviourPun
{
    [SerializeField] KeyCode SwapKey;
    [SerializeField] float Cooldown;
    [SerializeField] GameObject Player2;

    bool canDo = true;
    private float time;

    //private Movement CameraP2;
    //private Movement CameraP1;
    //private Transform CameraChangePos;
    //private Transform CameraOriPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     canDo = true;
    // }

    // Update is called once per frame
    // void Update()
    // {
    //     if (!canDo)
    //     {
    //         time += Time.deltaTime;
    //     }

    //     if (time > Cooldown)
    //     {
    //         canDo = true;
    //     }

    //     if (Input.GetKeyDown(SwapKey))
    //     {
    //         if (canDo)
    //         {
    //             Vector3 thisPlayerPos = transform.position;
    //             transform.position = Player2.transform.position;
    //             Player2.transform.position = thisPlayerPos;

    //             Quaternion thisPlayerRot = transform.rotation;
    //             transform.rotation = Player2.transform.rotation;
    //             Player2.transform.rotation = thisPlayerRot;

    //             canDo = false;
    //             time = 0f;
    //         }

            //Movement[] AllPlayer = FindObjectsByType<Movement>(FindObjectsSortMode.None);

            //Movement otherplayer = null;

            //foreach (Movement m in AllPlayer)
            //{
            //    if (m.gameObject != this)
            //    {
            //        otherplayer = m;
            //        break;
            //    }
            //}

            //if (otherplayer == null) return;

            //Vector3 thisPlayerPos = transform.position;
            //transform.position = otherplayer.transform.position;
            //otherplayer.transform.position = thisPlayerPos;

            //canDo = false;
            //time = 0f;
            //Debug.Log(CameraP1.CameraPosition);
            //Debug.Log(CameraP2.CameraPosition);

            //Vector3 CameraPos = CameraP1.Camera.transform.position;
            //CameraP1.Camera.transform.position = CameraP2.Camera.transform.position;
            //CameraP2.Camera.transform.position = CameraPos;
            //CameraP2.CameraPosition.transform.position = CameraP1.CameraPosition.transform.position;  
    //     }
    // }

        void Update()
    {
        if (!canDo)
        {
            time += Time.deltaTime;
            if (time > Cooldown)
            {
                canDo = true;
                time = 0f;
            }
        }

        // 只有本地玩家才能触发交换
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(SwapKey) && canDo)
        {
            // 找到对方的 PhotonView
            foreach (PhotonView pv in FindObjectsOfType<PhotonView>())
            {
                if (pv != photonView && pv.GetComponent<PlayerSwaping>() != null)
                {
                    // 把双方当前位置和旋转发给所有客户端执行
                    photonView.RPC("RPC_Swap", RpcTarget.All,
                        transform.position,
                        transform.rotation,
                        pv.transform.position,
                        pv.transform.rotation
                    );

                    canDo = false;
                    time = 0f;
                    break;
                }
            }
        }
    }

    [PunRPC]
    void RPC_Swap(Vector3 myPos, Quaternion myRot, Vector3 otherPos, Quaternion otherRot)
    {
        // 本地玩家移动到对方位置
        if (photonView.IsMine)
        {
            transform.position = otherPos;
            transform.rotation = otherRot;
        }
        // 远程玩家移动到本地玩家原来的位置
        else
        {
            transform.position = myPos;
            transform.rotation = myRot;
        }
    }
}
