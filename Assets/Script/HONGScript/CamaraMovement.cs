using UnityEngine;

public class CamaraMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform CameraPosition;  
    Vector3 Offset;

    bool Left = false;
    bool Right = false;
    bool Front = true;
    bool Back = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Offset = transform.position - player.position;
        Left = false;
        Right = false;
        Front = true;
        Back = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.position + Offset;
        targetPos.x = CameraPosition.position.x;
        //targetPos.x = 5.5f;
        transform.position = targetPos;
    }

    public void RotatingLeft()
    {
        if (Front)
        {
            Left = true;
            Front = false;
        }
        else if (Left)
        {
            Back = true; 
            Left = false;
        }
        else if (Back)
        {
            Right = true; 
            Back = false;
        }
        else if (Right)
        {
            Front = true;
            Right = false;
        }
    }

    public void RotatingRight()
    {

    }
}
