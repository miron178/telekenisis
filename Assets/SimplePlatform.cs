using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlatform : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_destination;
    private int m_target = 0;

    [SerializeField]
    private bool m_changeDir = false;

    [SerializeField]
    private float m_speed = 1;

    [SerializeField]
    private bool m_forward = true;

    private int m_first = 0;
    private int m_last  = 0;

    
    private void Start()
    {
        m_last = m_destination.Length - 1;
    }

    void Update()
    {
        // move towards the current target
        Vector3 direction = m_destination[m_target].transform.position - gameObject.transform.position;
        Vector3 move = direction.normalized * m_speed * Time.deltaTime;

        // make sure the last step doesn't overshoot
        if (move.sqrMagnitude > direction.sqrMagnitude)
        {
            move = direction;
        }
        gameObject.transform.position += move;


        if (gameObject.transform.position == m_destination[m_target].transform.position)
        {
            // reached the current target, choose next one
            if(m_changeDir) //return backwards
            {
                if(m_target == m_first|| m_target == m_last)
                {
                    m_forward = !m_forward;
                }
                m_target += m_forward ? 1 : -1;
            }
            else //loop
            {
                m_target += m_forward ? 1 : -1;
                if (m_target > m_last)
                {
                    m_target = m_first;
                }
                else if (m_target < m_first)
                {
                    m_target = m_last;
                }
            }
        }
    }
}
