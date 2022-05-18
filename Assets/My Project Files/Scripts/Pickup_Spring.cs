using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Spring : MonoBehaviour
{
    [SerializeField]
    Transform m_hand;

    SpringJoint m_spring;
    ChangeDistance m_changeDistance;
    Button m_buttonInRange = null;
    Button m_buttonPressed = null;

    Rigidbody m_pickupInRange;

    [SerializeField]
    float m_drag = 100f;
    float m_saveDrag;

    [SerializeField]
    float m_timeToDrop = 1f;
    float m_releaseTime = 0f;

    [SerializeField]
    float m_throwForce = 500f;

    [SerializeField]
    bool m_limitGrabDistance = false;

    public GameObject heldObject { get => m_spring.connectedBody ? m_spring.connectedBody.gameObject : null; }

    private PickUp pickUp { get => m_spring.connectedBody?.GetComponent<PickUp>(); }

    // Start is called before the first frame update
    void Start()
    {
        m_spring = GetComponent<SpringJoint>();
        m_changeDistance = GetComponent<ChangeDistance>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (m_spring.connectedBody || (m_buttonInRange && m_buttonInRange == m_buttonPressed))
            {
                Release();
            }
            else
            {
                Grab();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Throw();
        }
        Detect();
    }

    public bool IsHolding()
    {
        return m_spring.connectedBody != null;
    }

    void Detect()
    {
        m_pickupInRange = null;
        m_buttonInRange = null;

        Ray ray = new Ray(m_hand.position, m_hand.forward);
        float maxDistance = m_limitGrabDistance ? m_changeDistance.MaxDist : Mathf.Infinity;
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance))
        {
            //Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.collider.GetComponent<PickUp>())
            {
                m_pickupInRange = hitInfo.collider.GetComponent<Rigidbody>();
            }
            m_buttonInRange = hitInfo.collider.GetComponent<Button>();
        }
        ButterFingers();
    }

    private void Grab()
    {
        m_spring.connectedBody = m_pickupInRange;
        if (m_spring.connectedBody)
        {
            m_saveDrag = m_spring.connectedBody.drag;
            m_spring.connectedBody.drag = m_drag;
            m_releaseTime = m_timeToDrop;

            m_changeDistance.movePlayer = !pickUp.movable;
            m_changeDistance.MoveCloseTo(m_spring.connectedBody.transform);
        }
        else if (m_buttonInRange)
        {
            m_buttonInRange.Press();
            m_buttonPressed = m_buttonInRange;
        }
    }

    public void Release()
    {
        if (m_spring.connectedBody)
        {
            m_spring.connectedBody.drag = m_saveDrag;
            m_spring.connectedBody.WakeUp();
            m_spring.connectedBody = null;
            m_releaseTime = 0f;
        }
        if (m_buttonPressed)
        {
            m_buttonPressed.Release();
            m_buttonPressed = null;
        }
        m_changeDistance.movePlayer = false;
    }

    private void ButterFingers()
    {
        if ((m_spring.connectedBody && m_spring.connectedBody == m_pickupInRange) || 
            (m_buttonPressed && m_buttonPressed == m_buttonInRange))
        {
            m_releaseTime = m_timeToDrop;
        }
        else 
        {
            m_releaseTime -= Time.deltaTime;
            if (m_releaseTime <= 0)
            {
                Release();
            }
        }
    }

    private void Throw()
    {
        if (m_spring.connectedBody)
        {
            pickUp.thrown = true;
            
            m_spring.connectedBody.drag = m_saveDrag;
            m_spring.connectedBody.WakeUp();
            m_spring.connectedBody.AddForce(transform.forward * m_throwForce);
            m_spring.connectedBody = null;
            m_releaseTime = 0f;
        }
    }
}
