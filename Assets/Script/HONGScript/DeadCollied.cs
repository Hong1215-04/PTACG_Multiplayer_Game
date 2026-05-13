using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class DeadCollied : MonoBehaviour, IOnEventCallback
{
    private Movement movement;

    [SerializeField] string EachDeadCollide;

    private string playerRole;

    private bool isDeadOrWin = false;
    private bool isTeleporting = false;

    private const byte DIE_EVENT = 10;
    private const byte WIN_EVENT = 11;

    void Start()
    {
        movement = GetComponentInParent<Movement>();

        object role;

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerRole", out role))
        {
            playerRole = role.ToString();
        }
    }

    void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision.gameObject.layer);
    }

    private void OnCollisionStay(Collision collision)
    {
        CheckCollision(collision.gameObject.layer);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckCollision(other.gameObject.layer);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckCollision(other.gameObject.layer);
    }

    public void SetTeleporting(bool value)
    {
        isTeleporting = value;
    }

    void CheckCollision(int layer)
    {
        if (isDeadOrWin)
            return;
        if (isTeleporting) 
            return;

        // Everyone dies
        if (layer == LayerMask.NameToLayer("Dead"))
        {
            Die();
        }

        // Only P1 dies
        else if (layer == LayerMask.NameToLayer("Dead_P1"))
        {
            if (playerRole == "P1")
            {
                Die();
            }
        }

        // Only P2 dies
        else if (layer == LayerMask.NameToLayer("Dead_P2"))
        {
            if (playerRole == "P2")
            {
                Die();
            }
        }

        // Win
        else if (layer == LayerMask.NameToLayer("Win"))
        {
            Win();
        }

        // Extra trigger layer support
        else if (layer == LayerMask.NameToLayer(EachDeadCollide))
        {
            Die();
        }
    }

    void Die()
    {
        if (isDeadOrWin)
            return;
        isDeadOrWin = true;

        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };

        PhotonNetwork.RaiseEvent(
            DIE_EVENT,
            null,
            options,
            SendOptions.SendReliable
        );
    }

    void Win()
    {
        if (isDeadOrWin)
            return;
        isDeadOrWin = true;

        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };

        PhotonNetwork.RaiseEvent(
            WIN_EVENT,
            null,
            options,
            SendOptions.SendReliable
        );
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == DIE_EVENT)
        {
            AllPlayersDie();
        }
        else if (photonEvent.Code == WIN_EVENT)
        {
            AllPlayersWin();
        }
    }

    void AllPlayersDie()
    {
        if (isDeadOrWin)
            return;

        isDeadOrWin = true;

        Debug.Log(playerRole + " Dead!");

        movement.Die();

        Invoke(nameof(ShowDeathUI), 1f);
    }

    void AllPlayersWin()
    {
        if (isDeadOrWin)
            return;

        isDeadOrWin = true;

        Debug.Log(playerRole + " Win!");

        movement.Die();

        Invoke(nameof(ShowWinUI), 1f);
    }

    void ShowDeathUI()
    {
        //GameOverVoteManager.Instance.ShowDeathVote();
    }

    void ShowWinUI()
    {
        GameOverVoteManager.Instance.ShowLevelCompleteVote();
    }
    
    public void ResetState()
    {
        isDeadOrWin = false;
    }
}