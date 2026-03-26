using UnityEngine;

public class CamaraMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform CameraPosition; 
    Vector3 Offset;
    Vector3 CamerBasedRotation;

    bool Left = false;
    bool Right = false;
    bool Front = true;
    bool Back = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 CameraBasedRotation = new Vector3 (transform.rotation.x, transform.rotation.y, transform.rotation.z);
        Offset = transform.position - player.position;
        Left = false;
        Right = false;
        Front = true;
        Back = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Front)
        {
            Vector3 targetPos = player.position + Offset;
            targetPos.x = CameraPosition.position.x;
            //targetPos.x = 5.5f;
            transform.position = targetPos;
        }
        else if (Back)
        {
            Vector3 targetPos = new Vector3(player.position.x,(player.position.y + Offset.y),(player.position.z - Offset.z));
            targetPos.x = CameraPosition.position.x;
            //targetPos.x = 5.5f;
            transform.position = targetPos;
        }
        if (Left || Right)
        {
            Vector3 targetPos = player.position + Offset;
            targetPos.y = CameraPosition.position.y;
            //targetPos.x = 5.5f;
            transform.position = targetPos;
        }
    }

    public void RotatingLeft()
    {
        if (Front)
        {
            Left = true;
            Front = false;
            RecordCamPosition();
        }
        else if (Left)
        {
            Back = true; 
            Left = false;
            RecordCamPosition();
        }
        else if (Back)
        {
            Right = true; 
            Back = false;
            RecordCamPosition();
        }
        else if (Right)
        {
            Front = true;
            Right = false;
            RecordCamPosition();
        }
    }

    public void RotatingRight()
    {
        Quaternion RotateRight = Quaternion.Euler(0, 90, 0);
        transform.rotation = transform.rotation * RotateRight;
        Quaternion StillX = Quaternion.Euler(CamerBasedRotation.x, 0, 0);

        if (Front)
        {
            Right = true;
            Front = false;
            RecordCamPosition();
        }
        else if (Right)
        {
            Back = true;
            Right = false;
            RecordCamPosition();
        }
        else if (Back)
        {
            Left = true;
            Back = false;
            RecordCamPosition();
        }
        else if (Left)
        {
            Front = true;
            Left = false;
            RecordCamPosition();
        }
    }

    void RecordCamPosition()
    {
        CameraPosition.position = transform.position;
    }
}
