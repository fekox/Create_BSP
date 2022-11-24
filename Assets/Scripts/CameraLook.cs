using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSesitivity = 200;

    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Seteo la variable X del mouse.
        float mouseX = Input.GetAxis("Mouse X") * mouseSesitivity * Time.deltaTime;
        
        //Seteo la variable Y del mouse.
        float mouseY = Input.GetAxis("Mouse Y") * mouseSesitivity * Time.deltaTime;

        //Rotacion Horizontal.
        xRotation -= mouseY;

        //Rotacion Vertical.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
