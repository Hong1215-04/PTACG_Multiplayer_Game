using UnityEngine;

public class DeadCollied : MonoBehaviour
{
    [SerializeField] Movement movement;
    public string EachPlayerDeadlayer;
    private bool gameEnded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            EndGame();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Win"))
        {
            EndGame();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            EndGame();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Win"))
        {
            EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(EachPlayerDeadlayer))
        {
            EndGame();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(EachPlayerDeadlayer))
        {
            EndGame();
        }
    }

    void EndGame()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        movement.Die();

        GameOverVoteManager voteManager = GameOverVoteManager.FindInstance();
        if (voteManager == null)
        {
            Debug.LogError("[DeadCollied] GameOverVoteManager not found. Add it to your game scene Canvas and assign the death UI references.");
            return;
        }

        voteManager.ShowDeathVote();
    }
}
