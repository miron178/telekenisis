using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float jumpHeight = 2f;
    [SerializeField]
    private float touchdownForce = -1f;

    public float gravity = -9.81f;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius = 0.5f;
    private LayerMask groundMask;
    private bool isGrounded;

    private Vector3 velocity;

    private void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            //-2 allows the player to conect with the ground instead of being stuck on the ground ckeck or its radius
            velocity.y = touchdownForce;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        //walk
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        
        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * touchdownForce * gravity);
        }

        //calculate gravity
        //gravity requiers time squered thus "* Time.deltaTime" is repeated
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
