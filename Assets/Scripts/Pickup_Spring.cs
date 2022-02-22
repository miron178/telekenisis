using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Spring : MonoBehaviour
{
    [SerializeField]
    Transform m_hand;

    SpringJoint m_spring;
    ChangeDistance m_changeDistance;

    Rigidbody m_inRange;

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
            if (m_spring.connectedBody)
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
        m_inRange = null;

        Ray ray = new Ray(m_hand.position, m_hand.forward);
        float maxDistance = m_limitGrabDistance ? m_changeDistance.MaxDist : Mathf.Infinity;
        if (Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance))
        {
            //Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.collider.CompareTag("Pickup"))
            {
                m_inRange = hitInfo.collider.GetComponent<Rigidbody>();
            }
        }
        ButterFingers();
    }

    private void Grab()
    {
        m_spring.connectedBody = m_inRange;
        if (m_spring.connectedBody)
        {
            m_saveDrag = m_spring.connectedBody.drag;
            m_spring.connectedBody.drag = m_drag;
            m_releaseTime = m_timeToDrop;

            m_changeDistance.MoveCloseTo(m_spring.connectedBody.transform);
        }
    }

    private void Release()
    {
        if (m_spring.connectedBody)
        {
            m_spring.connectedBody.drag = m_saveDrag;
            m_spring.connectedBody.WakeUp();
            m_spring.connectedBody = null;
            m_releaseTime = 0f;
        }
    }

    private void ButterFingers()
    {
        if (m_spring.connectedBody)
        {
            if(m_spring.connectedBody == m_inRange)
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
    }

    private void Throw()
    {
        if (m_spring.connectedBody)
        {
            PickUp pickUp = m_spring.connectedBody.GetComponent<PickUp>();
            pickUp.thrown = true;
            
            m_spring.connectedBody.drag = m_saveDrag;
            m_spring.connectedBody.WakeUp();
            m_spring.connectedBody.AddForce(transform.forward * m_throwForce);
            m_spring.connectedBody = null;
            m_releaseTime = 0f;
        }
    }
}
