using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float speed;

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        transform.position += Vector3.forward * Time.deltaTime * speed;
    }
}
