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

    private Vector3 m_forward = Vector3.zero;
    public Vector3 forward { get => m_forward; set => m_forward = value; }

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
                m_direction = RightDir();
            }
            else if (x < 0 && !m_bumperLeft.hit)
            {
                m_direction = LeftDir();
            }
            else if (z > 0 && !m_bumperForward.hit)
            {
                m_direction = ForwardDir();
            }
            else if (z < 0 && !m_bumperBack.hit)
            {
                m_direction = BackDir();
            }
            // else stay as zero
        }

        transform.position += m_direction * m_speed * Time.fixedDeltaTime;
    }

    Vector3 ForwardDir()
    {
        Vector3 forwardDir = Vector3.zero;
        if (Mathf.Abs(m_forward.x) >= Mathf.Abs(m_forward.z))
        {
            forwardDir.x = Mathf.Sign(m_forward.x);
        }
        else
        {
            forwardDir.z = Mathf.Sign(m_forward.z);
        }
        return forwardDir;
    }

    Vector3 BackDir()
    {
        return ForwardDir() * -1.0f;
    }

    Vector3 RightDir()
    {
        return Quaternion.AngleAxis(90, Vector3.up) * ForwardDir();
    }

    Vector3 LeftDir()
    {
        return Quaternion.AngleAxis(-90, Vector3.up) * ForwardDir();
    }

    void Bump(Bumper bumper)
    {
        m_direction = Vector3.zero;
    }

    //debug stuff
    private void OnDrawGizmos()
    {
        //show triggers
        if (m_active)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
        }
        else
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
        }
        Gizmos.DrawCube(transform.position, transform.lossyScale);

        //show front by player
        Vector3 forwardOrigin = transform.position;
        forwardOrigin.y += transform.lossyScale.y / 2.0f + 0.1f;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(forwardOrigin, m_forward);
        
        //show front by cardenal directions to player
        if(m_forward != Vector3.zero)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(forwardOrigin, ForwardDir() * 1.5f);
        }
    }
}
