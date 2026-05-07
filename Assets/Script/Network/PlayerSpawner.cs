using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform p1SpawnPoint;
    public Transform p2SpawnPoint;

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

            if (playerRole == "P1")
            {
                spawnPoint = p1SpawnPoint;
            }
            else if (playerRole == "P2")
            {
                spawnPoint = p2SpawnPoint;
            }

            if (spawnPoint != null)
            {
                PhotonNetwork.Instantiate(
                    "Player1_Object",
                    spawnPoint.position,
                    spawnPoint.rotation
                );
            }
        }
    }
}