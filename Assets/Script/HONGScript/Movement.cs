using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviourPun
{
    public Transform Orientation;

    [SerializeField] float Speed = 5;
    [SerializeField] float HoriSpeed = 5;
    [SerializeField] float JumpForce = 100f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] KeyCode KeyJump;
    [SerializeField] KeyCode RotateLeft;
    [SerializeField] KeyCode RotateRight;

    public string MoveHori;

    public CamaraMovement cameraMovement;
    //public GameObject Camera;

    float horizontalInput;
    public Rigidbody rb;
    //float yRotation;
    //float RotationSpeed = 200f;
    bool CAN_Turn;
    bool alive;

    void Start()
    {
        alive = true;
        CAN_Turn = true;
    }

    private void FixedUpdate()
    {
        if (!alive) return;
        // forwardMove & horizontalmove is just variable (name) not function
        if (photonView.IsMine)
        {
            Vector3 forwardMove = transform.forward * Speed * Time.fixedDeltaTime;
            Vector3 horizontalMove = transform.right * horizontalInput * HoriSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + forwardMove + horizontalMove);
        }
            else
        {
            // 加这行 ↓
            Debug.Log($"[Remote] 对方位置: {transform.position}，PhotonView Owner: {photonView.Owner?.NickName}");
        }
        //Vector3 forwardMove = transform.forward * Speed * Time.fixedDeltaTime;
        //Vector3 horizontalMove = transform.right * horizontalInput * HoriSpeed * Time.fixedDeltaTime;
        //rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) return;

        if (photonView.IsMine)
        {
            horizontalInput = Input.GetAxis(MoveHori);

            float height = GetComponent<Collider>().bounds.size.y;
            bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.6f, groundMask);


            if (isGrounded)
            {
                Debug.Log("Ground");
                if (Input.GetKeyDown(KeyJump))
                {
                    Debug.Log("JUMPress");
                    Jump();
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                //play anim (slide)
            }
            if (CAN_Turn == true)
            {
                if (Input.GetKeyDown(RotateLeft))
                {
                    StartCoroutine(TurnLeft());
                    cameraMovement.RotatingLeft();
                    CAN_Turn = false;
                }
                else if (Input.GetKeyDown(RotateRight))
                {
                    StartCoroutine(TurnRight());
                    cameraMovement.RotatingRight();
                    CAN_Turn = false;
                }
            }
        }

        //horizontalInput = Input.GetAxis(MoveHori);

        //float height = GetComponent<Collider>().bounds.size.y;
        //bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.6f, groundMask);
 

        //if (isGrounded)
        //{
        //    Debug.Log("Ground");
        //    if (Input.GetKeyDown(KeyJump))
        //    {
        //        Debug.Log("JUMPress");
        //        Jump();
        //    }
        //}
      
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    //play anim (slide)
        //}
        //if (CAN_Turn == true)
        //{
        //    if (Input.GetKeyDown(RotateLeft))
        //    {
        //        StartCoroutine(TurnLeft());
        //        cameraMovement.RotatingLeft();
        //        CAN_Turn = false;
        //    }
        //    else if (Input.GetKeyDown(RotateRight))
        //    {
        //        StartCoroutine(TurnRight());
        //        cameraMovement.RotatingRight();
        //        CAN_Turn = false;
        //    }
        //}
    }

    IEnumerator TurnRight()
    {
        Quaternion rotateRight = Quaternion.Euler(0, 90, 0);
        transform.rotation = transform.rotation * rotateRight;
        //Orientation.rotation
        yield return new WaitForSeconds(1.5f);
        CAN_Turn = true;
    }

    IEnumerator TurnLeft()
    {
        Quaternion rotateLeft = Quaternion.Euler(0, -90, 0);
        transform.rotation = transform.rotation * rotateLeft;
        //Orientation.rotation
        yield return new WaitForSeconds(1.5f);
        CAN_Turn = true;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * JumpForce);
    }

    public void Die()
    {
        alive = false;
    }

    //public CamaraMovement ReturnCamMove()
    //{
    //    return cameraMovement;
    //}
}
