using UnityEngine;
using Photon.Pun;

public class PlayerNameInputManager : MonoBehaviour
{
    public void SetPlayerName(string playername)
    {
        if (string.IsNullOrEmpty(playername))
        {
            Debug.Log("Player name is empty");
            return;
        }

        PhotonNetwork.NickName = playername;
        Debug.Log("Player name set to: " + PhotonNetwork.NickName);
    }
}
