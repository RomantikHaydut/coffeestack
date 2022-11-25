using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;

    Transform target;

    Vector3 offset;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        offset = target.position-transform.position;
    }
    void LateUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (GameManager.Instance.isGameStarted)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z - offset.z);
        }
    }
}
