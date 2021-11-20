using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Spring : MonoBehaviour
{
    SpringJoint spring;

    Rigidbody inRange;

    [SerializeField]
    float drag = 100;
    float saveDrag;

    // Start is called before the first frame update
    void Start()
    {
        spring = GetComponent<SpringJoint>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (spring.connectedBody)
            {
                Release();
            }
            else
            {
                Grab();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!inRange && other.tag == "Pickup")
        {
            inRange = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (inRange == other.GetComponent<Rigidbody>())
        {
            inRange = null;
        }
    }

    Rigidbody FindObjectToGrab()
    {
        return inRange;
    }

    private void Grab()
    {
        spring.connectedBody = FindObjectToGrab();
        if (spring.connectedBody)
        {
            saveDrag = spring.connectedBody.drag;
            spring.connectedBody.drag = drag;
        }
    }

    private void Release()
    {
        if (spring.connectedBody)
        {
            spring.connectedBody.drag = saveDrag;
            spring.connectedBody = null;
        }
    }
}
