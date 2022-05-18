using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private bool m_thrown = false;
    private Rigidbody m_rb;

    public bool thrown { get => m_thrown; set => m_thrown = value; }

    [SerializeField]
    private bool m_movable = true;
    public bool movable { get => m_movable; }
    
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        if (!m_movable)
        {
            m_rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rb.velocity.magnitude < 0.05f)
        {
            thrown = false;
        }
    }
}
