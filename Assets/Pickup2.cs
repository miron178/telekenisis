using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup2 : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    float maxGrabDistance = 10f;
    [SerializeField]
    Transform dest;

    Rigidbody otherRB;

    void Update()
    {
        if (otherRB)
        {
            otherRB.MovePosition(dest.transform.position);
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (otherRB)
            {
                otherRB.isKinematic = false;
                otherRB = null;
            }
            else
            {
                RaycastHit hit;
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                if (Physics.Raycast(ray, out hit, maxGrabDistance))
                {
                    otherRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                    if (otherRB)
                    {
                        otherRB.isKinematic = true;
                    }
                }
            }
        }
    }

    //get player dir
    //use mousewheel to move object along player dir
}
