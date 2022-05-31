using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Trap : MonoBehaviour
{
    [SerializeField]
    UnityEvent m_onPress;

    [SerializeField]
    UnityEvent m_onRelease;

    [SerializeField]
    bool m_activeOnStart = false;

    public void Press() => m_onPress?.Invoke();

    public void Release() => m_onRelease?.Invoke();

    private void Start()
    {
        if (m_activeOnStart)
        {
            Press();
        }
        else
        {
            Release();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Press();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Release();
        }
    }
}
