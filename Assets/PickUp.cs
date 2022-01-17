using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private bool m_thrown = false;
    private Rigidbody m_rb;

    public bool thrown { get => m_thrown; set => m_thrown = value; }

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
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
