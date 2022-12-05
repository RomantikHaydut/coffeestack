using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed;

    Transform target;

    float offset;

    float finishOffset;

    float angle;

    float finishAngle;

    float height;

    float finishHeight;

    bool islevelFinished;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        offset = target.position.z - transform.position.z;
        finishOffset = offset * 2;

        angle = transform.eulerAngles.x;
        finishAngle = angle + 15;

        height = transform.position.y - GameObject.FindGameObjectWithTag("Ground").transform.position.y;
        finishHeight = height * 2;
    }
    void LateUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (GameManager.Instance.isGameStarted)
        {
            if (islevelFinished)
            {
                height = Mathf.Lerp(height, finishHeight, Time.deltaTime);
                offset = Mathf.Lerp(offset, finishOffset, Time.deltaTime);
                angle = Mathf.Lerp(angle, finishAngle, Time.deltaTime);
                transform.eulerAngles = new Vector3(angle, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            transform.position = new Vector3(transform.position.x, height, target.position.z - offset);
        }
    }

    public IEnumerator LevelFinish()
    {
        islevelFinished = true;
        yield break;
    }
}
