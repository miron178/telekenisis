using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDistance : MonoBehaviour
{
    Movement m_movement = null;

    [SerializeField]
    float m_minDist = 2f;
    [SerializeField]
    float m_maxDist = Mathf.Infinity;
    [SerializeField]
    float m_reactMouseSpeed = 200f;

    public float MaxDist { get => m_maxDist; }

    private void Start()
    {
        m_movement = GetComponentInParent<Movement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && m_movement.isGrounded)
        {
            UpdateDistance(m_minDist);
        }
        else
        {
            Vector3 distance = transform.position - transform.parent.position;
            float change = Input.GetAxis("Mouse ScrollWheel") * m_reactMouseSpeed * Time.deltaTime;
            UpdateDistance(distance.magnitude + change);
        }
    }

    void UpdateDistance(float newDistance)
    {

        Vector3 distance = transform.position - transform.parent.position;
        Vector3 minDist = distance.normalized * m_minDist;
        Vector3 maxDist = distance.normalized * m_maxDist;
        
        Debug.DrawRay(transform.parent.position, minDist, Color.red);
        Debug.DrawRay(transform.parent.position + minDist, distance - minDist, Color.yellow);
        Debug.DrawRay(transform.parent.position + distance, maxDist - distance, Color.green);

        newDistance = Mathf.Clamp(newDistance, m_minDist, m_maxDist);
        transform.position = transform.parent.position + distance.normalized * newDistance;
    }

    public void MoveCloseTo(Transform other)
    {
        Vector3 newDistance = other.position - transform.parent.position;
        UpdateDistance(newDistance.magnitude);
    }
}
