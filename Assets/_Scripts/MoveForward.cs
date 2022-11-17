using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed;

    [SerializeField] GameObject ground;
    private float boundry; // Grounds boundries.
    [SerializeField] float boundryOffset;
    private float sensitive; // Distance from camera.
    [SerializeField] [Range(1, 4)] float sensitiveMultiplier;

    private void Awake()
    {
        boundry = ground.GetComponent<BoxCollider>().bounds.extents.x;

        sensitive = transform.position.z - Camera.main.transform.position.z;
    }
    void Update()
    {
        if (GameManager.Instance.isGameStarted)
        {
            Movement();
        }
    }

    void Movement()
    {
        //Movement forward.
        transform.position += Vector3.forward * Time.deltaTime * speed;
        // Movement with mouse position in x axis...
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, sensitive * sensitiveMultiplier));
        float posX = Mathf.Clamp((worldPosition.x), -boundry + boundryOffset, boundry - boundryOffset);
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }
}
