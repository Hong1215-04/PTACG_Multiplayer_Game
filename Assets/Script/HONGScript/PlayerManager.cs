using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// Player role: "P1" or "P2"
    /// </summary>
    public string PlayerRole { get; private set; }
    
    /// <summary>
    /// Whether this is the local player
    /// </summary>
    public bool IsLocalPlayer { get; private set; }
    
    /// <summary>
    /// Player number: 1 (P1) or 2 (P2)
    /// </summary>
    public int PlayerNumber { get; private set; }
    
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        
        if (photonView != null && photonView.Owner != null)
        {
            Player owner = photonView.Owner;
            IsLocalPlayer = photonView.IsMine;
            
            // Get player role from custom properties
            if (owner.CustomProperties != null && owner.CustomProperties.ContainsKey("PlayerRole"))
            {
                PlayerRole = (string)owner.CustomProperties["PlayerRole"];
            }
            else
            {
                // Fallback: use ActorNumber if role is not set
                // ActorNumber 1 is P1, 2 is P2
                PlayerRole = owner.ActorNumber == 1 ? "P1" : "P2";
            }

            // Extract number from role name
            PlayerNumber = PlayerRole == "P1" ? 1 : 2;
            
            gameObject.name = $"Player_{PlayerRole}" + (IsLocalPlayer ? " (Local)" : " (Remote)");
            
            Debug.Log($"[PlayerManager] Player {gameObject.name} initialized - Role: {PlayerRole}, IsLocal: {IsLocalPlayer}");
        }
        else
        {
            Debug.LogError("[PlayerManager] PhotonView not found or owner not assigned!");
        }
    }

    /// <summary>
    /// Get player role name (P1 or P2)
    /// </summary>
    public string GetPlayerRole()
    {
        return PlayerRole;
    }

    /// <summary>
    /// Check if this player is the local player
    /// </summary>
    public bool IsMe()
    {
        return IsLocalPlayer;
    }

    /// <summary>
    /// Check if player is P1
    /// </summary>
    public bool IsP1()
    {
        return PlayerRole == "P1";
    }

    /// <summary>
    /// Check if player is P2
    /// </summary>
    public bool IsP2()
    {
        return PlayerRole == "P2";
    }
}
