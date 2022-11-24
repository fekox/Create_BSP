using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Creo el character controller.
    public CharacterController controller;

    //Creo el character speed.
    public float speed = 20.0f;

    void Update()
    {
        //Creo una variable x y le setteo el movimiento horizontal.
        float x = Input.GetAxis("Horizontal");

        //Creo una variable Y y le setteo el movimiento vertical.
        float z = Input.GetAxis("Vertical");

        //Seteo el movimiento.
        Vector3 move = transform.right * x + transform.forward * z;

        //Asigno el movimiento segun la velosidad por el delta time.
        controller.Move(move * speed * Time.deltaTime);
    }
}
