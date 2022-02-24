using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDistance : MonoBehaviour
{
    Movement m_movement = null;
    bool m_movePlayer = false;

    [SerializeField]
    float m_minDist = 2f;
    [SerializeField]
    float m_maxDist = Mathf.Infinity;
    [SerializeField]
    float m_reactMouseSpeed = 200f;
    [SerializeField]
    CharacterController m_player;
    [SerializeField]
    Transform m_hand;

    public float MaxDist { get => m_maxDist; }
    public bool movePlayer { get => m_movePlayer; set => m_movePlayer = value; }

    private void Start()
    {
        m_movement = GetComponentInParent<Movement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && m_movement.isGrounded)
        {
            UpdateDistance(m_minDist, m_movePlayer);
        }
        else
        {
            Vector3 distance = transform.position - transform.parent.position;
            float change = Input.GetAxis("Mouse ScrollWheel") * m_reactMouseSpeed * Time.deltaTime;
            UpdateDistance(distance.magnitude + change, m_movePlayer);
        }
    }

    void UpdateDistance(float newDistance, bool movePlayer = true)
    {

        Vector3 distance = transform.position - m_hand.position;
        Vector3 minDist = distance.normalized * m_minDist;
        Vector3 maxDist = distance.normalized * m_maxDist;
        
        Debug.DrawRay(transform.parent.position, minDist, Color.red);
        Debug.DrawRay(transform.parent.position + minDist, distance - minDist, m_movePlayer ? Color.blue : Color.yellow);
        Debug.DrawRay(transform.parent.position + distance, maxDist - distance, Color.green);

        newDistance = Mathf.Clamp(newDistance, m_minDist, m_maxDist);
        Vector3 oldPosition = transform.position;
        transform.position = m_hand.position + distance.normalized * newDistance;

        if (movePlayer)
        {
            m_player.Move(oldPosition - transform.position);
        }
    }

    public void MoveCloseTo(Transform other)
    {
        Vector3 delta = other.position - transform.parent.position;
        UpdateDistance(delta.magnitude, false);
        if(m_movePlayer)
        {
            UpdateDistance(m_minDist, true);
        }
    }
}
