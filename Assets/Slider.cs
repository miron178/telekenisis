using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 50.0f;

    private Vector3 m_direction;

    [SerializeField]
    private Bumper m_bumperLeft;

    [SerializeField]
    private Bumper m_bumperRight;

    [SerializeField]
    private Bumper m_bumperForward;

    [SerializeField]
    private Bumper m_bumperBack;

    private bool m_active = false;

    public bool active { get => m_active; set => m_active = value; }

    private void Start()
    {
        m_bumperLeft.OnHit = Bump;
        m_bumperRight.OnHit = Bump;
        m_bumperForward.OnHit = Bump;
        m_bumperBack.OnHit = Bump;
    }
    void FixedUpdate()
    {
        bool sliderPressed = Input.GetAxis("Slider") > 0;
        if (m_active && sliderPressed && m_direction == Vector3.zero)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            if (x > 0 && !m_bumperRight.hit)
            {
                m_direction = Vector3.right;
            }
            else if (x < 0 && !m_bumperLeft.hit)
            {
                m_direction = Vector3.left;
            }
            else if (z > 0 && !m_bumperForward.hit)
            {
                m_direction = Vector3.forward;
            }
            else if (z < 0 && !m_bumperBack.hit)
            {
                m_direction = Vector3.back;
            }
            // else stay as zero
        }

        transform.position += m_direction * m_speed * Time.fixedDeltaTime;
    }

    void Bump(Bumper bumper)
    {
        m_direction = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (m_active)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
        }
        else
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
        }
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }
}
