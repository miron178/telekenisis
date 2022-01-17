using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PickUp pickUp = collision.collider.GetComponent<PickUp>();

        if (pickUp && pickUp.thrown)
        {
            //switch to animation later
            pickUp.thrown = false;
            Destroy(gameObject);
        }
    }
}
