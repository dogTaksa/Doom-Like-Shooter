using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

//script should be assigned for the player
[RequireComponent(typeof(Rigidbody))]

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 jump;
    public float jumpForce = 2.0f; // player jump height
    public float currentSpeed; //player speed at the moment
    public float sprint = 5; //speed which is added to regular player speed while sprinting
    public float baseSpeed = 5; //player regular speed
    public float crouch = 0.5f; //camera height while crouch
    public Camera MainCamera; //child of player - Main camera with look script
    public float cameraY = 1f; //starting camera position
    public bool isGrounded; //checks if player standing on the ground to let it jump
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void Update()
    {
        var x = transform.position.x;
        var z = transform.position.z;
        var input = new Vector3();

        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");
        transform.position += input * baseSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = baseSpeed + sprint;
        }
        else
        {
            currentSpeed = baseSpeed;
        }
        if (input != Vector3.zero)
        {
            transform.forward = input;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            var camJump = Camera.main.transform.position.y * jumpForce;
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }  
}