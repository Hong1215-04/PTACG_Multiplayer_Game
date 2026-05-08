using UnityEngine;

public class PlayerSwaping : MonoBehaviour
{
    [SerializeField] KeyCode SwapKey;
    [SerializeField] float Cooldown;
    public GameObject Player2;

    bool canDo;
    private float time;

    //private Movement CameraP2;
    //private Movement CameraP1;
    //private Transform CameraChangePos;
    //private Transform CameraOriPos;

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
                Vector3 thisPlayerPos = transform.position;
                transform.position = Player2.transform.position;
                Player2.transform.position = thisPlayerPos;

                Quaternion thisPlayerRot = transform.rotation;
                transform.rotation = Player2.transform.rotation;
                Player2.transform.rotation = thisPlayerRot;

                canDo = false;
                time = 0f;
            }

            //Movement[] AllPlayer = FindObjectsByType<Movement>(FindObjectsSortMode.None);

            //Movement otherplayer = null;

            //foreach (Movement m in AllPlayer)
            //{
            //    if (m.gameObject != this)
            //    {
            //        otherplayer = m;
            //        break;
            //    }
            //}

            //if (otherplayer == null) return;

            //Vector3 thisPlayerPos = transform.position;
            //transform.position = otherplayer.transform.position;
            //otherplayer.transform.position = thisPlayerPos;

            //canDo = false;
            //time = 0f;
            //Debug.Log(CameraP1.CameraPosition);
            //Debug.Log(CameraP2.CameraPosition);

            //Vector3 CameraPos = CameraP1.Camera.transform.position;
            //CameraP1.Camera.transform.position = CameraP2.Camera.transform.position;
            //CameraP2.Camera.transform.position = CameraPos;
            //CameraP2.CameraPosition.transform.position = CameraP1.CameraPosition.transform.position;  
        }
    }
}
