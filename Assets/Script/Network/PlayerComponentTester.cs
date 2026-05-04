using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

public class PlayerComponentTester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestPlayerComponent();
        }
    }

    void TestPlayerComponent()
    {
        // 测试1: 获取本地玩家
        Player localPlayer = PhotonNetwork.LocalPlayer;
        Debug.Log($"[测试] 本地玩家: {localPlayer.NickName}");
        
        // 测试2: 获取所有玩家的 PhotonView
        PhotonView[] allPhotonViews = FindObjectsOfType<PhotonView>();
        foreach (var pv in allPhotonViews)
        {
            if (pv.Owner != null)
            {
                Debug.Log($"[测试] 找到玩家对象: {pv.Owner.NickName}, GameObject: {pv.gameObject.name}");
                
                // 测试3: 获取玩家身上的各种 Component
                var transform = pv.GetComponent<Transform>();
                var movement = pv.GetComponent<Movement>();
                var photonTransformView = pv.GetComponent<PhotonTransformView>();
                
                Debug.Log($"  - Transform: {(transform != null ? "✓ 存在" : "✗ 不存在")}");
                Debug.Log($"  - Movement: {(movement != null ? "✓ 存在" : "✗ 不存在")}");
                Debug.Log($"  - PhotonTransformView: {(photonTransformView != null ? "✓ 存在" : "✗ 不存在")}");
            }
        }
        
        // 测试4: 获取房间内的玩家列表
        Player[] playersInRoom = PhotonNetwork.PlayerList;
        Debug.Log($"[测试] 房间内玩家数: {playersInRoom.Length}");
        foreach (var player in playersInRoom)
        {
            Debug.Log($"  - {player.NickName} (ID: {player.ActorNumber})");
        }
    }
}