using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private Transform destination;

    private bool m_held = false;
    private void Grab()
    {
        //disable collision
        GetComponent<BoxCollider>().enabled = true; //activating partialy solves problem (loop code above to solve problem permanently)

        //stop gravity
        GetComponent<Rigidbody>().useGravity = false;

        //this.transform.position = destination.position;
        this.transform.parent = destination.transform;

        m_held = true;
    }

    private void Release()
    {
        this.transform.parent = null;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        m_held = false;
    }

    private void OnMouseDrag()
    {
        if (m_held)
        {
            //stop object from floating or turning
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

    }

    private void OnMouseDown() //may want to use mouse up and down as axis
    {
        Grab();
    }

    private void OnMouseExit()
    {
        //Release();
    }
    private void OnMouseUp()
    {
        Release();
    }

    //get player dir
    //use mousewheel to move object along player dir
}
