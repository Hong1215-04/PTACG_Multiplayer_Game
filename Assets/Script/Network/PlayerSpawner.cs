using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Vector3 p1StartPosition = new Vector3(-5, 1, 0);
    [SerializeField] private Vector3 p2StartPosition = new Vector3(5, 1, 0);
    [SerializeField] private string playerPrefabName = "Player"; // Prefab name

    void Start()
    {
        // Spawn player when game starts
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            SpawnLocalPlayer();
        }
    }

    void SpawnLocalPlayer()
    {
        // Get local player's role
        Player localPlayer = PhotonNetwork.LocalPlayer;
        string playerRole = "P1"; // Default value

        if (localPlayer.CustomProperties != null && localPlayer.CustomProperties.ContainsKey("PlayerRole"))
        {
            playerRole = (string)localPlayer.CustomProperties["PlayerRole"];
        }

        // Choose spawn position based on player role
        Vector3 spawnPos = playerRole == "P1" ? p1StartPosition : p2StartPosition;

        Debug.Log($"[PlayerSpawner] Spawning player {playerRole} at position: {spawnPos}");

        // Use PhotonNetwork.Instantiate to spawn player over network
        // All players will see the new player appear
        PhotonNetwork.Instantiate(
            playerPrefabName,
            spawnPos,
            Quaternion.identity
        );
    }
}
