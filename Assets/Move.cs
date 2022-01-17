using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private Vector3 direction = Vector3.forward;

    [SerializeField]
    private float swapDelay = 3.0f;

    private float swapTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        swapTime = Time.time + swapDelay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (swapTime <= Time.time)
        {
            swapTime = Time.time + swapDelay;
            direction = direction * -1.0f;
        }

        transform.position += direction * speed * Time.fixedDeltaTime;
    }
}
