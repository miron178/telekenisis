using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 100f;
    [SerializeField]
    private Transform body;
    [SerializeField]
    private Transform hand;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    // Update is called once per frame
    void Update()
    {
        //get x & y axis
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //clamp x axis
        xRotation  = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate camera up and down
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //rotate character left and right
        body.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;

        hand.rotation = transform.rotation;
    }
}
