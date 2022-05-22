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
    //[SerializeField]
    //private float groundCheckRadius = 0.5f;
    private LayerMask groundMask;

    public bool isGrounded { get => controller.isGrounded; }

    private Vector3 velocity;

    private Pickup_Spring m_pickupSpring;

    [SerializeField]
    private float m_sliderDetectionDistance = 0.5f;
    Slider m_slider = null;

    private enum State
    {
        WALKING,
        SLIDING,
        FALLING
    }
    private State state = State.FALLING;



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
        switch (state)
        {
            case State.WALKING:
                Walk();
                break;
            case State.SLIDING:
                Slide();
                break;
            case State.FALLING:
                Fall();
                break;
            default:
                Debug.LogError("The state is unknown.");
                break;
        }
    }

    private void DetectSliderOrPlatform()
    {
        Debug.DrawRay(groundCheck.position, new Vector3(0, -m_sliderDetectionDistance, 0), Color.white);
        Ray ray = new Ray(groundCheck.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, m_sliderDetectionDistance))
        {
            if (!DetectSlider(hitInfo))
            {
                DeactivateSlider();
            }
            if (!DetectPlatform(hitInfo))
            {
                DeactivatePlatform();
            }
        }
        else
        {
            DeactivateSlider();
            DeactivatePlatform();
        }
    }

    private bool DetectSlider(RaycastHit hitInfo)
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
        return slider != null;
    }

    private bool DetectPlatform(RaycastHit hitInfo)
    {
        Platform platform = hitInfo.collider.GetComponent<Platform>();
        if (platform)
        {
            transform.parent = hitInfo.collider.transform;
        }
        return platform != null;
    }

    private void DeactivateSlider()
    {
        if (m_slider != null)
        {
            m_slider.active = false;
            m_slider.forward = Vector3.zero;
        }
        m_slider = null;
    }
    private void DeactivatePlatform()
    {
        transform.SetParent(null);
    }

    private void UpdatePlayerVelocityXZ()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //walk
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    private void StartWalk()
    {
        state = State.WALKING;
        velocity.y = 0;
        DetectSliderOrPlatform();
    }

    private void Walk()
    {
        UpdatePlayerVelocityXZ();

        if (Input.GetButtonDown("Jump"))
        {
            //*-1 to make value positive (gravity is -9.81)
            velocity.y = Mathf.Sqrt(jumpHeight * gravity * -1.0f);
            StartFall();
        }
        else if (Input.GetAxis("Slider") > 0)
        {
            StartSlide();
        }
        else
        {
            //keep controller grounded (playerVelocity.y = 0 doesn't work)
            velocity.y = gravity * Time.deltaTime;
        }

        Vector3 movement = velocity * Time.deltaTime;
        if (movement.magnitude >= controller.minMoveDistance)
        {
            controller.Move(movement);
            if (controller.isGrounded)
            {
                DetectSliderOrPlatform();
            }
            else
            {
                StartFall();
            }
        }
    }

    private void StartSlide()
    {
        state = State.SLIDING;
    }

    private void Slide()
    {
        if (Input.GetAxis("Slider") > 0)
        {
            //keep controller grounded (playerVelocity.y = 0 doesn't work)
            velocity.y = gravity * Time.deltaTime;

            m_slider.forward = transform.forward;
        }
        else
        {
            StartWalk();
        }
    }

    private void StartFall()
    {
        state = State.FALLING;
        DeactivateSlider();
        DeactivatePlatform();
    }

    private void Fall()
    {
        UpdatePlayerVelocityXZ();

        //*-1 to make value positive (gravity is -9.81)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            StartWalk();
        }
    }
}
