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

    private bool m_debugHighlihgt = false;
    public bool debugHighlihgt { get => m_debugHighlihgt; set => m_debugHighlihgt = value; }

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
        float highlight = m_debugHighlihgt ? 1.0f : 0.3f;
        if (m_hit == null)
        {
            Gizmos.color = new Color(0, highlight, 0, 0.5f);
        }
        else
        {
            Gizmos.color = new Color(highlight, 0, 0, 0.5f);
        }
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }
}
