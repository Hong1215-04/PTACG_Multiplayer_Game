using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform Orientation;

    [SerializeField] float Speed = 5;
    [SerializeField] float HoriSpeed = 5;
    [SerializeField] float JumpForce = 100f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] CamaraMovement cameraMovement;
 
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

        Vector3 forwardMove = transform.forward * Speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * HoriSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) return;

        horizontalInput = Input.GetAxis("Horizontal");

        float height = GetComponent<Collider>().bounds.size.y;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.1f, groundMask);

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
        }
      
        if (Input.GetKeyDown(KeyCode.S))
        {
            //play anim (slide)
        }
        if (CAN_Turn == true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(TurnLeft());
                cameraMovement.RotatingLeft();
                CAN_Turn = false;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(TurnRight());
                cameraMovement.RotatingRight();
                CAN_Turn = false;
            }
        }

    }

    IEnumerator TurnRight()
    {
        Quaternion RotateRight = Quaternion.Euler(0, 90, 0);
        transform.rotation = transform.rotation * RotateRight;
        //Orientation.rotation
        yield return new WaitForSeconds(1.5f);
        CAN_Turn = true;
    }

    IEnumerator TurnLeft()
    {
        Quaternion RotateLeft = Quaternion.Euler(0, -90, 0);
        transform.rotation = transform.rotation * RotateLeft;
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
}
