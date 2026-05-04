using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class TeamPanelManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text p1StatusText;
    [SerializeField] private TMP_Text p2StatusText;
    [SerializeField] private Color onlineColor = Color.green;
    [SerializeField] private Color offlineColor = Color.red;

    private void Start()
    {
        UpdatePlayerStatus();
    }

    private void OnEnable()
    {
        UpdatePlayerStatus();
    }

    public override void OnJoinedRoom()
    {
        UpdatePlayerStatus();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerStatus();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerStatus();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        UpdatePlayerStatus();
    }

    public void UpdatePlayerStatus()
    {
        if (!PhotonNetwork.InRoom)
        {
            UpdateP1Display(false);
            UpdateP2Display(false);
            return;
        }

        Player[] players = PhotonNetwork.PlayerList;
        bool p1Online = false;
        bool p2Online = false;

        foreach (Player player in players)
        {
            string role = GetPlayerRole(player);

            if (role == "P1")
            {
                p1Online = true;
            }
            else if (role == "P2")
            {
                p2Online = true;
            }
        }

        UpdateP1Display(p1Online);
        UpdateP2Display(p2Online);
    }

    private string GetPlayerRole(Player player)
    {
        if (player.CustomProperties != null &&
            player.CustomProperties.TryGetValue("PlayerRole", out object roleValue) &&
            roleValue is string role &&
            !string.IsNullOrEmpty(role))
        {
            return role;
        }

        Player[] players = PhotonNetwork.PlayerList;
        System.Array.Sort(players, (left, right) => left.ActorNumber.CompareTo(right.ActorNumber));

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].ActorNumber == player.ActorNumber)
            {
                return i == 0 ? "P1" : "P2";
            }
        }

        return string.Empty;
    }

    private void UpdateP1Display(bool isOnline)
    {
        if (p1StatusText == null)
        {
            Debug.LogWarning("[TeamPanelManager] P1 Status Text is not assigned.");
            return;
        }

        p1StatusText.text = isOnline ? "P1: Online" : "P1: Offline";
        p1StatusText.color = isOnline ? onlineColor : offlineColor;
    }

    private void UpdateP2Display(bool isOnline)
    {
        if (p2StatusText == null)
        {
            Debug.LogWarning("[TeamPanelManager] P2 Status Text is not assigned.");
            return;
        }

        p2StatusText.text = isOnline ? "P2: Online" : "P2: Offline";
        p2StatusText.color = isOnline ? onlineColor : offlineColor;
    }
}
