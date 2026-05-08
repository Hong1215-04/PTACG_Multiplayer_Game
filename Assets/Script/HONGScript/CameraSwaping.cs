using UnityEngine;

public class CameraSwaping : MonoBehaviour
{
    [SerializeField] KeyCode SwapKey;
    private CamaraMovement CamMove;
    private CamaraMovement Camera2Move;
    [SerializeField] float Cooldown;
    public GameObject Camera2;

    bool canDo;
    private float time; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canDo = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canDo)
        {
            time += Time.deltaTime;
        }

        if (time > Cooldown)
        {
            canDo = true;
        }

        //if (Input.GetKeyDown(SwapKey))
        //{
        //    if (canDo) 
        //    {
        //        //CamaraMovement[] AllCamera = FindObjectsByType<CamaraMovement>(FindObjectsSortMode.None);

        //        //CamaraMovement othercamera = null;

        //        //foreach (CamaraMovement c in AllCamera)
        //        //{
        //        //    if (c.gameObject != this)
        //        //    {
        //        //        othercamera = c;
        //        //        break;
        //        //    }
        //        //}

        //        //if (othercamera == null) return;

        //        CamMove = this.GetComponentInParent<CamaraMovement>();
        //        Camera2Move = Camera2.GetComponent<CamaraMovement>();

        //        Vector3 thisCameraPos = CamMove.CameraPosition.position;
        //        CamMove.CameraPosition.position = Camera2Move.CameraPosition.position;
        //        Camera2Move.CameraPosition.position = thisCameraPos;

        //        ChangeRotationBool();

        //        canDo = false;
        //        time = 0f;
        //    }
        //}
    }

    public void ChangeRotationBool()
    {
        bool nowFront = CamMove.Front;
        CamMove.Front = Camera2Move.Front;
        Camera2Move.Front = nowFront;

        bool nowLeft = CamMove.Left;
        CamMove.Left = Camera2Move.Left;
        Camera2Move.Left = nowLeft;

        bool nowRight = CamMove.Right;
        CamMove.Right = Camera2Move.Right;
        Camera2Move.Right = nowRight;

        bool nowBack = CamMove.Back;
        CamMove.Back = Camera2Move.Back;
        Camera2Move.Back = nowBack;
    }
}
