using UnityEngine;

public class CamaraMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 Offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Offset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.position + Offset;
        targetPos.x = 5.5f;
        transform.position = targetPos;
    }
}
