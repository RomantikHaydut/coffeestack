using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public float boundry;
    public float speed;
    void Update()
    {
        Movement();
        
    }


    void Movement()
    {
        transform.position += Vector3.forward * Time.deltaTime * speed;

        // Movement with mouse position ...
        Vector3 worldPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float posX = Mathf.Clamp(((worldPosition.x * 8) - 4), -boundry , boundry);
        transform.position = new Vector3( posX , transform.position.y, transform.position.z);
    }
}
