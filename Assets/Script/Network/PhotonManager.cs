using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
    
    public List<RoomInfo> availableRooms = new List<RoomInfo>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PhotonNetwork.GameVersion = "1.0";
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Photon...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server!");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby!");
        availableRooms.Clear();
    }

    // 显示房间列表
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log($"[房间列表更新] 共有 {roomList.Count} 个房间");
        
        availableRooms.Clear();
        
        foreach (RoomInfo room in roomList)
        {
            // 只显示有效的房间（未移除且未满）
            if (!room.RemovedFromList && room.IsOpen && room.IsVisible)
            {
                availableRooms.Add(room);
                Debug.Log($"  房间: {room.Name} - 玩家: {room.PlayerCount}/{room.MaxPlayers}");
            }
        }
    }
}