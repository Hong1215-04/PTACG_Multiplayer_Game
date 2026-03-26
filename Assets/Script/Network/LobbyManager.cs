using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public MainMenuController mainMenuController;

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

        if (mainMenuController != null)
        {
            mainMenuController.OnRoomJoined();
        }
        else
        {
            Debug.LogError("LobbyManager.mainMenuController is not assigned, cannot notify team panel.");
        }
    }

    // New player joined the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Another player joined!");

        Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Room Full!");
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
}