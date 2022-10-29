using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float speed;

    private void Start()
    {
        if (speed == 0)
        {
            speed = 1;
        }
    }
    void Update()
    {
        transform.position += Vector3.forward * Time.deltaTime * speed;
    }
}
