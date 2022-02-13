using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


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

    private bool perpetualMotion = false;

    private void Start()
    {
        m_bumperLeft.OnHit = Bump;
        m_bumperRight.OnHit = Bump;
        m_bumperForward.OnHit = Bump;
        m_bumperBack.OnHit = Bump;
    }
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool sliderPressed = Input.GetAxis("Slider") > 0;
        if (m_active && sliderPressed && (m_direction == Vector3.zero || !perpetualMotion))
        {
            Vector3 direction;
            if (x > 0)
            {
                direction = RightDir();
                z = 0;
            }
            else if (x < 0)
            {
                direction = LeftDir();
                z = 0;
            }
            else if (z > 0)
            {
                direction = ForwardDir();
                x = 0;
            }
            else if (z < 0)
            {
                direction = BackDir();
                x = 0;
            }
            else
            {
                direction = Vector3.zero;
                x = 0;
                z = 0;
            }

            if (direction != Vector3.zero)
            {
                Bumper bumper = BumperForDirection(direction);
                if (!bumper.hit)
                {
                    m_direction = direction;
                }
            }
        }
        if (perpetualMotion)
        {
            transform.position += m_direction * m_speed * Time.fixedDeltaTime;
        }
        else
        {
            Vector3 input = m_direction * Mathf.Abs(x + z);
            transform.position += input * m_speed * Time.fixedDeltaTime;
        }
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

    Bumper BumperForDirection(Vector3 direction)
    {
        // Only X or Z axis direction is supported, only x or z are allowed, but not both
        Assert.AreEqual(direction.y, 0.0f);
        Assert.IsFalse(Mathf.Abs(direction.x) > 0.5f && Mathf.Abs(direction.z) > 0.5f);

        if (direction.x > 0.5)
        {
            return m_bumperRight;
        }
        else if (direction.x < -0.5)
        {
            return m_bumperLeft;
        }
        else if (direction.z > 0.5)
        {
            return m_bumperForward;
        }
        else if (direction.z < -0.5)
        {
            return m_bumperBack;
        }
        else
        { 
            return null;
        }
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

        // highlight forward bumper
        Bumper bumper = BumperForDirection(ForwardDir());
        m_bumperForward.debugHighlihgt = false;
        m_bumperBack.debugHighlihgt = false;
        m_bumperLeft.debugHighlihgt = false;
        m_bumperRight.debugHighlihgt = false;
        bumper.debugHighlihgt = true;

    }
}
