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

    public float gravity = -9.81f;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius = 0.5f;
    private LayerMask groundMask;

    private bool m_isGrounded;
    public bool isGrounded { get => m_isGrounded; }

    private Vector3 velocity;

    private Pickup_Spring m_pickupSpring;

    [SerializeField]
    private float m_sliderDetectionDistance = 0.5f;
    Slider m_slider = null;


    private void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
        m_pickupSpring = GetComponentInChildren<Pickup_Spring>();

        CharacterController controller = GetComponent<CharacterController>();
        // calculate the correct vertical position:
        float correctHeight = controller.center.y + controller.skinWidth;
        // set the controller center vector:
        controller.center = new Vector3(0, correctHeight, 0);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (m_pickupSpring.heldObject == hit.gameObject)
        {
            m_pickupSpring.Release();
        }
    }

    void Update()
    {
        m_isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        Debug.DrawRay(groundCheck.position, new Vector3(0, -m_sliderDetectionDistance, 0), Color.black);
        if (m_isGrounded)
        {
            Ray ray = new Ray(groundCheck.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, m_sliderDetectionDistance))
            {
                Slider slider = hitInfo.collider.GetComponent<Slider>();
                if (slider && slider != m_slider)
                {
                    //deactivate previous slider
                    if (m_slider != null)
                        m_slider.active = false;

                    //activate new slider
                    m_slider = slider;
                    m_slider.active = true;
                }

                Platform platform = hitInfo.collider.GetComponent<Platform>();
                if (platform)
                {
                    transform.parent = hitInfo.collider.transform;
                }
            }
        }
        else
        {
            if (m_slider != null)
            {
                m_slider.active = false;
                m_slider.forward = Vector3.zero;
            }
            m_slider = null;

            transform.SetParent(null);
        }

        if (m_slider != null)
        {
            m_slider.forward = transform.forward;
        }

        bool sliderPressed = Input.GetAxis("Slider") > 0;
        if (!sliderPressed)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");



            //walk
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);

            //jump
            if (Input.GetButtonDown("Jump") && m_isGrounded)
            {
                //*-1 to make value positive (gravity is -9.81)
                velocity.y = Mathf.Sqrt(jumpHeight * gravity * -1.0f);
            }
        }

        //calculate gravity
        //gravity requiers time squered thus "* Time.deltaTime" is repeated
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
