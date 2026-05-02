using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        //Connect to the Photon Master Server using the settings defined in the PhotonServerSettings file
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("成功连接到 Photon Master Server!");
        //Join a random room. If no room is available, it will create one.
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.LogWarningFormat("Connected failed, cause: {0}", cause);
    }
}
