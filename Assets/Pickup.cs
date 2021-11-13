using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private Transform destination;

    private void OnMouseDown()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<BoxCollider>().enabled  = false;
        GetComponent<Rigidbody>().useGravity = false;
        //this.transform.position = destination.position;
        this.transform.parent = destination.transform;
    }

    private void OnMouseUp()
    {
        this.transform.parent = null;
        GetComponent<BoxCollider>().enabled  = true;
        GetComponent<Rigidbody>().useGravity = true;
    }
}
