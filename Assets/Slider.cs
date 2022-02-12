using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private float m_speed = 50.0f;

    [SerializeField]
    private Vector3 m_direction;

    [SerializeField]
    private bool m_perpetualMotion = false;

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (m_perpetualMotion)
        {
            m_direction = transform.right * x + transform.forward * z;
        }
        else
        {
            x = Mathf.Abs(x) < 0.5 ? 0 : Mathf.Sign(x);
            z = Mathf.Abs(z) < 0.5 ? 0 : Mathf.Sign(z);
            if (m_direction == Vector3.zero)
            {
                m_direction = transform.right * x + transform.forward * z;
            }
        }
        controller.Move(m_direction * m_speed * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Wall"))
        {
            m_direction = Vector3.zero;
        }
    }
}
