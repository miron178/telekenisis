using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpToHub : MonoBehaviour
{
    [SerializeField]
    private Transform m_destination;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Esc") > 0)
        {
            this.GetComponent<CharacterController>().enabled = false;
            this.gameObject.transform.position = m_destination.position;
            this.GetComponent<CharacterController>().enabled = true;
        }
    }
}
