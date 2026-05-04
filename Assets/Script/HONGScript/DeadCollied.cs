using UnityEngine;

public class DeadCollied : MonoBehaviour
{
    [SerializeField] Movement movement;
    public string EachPlayerDeadlayer;
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
            movement.Die();
            Invoke("LoseGame", 2);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Win"))
        {
            movement.Die();
            Invoke("WinGame", 2);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            movement.Die();
            Invoke("LoseGame", 2);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Win"))
        {
            movement.Die();
            Invoke("WinGame", 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(EachPlayerDeadlayer))
        {
            movement.Die();
            Invoke("LoseGame", 2);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(EachPlayerDeadlayer))
        {
            movement.Die();
            Invoke("LoseGame", 2);
        }
    }

    void LoseGame() 
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    void WinGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
