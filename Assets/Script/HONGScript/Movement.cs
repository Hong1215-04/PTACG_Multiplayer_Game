using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Movement : MonoBehaviour, IPunObservable
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
    public GameObject Camera;

    float horizontalInput;
    public Rigidbody rb;
    private PhotonView photonView;
    //float yRotation;
    //float RotationSpeed = 200f;

    bool CAN_Turn;
    bool alive;

    void Start()
    {
        alive = true;
        CAN_Turn = true;
        photonView = GetComponent<PhotonView>();
        
        // Register this script as observable for network synchronization
        if (photonView != null && !photonView.ObservedComponents.Contains(this))
        {
            photonView.ObservedComponents.Add(this);
        }
    }

    private void FixedUpdate()
    {
        if (!alive) return;
        
        // Only the owner can control this player
        if (photonView != null && !photonView.IsMine) return;
        // forwardMove & horizontalmove is just variable (name) not function

        Vector3 forwardMove = transform.forward * Speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * HoriSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + forwardMove + horizontalMove);
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) return;
        
        // Only the owner receives input
        if (photonView != null && !photonView.IsMine) return;

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

    // Network synchronization - sync player state across network
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send local player's input state to others
            stream.SendNext(horizontalInput);
            stream.SendNext(alive);
            stream.SendNext(CAN_Turn);
        }
        else
        {
            // Receive remote player's input state
            float remoteHorizontalInput = (float)stream.ReceiveNext();
            bool remoteAlive = (bool)stream.ReceiveNext();
            bool remoteCAN_Turn = (bool)stream.ReceiveNext();
            
            // Apply remote player's state
            if (!photonView.IsMine)
            {
                horizontalInput = remoteHorizontalInput;
                alive = remoteAlive;
                CAN_Turn = remoteCAN_Turn;
            }
        }
    }
}
