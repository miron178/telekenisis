using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PickUp>())
        {
            other.transform.parent = transform;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == transform)
        {
            other.transform.SetParent(null);
        }
    }
}
