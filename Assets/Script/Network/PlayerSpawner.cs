using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

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

    //public GameObject P1;
    //public GameObject P2;
    //public GameObject P1Cam;
    //public GameObject P2Cam;

    public static PlayerSpawner Instance;

    void Start()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
            return;

        Instance = this;

        object role;

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out role))
        {
            string playerRole = role.ToString();

            Transform spawnPoint = null;
            Transform CamSpawnPoint = null;

            if (playerRole == "P1")
            {
                spawnPoint = p1SpawnPoint;
                CamSpawnPoint = p1CamSpawnPoint;
                PhotonNetwork.Instantiate("Player1_Object", spawnPoint.position, spawnPoint.rotation);
                PhotonNetwork.Instantiate("Main_Camera", CamSpawnPoint.position, CamSpawnPoint.rotation);

            }

            else if (playerRole == "P2")
            {
                spawnPoint = p2SpawnPoint;
                CamSpawnPoint = p2CamSpawnPoint;
                PhotonNetwork.Instantiate("Player2Object", spawnPoint.position, spawnPoint.rotation);
                PhotonNetwork.Instantiate("Main_Camera2", CamSpawnPoint.position, CamSpawnPoint.rotation);
            }

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

    //IEnumerator FindOtherPlayerWhenReadyP1()
    //{
    //    GameObject otherplayer = null;
    //    GameObject othercamera = null;

    //    while (otherplayer == null || othercamera == null)
    //    {
    //        foreach (PhotonView pv in FindObjectsOfType<PhotonView>())
    //        {
    //            if (pv.Owner != PhotonNetwork.LocalPlayer)
    //            {
    //                if (pv.CompareTag("Player"))
    //                {
    //                    otherplayer = pv.gameObject;
    //                    break;
    //                }
    //                if (pv.CompareTag("MainCamera"))
    //                {
    //                    othercamera = pv.gameObject;
    //                    break;
    //                }
    //            }
    //        }
    //        yield return new WaitForSeconds(0.2f);

    //        P2 = otherplayer;
    //        P2Cam = othercamera;

    //        ReferencesforP1();
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