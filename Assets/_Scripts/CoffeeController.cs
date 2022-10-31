using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeController : MonoBehaviour
{
    GameObject player; // Hand

    HandController handController;

    Vector3 offset; // Offset with hand.

    public int coffeeAmount = 0; // For fill with coffee
    List<GameObject> coffeeGrades = new List<GameObject>();
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        handController = player.GetComponent<HandController>();
        offset = transform.position - player.transform.position;
        handController.CoffeeList.Add(gameObject);


        for (int i = 0; i < transform.childCount; i++)  //Inactive child coffees and get them in a list.
        {
            coffeeGrades.Add(transform.GetChild(i).gameObject);
            coffeeGrades[i].gameObject.SetActive(false);
        }
        FillCoffee();
        
    }

    
    void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        transform.position = player.transform.position + offset;
    }

    public void FillCoffee()
    {
        if (coffeeAmount == 1)
        {
            coffeeGrades[0].gameObject.SetActive(true);
        }
        else if (coffeeAmount == 2)
        {
            coffeeGrades[1].gameObject.SetActive(true);
        }
        else if (coffeeAmount == 3)
        {
            coffeeGrades[2].gameObject.SetActive(true);
        }

    }
}
