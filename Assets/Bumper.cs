using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField]
    string m_tag = "";

    private Collider m_hit = null;
    public Collider hit { get => m_hit; }
    
    public delegate void OnHitDelegate(Bumper bumper);
    OnHitDelegate m_onHit = null;
    public OnHitDelegate OnHit { get => m_onHit; set => m_onHit = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_tag))
        {
            Debug.Log("bumper: " + m_tag);
            m_hit = other;
            if (m_onHit != null)
            {
                m_onHit(this);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (m_hit == other)
            m_hit = null;
    }

    private void OnDrawGizmos()
    {
        if (m_hit == null)
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
