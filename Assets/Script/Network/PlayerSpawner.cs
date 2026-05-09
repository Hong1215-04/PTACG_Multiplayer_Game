using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.SceneView;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public Transform p1SpawnPoint;
    public Transform p2SpawnPoint;
    public Transform p1CamSpawnPoint;
    public Transform p2CamSpawnPoint;
    //[SerializeField] GameObject CameraP1;
    //[SerializeField] GameObject CameraP2;
    //[SerializeField] GameObject Player1;
    //[SerializeField] GameObject Player2;
    //[SerializeField] CamaraMovement CamMoveP1;
    //[SerializeField] CamaraMovement CamMoveP2;
    //[SerializeField] Movement MoveP1;
    //[SerializeField] Movement MoveP2;

    public GameObject P1;
    public GameObject P1Pos;
    public GameObject P2Pos;
    //public GameObject P1Cam;
    //public GameObject P2Cam;
    [SerializeField] KeyCode SwapKey;
    [SerializeField] float Cooldown;

    bool canDo;
    private float time;
    public bool swap;

    private CamaraMovement CamMove;
    private CamaraMovement Camera2Move;

    public static PlayerSpawner Instance;

    void Start()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
            return;

        Instance = this;
        canDo = true;

        object role;

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out role))
        {
            string playerRole = role.ToString();

            Transform spawnPoint = null;
            //Transform CamSpawnPoint = null;

            if (playerRole == "P1")
            {
                spawnPoint = p1SpawnPoint;
                //CamSpawnPoint = p1CamSpawnPoint;
                P1 = PhotonNetwork.Instantiate("Player1_Object", spawnPoint.position, spawnPoint.rotation);
                P1.GetComponentInChildren<Camera>().enabled = true;
                //P1Pos = P1.transform.Find("Character_Test").gameObject;
                //StartCoroutine(FindOtherPlayerWhenReady());
                //PhotonNetwork.Instantiate("Main_Camera", CamSpawnPoint.position, CamSpawnPoint.rotation);

            }

            else if (playerRole == "P2")
            {
                spawnPoint = p2SpawnPoint;
                //CamSpawnPoint = p2CamSpawnPoint;
                P1 = PhotonNetwork.Instantiate("Player2Object", spawnPoint.position, spawnPoint.rotation);
                P1.GetComponentInChildren<Camera>().enabled = true;
                //P1Pos = P1.transform.Find("Character_Test").gameObject;
                //StartCoroutine(FindOtherPlayerWhenReady());
                //PhotonNetwork.Instantiate("Main_Camera2", CamSpawnPoint.position, CamSpawnPoint.rotation);
            }

            P1Pos = P1.transform.GetChild(1).gameObject;
            CamMove = P1.GetComponentInChildren<CamaraMovement>();

            //P2Pos = P2.transform.Find("Character_Test").gameObject;

            //Camera2Move = P2.GetComponentInChildren<CamaraMovement>();
            //PlayerSetup CamSetupP1 = P1.GetComponent<PlayerSetup>();
            //PlayerSetup CamSetupP2 = P2.GetComponent<PlayerSetup>();

            //CameraSwaping P1CamSwap = P1Cam.GetComponent<CameraSwaping>();
            //CameraSwaping P2CamSwap = P2Cam.GetComponent<CameraSwaping>();

            //CamaraMovement P1CamMove = P1Cam.GetComponent<CamaraMovement>();
            //CamaraMovement P2CamMove = P2Cam.GetComponent<CamaraMovement>();

            //PlayerSwaping P1Swap = P1.GetComponent<PlayerSwaping>();
            //PlayerSwaping P2Swap = P2.GetComponent<PlayerSwaping>();

            //Movement P1Movement = P1.GetComponent<Movement>();
            //Movement P2Movement = P2.GetComponent<Movement>();

            //CamSetupP1.playerCamera1 = P1Cam;
            //CamSetupP2.playerCamera1 = P2Cam;
            //CamSetupP1.playerCamera2 = P2Cam;
            //CamSetupP2.playerCamera2 = P1Cam;

            //P1Movement.cameraMovement = P1CamMove;
            //P2Movement.cameraMovement = P2CamMove;

            //P1Movement.Camera = P1Cam;
            //P2Movement.Camera = P2Cam;

            //P1Swap.Player2 = P2;
            //P2Swap.Player2 = P1;

            //P1CamSwap.Camera2 = P2Cam;
            //P2CamSwap.Camera2 = P1Cam;

            //if (spawnPoint != null)
            //{
            //    PhotonNetwork.Instantiate(
            //        "Player1_Object",
            //        spawnPoint.position,
            //        spawnPoint.rotation
            //    );
            //}
        }
    }
    private void Update()
    {
        foreach (PhotonView pv in FindObjectsOfType<PhotonView>())
        {
            if (pv.Owner != null && pv.Owner != PhotonNetwork.LocalPlayer)
            {
                P2Pos = pv.gameObject;
                Camera2Move = P2Pos.GetComponentInChildren<CamaraMovement>();
                break;
            }
        }

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
                Swapping();
                ChangeRotationBool();
                canDo = false;
                time = 0f;
            }
        }
    }

    void Swapping()
    {
        if (P2Pos == null)
        {
            Debug.LogWarning("[Swap] P2Pos 还没找到，无法交换");
            return;
        }

        Vector3 myPos = P1Pos.transform.position;
        Vector3 otherPos = P2Pos.transform.position;

        // 改成从子物体找 PhotonView
        PhotonView myPV = P1.GetComponentInChildren<PhotonView>();
        PhotonView otherPV = P2Pos.GetComponentInChildren<PhotonView>();

        if (myPV == null || otherPV == null)
        {
            Debug.LogWarning("[Swap] PhotonView 找不到");
            return;
        }

        myPV.RPC("RPC_TeleportMe", RpcTarget.All, otherPos);
        otherPV.RPC("RPC_TeleportMe", RpcTarget.All, myPos);
    }
    //if (!swap)
    //{
    //    if (this.gameObject.layer == LayerMask.NameToLayer("Player1"))
    //    {
    //        Debug.Log("Swap");
    //        playerCamera.GetComponent<Camera>().enabled = false;
    //        swap = true;
    //    }
    //    else if (this.gameObject.layer == LayerMask.NameToLayer("Player2"))
    //    {
    //        playerCamera.GetComponent<Camera>().enabled = true;
    //        swap = true;
    //    }
    //}
    //else if (swap)
    //{
    //    if (this.gameObject.layer == LayerMask.NameToLayer("Player1"))
    //    {
    //        playerCamera.GetComponent<Camera>().enabled = true;
    //        swap = false;
    //    }
    //    else if (this.gameObject.layer == LayerMask.NameToLayer("Player2"))
    //    {
    //        playerCamera.GetComponent<Camera>().enabled = false;
    //        swap = false;
    //    }
    //}

    // [PunRPC]
    // void TeleportPlayer(Vector3 p1NewPos, Vector3 p2NewPos)
    // {
    //     P1.transform.position = p2NewPos; // your local player goes to P2's old spot
    //     P2Pos.transform.position = p1NewPos; // other player goes to P1's old spot
    // }

    public void ChangeRotationBool()
    {
        bool nowFront = CamMove.Front;
        CamMove.Front = Camera2Move.Front;
        Camera2Move.Front = nowFront;

        bool nowLeft = CamMove.Left;
        CamMove.Left = Camera2Move.Left;
        Camera2Move.Left = nowLeft;

        bool nowRight = CamMove.Right;
        CamMove.Right = Camera2Move.Right;
        Camera2Move.Right = nowRight;

        bool nowBack = CamMove.Back;
        CamMove.Back = Camera2Move.Back;
        Camera2Move.Back = nowBack;
    }

    //IEnumerator FindOtherPlayerWhenReady()
    //{
    //    GameObject otherplayer = null;

    //    while (otherplayer == null)
    //    {
    //        foreach (PhotonView pv in FindObjectsOfType<PhotonView>())
    //        {
    //            if (pv.Owner != PhotonNetwork.LocalPlayer)
    //            {
    //                otherplayer = pv.gameObject;
    //                break;
    //            }
    //        }
    //        yield return new WaitForSeconds(0.2f);

    //        P2 = otherplayer;
    //        P2Pos = P2.transform.GetChild(1).gameObject;
    //        Camera2Move = P2.GetComponentInChildren<CamaraMovement>();
    //    }
    //}



    //void ReferencesforP2()
    //{
    //    PlayerSetup CamSetupP1 = P1.GetComponent<PlayerSetup>();
    //    PlayerSetup CamSetupP2 = P2.GetComponent<PlayerSetup>();

    //    CameraSwaping P1CamSwap = P1Cam.GetComponent<CameraSwaping>();
    //    CameraSwaping P2CamSwap = P2Cam.GetComponent<CameraSwaping>();

    //    CamaraMovement P1CamMove = P1Cam.GetComponent<CamaraMovement>();
    //    CamaraMovement P2CamMove = P2Cam.GetComponent<CamaraMovement>();

    //    PlayerSwaping P1Swap = P1.GetComponent<PlayerSwaping>();
    //    PlayerSwaping P2Swap = P2.GetComponent<PlayerSwaping>();

    //    Movement P1Movement = P1.GetComponent<Movement>();
    //    Movement P2Movement = P2.GetComponent<Movement>();

    //    CamSetupP1.playerCamera1 = P1Cam;
    //    CamSetupP2.playerCamera1 = P2Cam;
    //    CamSetupP1.playerCamera2 = P2Cam;
    //    CamSetupP2.playerCamera2 = P1Cam;

    //    P1Movement.cameraMovement = P1CamMove;
    //    P2Movement.cameraMovement = P2CamMove;

    //    P1Movement.Camera = P1Cam;
    //    P2Movement.Camera = P2Cam;

    //    P1Swap.Player2 = P2;
    //    P2Swap.Player2 = P1;

    //    P1CamSwap.Camera2 = P2Cam;
    //    P2CamSwap.Camera2 = P1Cam;
    //}

    //void ReferencesforP1()
    //{
    //    PlayerSetup CamSetupP1 = P1.GetComponent<PlayerSetup>();
    //    PlayerSetup CamSetupP2 = P2.GetComponent<PlayerSetup>();

    //    CameraSwaping P1CamSwap = P1Cam.GetComponent<CameraSwaping>();
    //    CameraSwaping P2CamSwap = P2Cam.GetComponent<CameraSwaping>();

    //    CamaraMovement P1CamMove = P1Cam.GetComponent<CamaraMovement>();
    //    CamaraMovement P2CamMove = P2Cam.GetComponent<CamaraMovement>();

    //    PlayerSwaping P1Swap = P1.GetComponent<PlayerSwaping>();
    //    PlayerSwaping P2Swap = P2.GetComponent<PlayerSwaping>();

    //    Movement P1Movement = P1.GetComponent<Movement>();
    //    Movement P2Movement = P2.GetComponent<Movement>();

    //    CamSetupP1.playerCamera1 = P1Cam;
    //    CamSetupP2.playerCamera1 = P2Cam;
    //    CamSetupP1.playerCamera2 = P2Cam;
    //    CamSetupP2.playerCamera2 = P1Cam;

    //    P1Movement.cameraMovement = P1CamMove;
    //    P2Movement.cameraMovement = P2CamMove;

    //    P1Movement.Camera = P1Cam;
    //    P2Movement.Camera = P2Cam;

    //    P1Swap.Player2 = P2;
    //    P2Swap.Player2 = P1;

    //    P1CamSwap.Camera2 = P2Cam;
    //    P2CamSwap.Camera2 = P1Cam;
    //}
}