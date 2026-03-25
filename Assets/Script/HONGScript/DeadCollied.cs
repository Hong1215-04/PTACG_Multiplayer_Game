using UnityEngine;

public class DeadCollied : MonoBehaviour
{
    [SerializeField] Movement movement;
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
            Invoke("StopGame", 2);
        }
    }
    void StopGame() 
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
