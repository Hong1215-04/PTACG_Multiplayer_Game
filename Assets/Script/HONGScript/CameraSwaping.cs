using UnityEngine;

public class CameraSwaping : MonoBehaviour
{
    [SerializeField] KeyCode SwapKey;
    private CamaraMovement CamMove;
    [SerializeField] float Cooldown;

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

        if (Input.GetKeyDown(SwapKey))
        {
            if (canDo) 
            {
                CamaraMovement[] AllCamera = FindObjectsByType<CamaraMovement>(FindObjectsSortMode.None);

                CamaraMovement othercamera = null;

                foreach (CamaraMovement c in AllCamera)
                {
                    if (c.gameObject != this)
                    {
                        othercamera = c;
                        break;
                    }
                }

                if (othercamera == null) return;

                CamMove = this.GetComponentInParent<CamaraMovement>();

                Vector3 thisCameraPos = CamMove.CameraPosition.position;
                CamMove.CameraPosition.position = othercamera.CameraPosition.position;
                othercamera.CameraPosition.position = thisCameraPos;

                canDo = false;
                time = 0f;
            }
        }
    }
}
