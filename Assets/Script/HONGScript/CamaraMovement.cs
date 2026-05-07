using UnityEngine;

public class CamaraMovement : MonoBehaviour
{
    public Transform CameraPosition;

    private Transform player;

    [SerializeField] float FrontRotation;
    [SerializeField] float LeftRotation;
    [SerializeField] float RightRotation;
    [SerializeField] float BackRotation;

    Vector3 Offset;

    private float originalX;
    private float originalZ;

    public bool Left = false;
    public bool Right = false;
    public bool Front = true;
    public bool Back = false;

    private float HeightOffSet;
    private float ForwardOffSet;

    void Start()
    {
        // 自动寻找同一个 PlayerPrefab 里的 Character_Test
        player = transform.parent.Find("Character_Test");

        if (player == null)
        {
            Debug.LogError("Character_Test not found!");
            return;
        }

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

    void Update()
    {
        if (player == null)
            return;

        if (Front)
        {
            transform.eulerAngles = new Vector3(originalX, FrontRotation, originalZ);

            Vector3 targetPos = new Vector3(
                CameraPosition.position.x,
                player.position.y,
                player.position.z
            );

            transform.position =
                targetPos +
                new Vector3(0, 0, ForwardOffSet) +
                Vector3.up * HeightOffSet;
        }
        else if (Back)
        {
            transform.eulerAngles = new Vector3(originalX, BackRotation, originalZ);

            Vector3 targetPos = new Vector3(
                CameraPosition.position.x,
                player.position.y,
                player.position.z
            );

            transform.position =
                targetPos +
                new Vector3(0, 0, -ForwardOffSet) +
                Vector3.up * HeightOffSet;
        }
        else if (Left)
        {
            transform.eulerAngles = new Vector3(originalX, LeftRotation, originalZ);

            Vector3 targetPos = new Vector3(
                player.position.x,
                player.position.y,
                CameraPosition.position.z
            );

            transform.position =
                targetPos +
                new Vector3(-ForwardOffSet, 0, 0) +
                Vector3.up * HeightOffSet;
        }
        else if (Right)
        {
            transform.eulerAngles = new Vector3(originalX, RightRotation, originalZ);

            Vector3 targetPos = new Vector3(
                player.position.x,
                player.position.y,
                CameraPosition.position.z
            );

            transform.position =
                targetPos +
                new Vector3(ForwardOffSet, 0, 0) +
                Vector3.up * HeightOffSet;
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
        if (player != null)
        {
            CameraPosition.position = player.position;
        }
    }
}