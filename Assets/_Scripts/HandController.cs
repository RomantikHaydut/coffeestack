using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public float boundry; // Screen boundry
    public float speed;

    Animator anim;

    public List<GameObject> CoffeeList = new List<GameObject>();  // Coffees adds themselves to this list.

    public GameObject lastCoffee; // Last coffee in coffees.

    public float distanceBetweenCoffees;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Movement();
        RotateHand();
        
    }


    void Movement()
    {
        transform.position += Vector3.forward * Time.deltaTime * speed;

        // Movement with mouse position ...
        Vector3 worldPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float posX = Mathf.Clamp(((worldPosition.x * 8) - 4), -boundry , boundry);
        transform.position = new Vector3( posX , transform.position.y, transform.position.z);
    }

    void RotateHand()
    {
        if (transform.position.x <= 0)
        {
            anim.SetBool("isLeft", true);
            anim.SetBool("isRight", false);
        }
        else
        {
            anim.SetBool("isLeft", false);
            anim.SetBool("isRight", true);
        }
    }

    public GameObject LastCoffee() // Sets last coffee in list.
    {
        float distance = transform.position.z;
        foreach (GameObject coffee in CoffeeList)
        {
            if (coffee.transform.position.z > distance)
            {
                lastCoffee = coffee;
            }
        }
        return lastCoffee;
    }
}
