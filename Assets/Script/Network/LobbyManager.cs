using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public MainMenuController mainMenuController;
    private TeamPanelManager teamPanelManager;

    private bool isJoinWithPassword = false;
    
    public void OnClickCreateRoom()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Not ready yet!");
            return;
        }

        PhotonNetwork.CreateRoom(null, new RoomOptions
        {
            MaxPlayers = 2
        });
    }

    public void CreateRoomWithPassword(string password)
    {
        isJoinWithPassword = false;

        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Not ready yet! Connect to Photon first.");
            return;
        }

        var props = new Hashtable { { "pw", password } };
        var options = new RoomOptions
        {
            MaxPlayers = 2,
            CustomRoomProperties = props,
            CustomRoomPropertiesForLobby = new string[] { "pw" }
        };

        PhotonNetwork.CreateRoom(null, options);
        Debug.Log("Creating room with password: " + password);
    }

    public void JoinRoomWithPassword(string password)
    {
        isJoinWithPassword = true;

        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Not ready yet! Connect to Photon first.");
            return;
        }

        var expectedProps = new Hashtable { { "pw", password } };
        PhotonNetwork.JoinRandomRoom(expectedProps, 0);
        Debug.Log("Joining room with password: " + password);
    }

    // Join Room
    public void OnClickJoinRoom()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogWarning("Not ready yet!");
            return;
        }

        PhotonNetwork.JoinRandomRoom();
    }

    // Join Failed → Create Room
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Join random room failed: " + message);

        if (isJoinWithPassword)
        {
            if (mainMenuController != null)
            {
                mainMenuController.ShowPasswordPanelWithError("Room with this password not found. Please try again.");
            }
        }
        else
        {
            // fallback: create room if no random join succeeded
            PhotonNetwork.CreateRoom(null, new RoomOptions
            {
                MaxPlayers = 2
            });
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Create room failed: " + message);

        // For create, just show password panel without error message
        if (mainMenuController != null)
        {
            mainMenuController.ShowPasswordPanel();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room! from LobbyManager");
        Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        // Set player role: first player is P1 (Master Client), second player is P2 (Joiner)
        SetPlayerRole();

        // Update UI display player status
        if (teamPanelManager != null)
        {
            Debug.Log("[LobbyManager] teamPanelManager found, calling UpdatePlayerStatus");
            teamPanelManager.UpdatePlayerStatus();
        }
        else
        {
            Debug.LogError("[LobbyManager] teamPanelManager is NULL!");
        }

        if (mainMenuController != null)
        {
            mainMenuController.OnRoomJoined();
            mainMenuController.RefreshRoomUi();
        }
        else
        {
            Debug.LogError("LobbyManager.mainMenuController is not assigned, cannot notify team panel.");
        }
    }

    /// <summary>
    /// Set player role based on room player count. First player is P1, second player is P2.
    /// </summary>
    void SetPlayerRole()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            // Only local player (master client), mark as P1
            Hashtable playerProps = new Hashtable { { "PlayerRole", "P1" } };
            localPlayer.SetCustomProperties(playerProps);
            Debug.Log("[LobbyManager] Local player set as P1 (Master Client)");
            Debug.Log($"[LobbyManager] Local player custom properties: {string.Join(", ", playerProps.Keys)}");
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            // Two players, local player is P2 (Joiner)
            Hashtable playerProps = new Hashtable { { "PlayerRole", "P2" } };
            localPlayer.SetCustomProperties(playerProps);
            Debug.Log("[LobbyManager] Local player set as P2 (Joiner)");
            Debug.Log($"[LobbyManager] Local player custom properties: {string.Join(", ", playerProps.Keys)}");
        }
    }

    // New player joined the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Another player joined!");
        Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        // Update UI display player status
        if (teamPanelManager != null)
        {
            teamPanelManager.UpdatePlayerStatus();
        }

        if (mainMenuController != null)
        {
            mainMenuController.RefreshRoomUi();
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Room Full! Both players ready.");
            if (mainMenuController != null)
            {
                mainMenuController.ShowStartButton();
            }
        }
    }

    // Player left the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.NickName} left the room!");
        Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        // Update UI display player status (show if a player is offline)
        if (teamPanelManager != null)
        {
            teamPanelManager.UpdatePlayerStatus();
        }

        if (mainMenuController != null)
        {
            mainMenuController.RefreshRoomUi();
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left room.");
        if (mainMenuController != null)
        {
            mainMenuController.ShowPasswordPanel();
        }
    }

    // Start game - called by master client
    public void StartGame()
    {
        Debug.Log("StartGame() called");
        Debug.Log("Is Master Client: " + PhotonNetwork.IsMasterClient);
        Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("Only master client can start the game!");
            return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount < 1)
        {
            Debug.LogWarning("Not enough players to start the game!");
            return;
        }

        // 直接调用，不用 RPC（因为只有房主能调用）
        string sceneName = mainMenuController != null ? mainMenuController.gameSceneName : "Testing";
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Game scene name is empty.");
            return;
        }

        Debug.Log("Starting game...");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel(sceneName);
    }

    [PunRPC]
    void RPC_StartGame()
    {
        Debug.Log("RPC_StartGame called!");
        
        if (mainMenuController != null)
        {
            mainMenuController.EnterGame();
        }
        else
        {
            Debug.LogError("mainMenuController not assigned!");
        }
    }

    // Get PhotonView component (for RPC)
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            photonView = gameObject.AddComponent<PhotonView>();
        }

        // Get TeamPanelManager reference
        teamPanelManager = FindObjectOfType<TeamPanelManager>();
        if (teamPanelManager == null)
        {
            Debug.LogWarning("[LobbyManager] TeamPanelManager not found, player status will not update");
        }
        else
        {
            Debug.Log("[LobbyManager] TeamPanelManager found and assigned");
        }
    }
}
