using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class TeamPanelManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text p1StatusText;  // Status text for P1
    [SerializeField] private TMP_Text p2StatusText;  // Status text for P2
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

    /// <summary>
    /// Update online status display for P1 and P2
    /// </summary>
    public void UpdatePlayerStatus()
    {
        if (!PhotonNetwork.InRoom)
        {
            UpdateP1Display(false);
            UpdateP2Display(false);
            return;
        }

        Player[] players = PhotonNetwork.PlayerList;
        Debug.Log($"[TeamPanelManager] Checking {players.Length} players in room");
        
        // Initialize: both offline
        bool p1Online = false;
        bool p2Online = false;

        foreach (Player player in players)
        {
            string role = GetPlayerRole(player);
            Debug.Log($"[TeamPanelManager] Player: {player.NickName}, ActorNumber: {player.ActorNumber}, Role: {role}");

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

        Debug.Log($"[TeamPanelManager] Update status - P1: {(p1Online ? "Online" : "Offline")}, P2: {(p2Online ? "Online" : "Offline")}");
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

    void UpdateP1Display(bool isOnline)
    {
        if (p1StatusText != null)
        {
            if (isOnline)
            {
                p1StatusText.text = "P1: Online [OK]";
                p1StatusText.color = onlineColor;
            }
            else
            {
                p1StatusText.text = "P1: Offline [X]";
                p1StatusText.color = offlineColor;
            }
        }
    }

    void UpdateP2Display(bool isOnline)
    {
        if (p2StatusText != null)
        {
            if (isOnline)
            {
                p2StatusText.text = "P2: Online [OK]";
                p2StatusText.color = onlineColor;
            }
            else
            {
                p2StatusText.text = "P2: Offline [X]";
                p2StatusText.color = offlineColor;
            }
        }
    }
}
