using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField]
    UnityEvent m_onPress;

    [SerializeField]
    UnityEvent m_onRelease;

    [SerializeField]
    UnityEvent m_onEnter;

    [SerializeField]
    UnityEvent m_onExit;

    [SerializeField]
    bool m_activeOnStart = false;

    public void Press() => m_onPress?.Invoke();
    public void Release() => m_onRelease?.Invoke();
    public void Enter() => m_onEnter?.Invoke();
    public void Exit() => m_onExit?.Invoke();

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
        if (other.tag == "Player" || other.GetComponent<PickUp>())
        {
            Press();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.GetComponent<PickUp>())
        {
            Release();
        }
    }
}
