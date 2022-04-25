﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private Transform m_destination;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CharacterController>() != null)
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.gameObject.transform.position = m_destination.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
        else
        {
            other.gameObject.transform.position = m_destination.position;
        }
    }
}
