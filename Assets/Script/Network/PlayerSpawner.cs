using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform p1SpawnPoint;
    public Transform p2SpawnPoint;
    [SerializeField] GameObject CameraP1;
    [SerializeField] GameObject CameraP2;
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;
    [SerializeField] CamaraMovement CamMoveP1;
    [SerializeField] CamaraMovement CamMoveP2;
    [SerializeField] Movement MoveP1;
    [SerializeField] Movement MoveP2;

    public static PlayerSpawner Instance;

    void Awake()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
            return;

        Instance = this;

        object role;

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out role))
        {
            string playerRole = role.ToString();

            Transform spawnPoint = null;

            if (playerRole == "P1")
            {
                spawnPoint = p1SpawnPoint;
                GameObject P1 = PhotonNetwork.Instantiate("Player1_Object", spawnPoint.position, spawnPoint.rotation);
                CamaraMovement[] allCamera = FindObjectsByType<CamaraMovement>(FindObjectsSortMode.None);

                CamaraMovement otherCamera = null;
                CamaraMovement thisCamera = null;

                foreach (CamaraMovement p in allCamera)
                {
                    if (p.gameObject != this)
                    {
                        otherCamera = p;
                        break;
                    }
                }
                foreach (CamaraMovement m in allCamera)
                {
                    if (m.gameObject == this)
                    {
                        thisCamera = m;
                        break;
                    }
                }
            }

            else if (playerRole == "P2")
            {
                spawnPoint = p2SpawnPoint;
                GameObject P2 = PhotonNetwork.Instantiate("Player1_Object", spawnPoint.position, spawnPoint.rotation);
            }

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
}