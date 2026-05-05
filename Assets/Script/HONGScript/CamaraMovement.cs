using UnityEngine;

public class CamaraMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    public Transform CameraPosition;

    [SerializeField] float FrontRotation;
    [SerializeField] float LeftRotation;
    [SerializeField] float RightRotation;
    [SerializeField] float BackRotation;

    Vector3 Offset;
    //Vector3 OffsetRight;
    //Vector3 OffsetLeft;
    //Vector3 OffsetBack;

    //Vector3 CamerBasedRotation;
    private float originalX;
    private float originalZ;

    bool Left = false;
    bool Right = false;
    bool Front = true;
    bool Back = false;

    private float HeightOffSet;
    private float ForwardOffSet;
    private bool paused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Vector3 CameraBasedRotation = new Vector3 (transform.rotation.x, transform.rotation.y, transform.rotation.z);
        Vector3 CameraBasedRotation = CameraPosition.transform.eulerAngles;
        originalX = CameraBasedRotation.x;
        originalZ = CameraBasedRotation.z;

        Offset = CameraPosition.transform.position - player.position;
        HeightOffSet = Offset.y;
        ForwardOffSet = Offset.z;

        Left = false;
        Right = false;
        Front = true;
        Back = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            return;
        }

        if (Front)
        {
            transform.eulerAngles = new Vector3(originalX, FrontRotation, originalZ);
            //targetPos.x = 5.5f;
            Vector3 targetPos = new Vector3(CameraPosition.position.x, player.position.y, player.position.z);
            transform.position = targetPos + player.forward * -ForwardOffSet + Vector3.up * HeightOffSet;
        }
        else if (Back)
        {
            //Vector3 targetPos = player.position;
            //targetPos.y = player.position.y + OffsetBack.y;
            //targetPos.x = CameraPosition.position.x;
            //targetPos.z = player.position.z + OffsetBack.z;
            transform.eulerAngles = new Vector3(originalX, BackRotation, originalZ);
            //targetPos.x = 5.5f;
            Vector3 targetPos = new Vector3 (CameraPosition.position.x, player.position.y, player.position.z);
            transform.position = targetPos + player.forward * -ForwardOffSet + Vector3.up * HeightOffSet;

        }
        else if (Left)
        {
            transform.eulerAngles = new Vector3(originalX, LeftRotation, originalZ);
            //targetPos.x = 5.5f;
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, CameraPosition.position.z);
            transform.position = targetPos + player.forward * -ForwardOffSet + Vector3.up * HeightOffSet;
        }
        else if (Right)
        {
            transform.eulerAngles = new Vector3(originalX, RightRotation, originalZ);
            //targetPos.x = 5.5f;
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, CameraPosition.position.z);
            transform.position = targetPos + player.forward * -ForwardOffSet + Vector3.up * HeightOffSet;
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
        //Quaternion RotateRight = Quaternion.Euler(0, 90, 0);
        //transform.rotation = transform.rotation * RotateRight;
        
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
        CameraPosition.position = player.position;
    }

    public void PauseCamera()
    {
        paused = true;
    }

    public void ResumeCamera()
    {
        paused = false;
    }

    public static void PauseAllCameras()
    {
        CamaraMovement[] cameras = FindObjectsByType<CamaraMovement>(FindObjectsSortMode.None);
        foreach (CamaraMovement camera in cameras)
        {
            camera.PauseCamera();
        }
    }

    public static void ResumeAllCameras()
    {
        CamaraMovement[] cameras = FindObjectsByType<CamaraMovement>(FindObjectsSortMode.None);
        foreach (CamaraMovement camera in cameras)
        {
            camera.ResumeCamera();
        }
    }
}
