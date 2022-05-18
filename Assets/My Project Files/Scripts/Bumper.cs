using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField]
    string m_tag = "";

    private int m_hitCount = 0;
    public int hitCount { get => m_hitCount; }
    
    public delegate void OnHitDelegate(Bumper bumper);
    OnHitDelegate m_onHit = null;
    public OnHitDelegate OnHit { get => m_onHit; set => m_onHit = value; }

    private bool m_debugHighlihgt = false;
    public bool debugHighlihgt { get => m_debugHighlihgt; set => m_debugHighlihgt = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_tag))
        {
            m_hitCount++;
            Debug.Log("bumper: " + m_tag + " enter " + m_hitCount);
            if (m_onHit != null)
            {
                m_onHit(this);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(m_tag))
        {
            Debug.Log("bumper: " + m_tag + " exit " + m_hitCount);
            m_hitCount--;
        }
    }

    private void OnDrawGizmos()
    {
        float highlight = m_debugHighlihgt ? 1.0f : 0.3f;
        if (m_hitCount == 0)
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
