using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Main camera shoul be player child
//Script should be assigned for Main camera
//Body - Player(Transform)

public class Look : MonoBehaviour
{
    public Transform body;

    private float xRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        body.Rotate(Vector3.up * mouseX);
    }
}
