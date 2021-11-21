using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Spring : MonoBehaviour
{
    [SerializeField]
    Transform m_hand;

    SpringJoint m_spring;

    Rigidbody m_inRange;

    [SerializeField]
    float m_drag = 100f;
    float m_saveDrag;

    [SerializeField]
    float m_timeToDrop = 3f;
    float m_releaseTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_spring = GetComponent<SpringJoint>();
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
        Detect();
    }

    void Detect()
    {
        m_inRange = null;

        Ray ray = new Ray(m_hand.position, m_hand.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
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
        }
    }

    private void Release()
    {
        if (m_spring.connectedBody)
        {
            m_spring.connectedBody.drag = m_saveDrag;
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
}
