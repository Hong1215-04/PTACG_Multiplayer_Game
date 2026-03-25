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
 
    float horizontalInput;
    public Rigidbody rb;
    float yRotation;
    float RotationSpeed = 200f;
    float savedRotation;


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
        savedRotation = transform.rotation.y;

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
                CAN_Turn = false;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(TurnRight());
                CAN_Turn = false;
            }
        }

    }

    IEnumerator TurnRight()
    {
        Quaternion RotateRight = Quaternion.Euler(0, (savedRotation - 90), 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, RotateRight, Time.deltaTime * RotationSpeed);
        //Orientation.rotation
        yield return new WaitForSeconds(1.5f);
        CAN_Turn = true;
    }

    IEnumerator TurnLeft()
    {
        Quaternion RotateRight = Quaternion.Euler(0, (savedRotation + 90), 0);
        transform.rotation = RotateRight;

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
