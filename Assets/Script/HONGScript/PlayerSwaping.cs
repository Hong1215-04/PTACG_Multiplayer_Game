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

        if (Input.GetKeyDown(SwapKey) && canDo)
            {
                foreach (PhotonView pv in FindObjectsOfType<PhotonView>())
                {
                    // 找到对方玩家的 PhotonView
                    if (pv != photonView && pv.GetComponent<PlayerSwaping>() != null)
                    {
                        Vector3 myPos = transform.position;
                        Quaternion myRot = transform.rotation;
                        Vector3 otherPos = pv.transform.position;
                        Quaternion otherRot = pv.transform.rotation;

                        // 通知自己的脚本移动到对方位置
                        photonView.RPC("RPC_MoveToPos", RpcTarget.All, otherPos, otherRot);

                        // 通知对方的脚本移动到我的位置
                        pv.RPC("RPC_MoveToPos", RpcTarget.All, myPos, myRot);

                        canDo = false;
                        time = 0f;
                        break;
                    }
                }
            }
    }

    [PunRPC]
    void RPC_Swap(Vector3 p1Pos, Quaternion p1Rot, Vector3 p2Pos, Quaternion p2Rot)
    {
        // 发起交换的人（P1）移到 P2 的位置
        // 收到交换的人（P2）移到 P1 的位置
        if (photonView.IsMine)
        {
            transform.position = p2Pos;
            transform.rotation = p2Rot;
        }
        else
        {
            transform.position = p1Pos;
            transform.rotation = p1Rot;
        }
    }
}
