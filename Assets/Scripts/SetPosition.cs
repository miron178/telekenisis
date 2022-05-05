using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    //[SerializeField]
    //private Transform m_destination;

    public void MoveTo(Transform destination)
    {
        this.gameObject.transform.position = destination.position;
    }
}
