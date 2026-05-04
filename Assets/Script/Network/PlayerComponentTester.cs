using Photon.Pun;
using Photon.Realtime;
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
        Player localPlayer = PhotonNetwork.LocalPlayer;
        Debug.Log($"[Test] Local player: {localPlayer.NickName}");

        PhotonView[] allPhotonViews = FindObjectsOfType<PhotonView>();
        foreach (var pv in allPhotonViews)
        {
            if (pv.Owner == null)
            {
                continue;
            }

            Debug.Log($"[Test] Found player object: {pv.Owner.NickName}, GameObject: {pv.gameObject.name}");

            var transform = pv.GetComponent<Transform>();
            var movement = pv.GetComponent<Movement>();
            var photonTransformView = pv.GetComponent<PhotonTransformView>();

            Debug.Log($"  - Transform: {(transform != null ? "[OK] Exists" : "[X] Not found")}");
            Debug.Log($"  - Movement: {(movement != null ? "[OK] Exists" : "[X] Not found")}");
            Debug.Log($"  - PhotonTransformView: {(photonTransformView != null ? "[OK] Exists" : "[X] Not found")}");
        }

        Player[] playersInRoom = PhotonNetwork.PlayerList;
        Debug.Log($"[Test] Players in room: {playersInRoom.Length}");
        foreach (var player in playersInRoom)
        {
            Debug.Log($"  - {player.NickName} (ID: {player.ActorNumber})");
        }
    }
}
