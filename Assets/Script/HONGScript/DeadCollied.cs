using UnityEngine;
using UnityEngine.Serialization;
using Photon.Pun;

public class DeadCollied : MonoBehaviour
{
    [SerializeField] Movement movement;
    [FormerlySerializedAs("EachPlayerDeadlayer")]
    [SerializeField] string EachDeadCollide;
    private bool gameEnded;
    private PhotonView photonView;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            EndGame(false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Win"))
        {
            EndGame(true);
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            EndGame(false);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Win"))
        {
            EndGame(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Win"))
        {
            EndGame(true);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer(EachDeadCollide))
        {
            EndGame(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Win"))
        {
            EndGame(true);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer(EachDeadCollide))
        {
            EndGame(false);
        }
    }

    void EndGame(bool completedLevel)
    {
        // Only the owner of this object processes the collision
        if (photonView != null && !photonView.IsMine)
        {
            return;
        }
        
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        if (movement != null)
        {
            movement.Die();
        }

        CamaraMovement.PauseAllCameras();
        // UnityEditor.EditorApplication.isPlaying = false;

            GameOverVoteManager voteManager = GameOverVoteManager.FindInstance();
            if (voteManager == null)
            {
               Debug.LogError("[DeadCollied] GameOverVoteManager not found. Add it to your game scene Canvas and assign the death/win UI references.");
               return;
            }

            if (completedLevel)
            {
               voteManager.ShowLevelCompleteVote();
            }
            else
            {
               voteManager.ShowDeathVote();
            }
    }
}
