using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCoffeeController : MonoBehaviour
{
    GameObject player; // Hand

    HandController handController;

    GameObject followCoffee;

    float offset; // Distance between coffees.

    bool following = false;

    public int coffeeAmount = 0; // For fill with coffee
    List<GameObject> coffeeGrades = new List<GameObject>();
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        handController = player.GetComponent<HandController>();

        for (int i = 0; i < transform.childCount; i++)  //Inactive child coffees and get them in a list.
        {
            coffeeGrades.Add(transform.GetChild(i).gameObject);
            coffeeGrades[i].gameObject.SetActive(false);
        }
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coffee"))
        {
            if (!following)
            {
                followCoffee = handController.LastCoffee();
                StartCoroutine(Follow());
                handController.CoffeeList.Add(gameObject);

                following = true;
                enabled = false;
            }
        }
    }

    IEnumerator Follow()
    {
        offset = handController.distanceBetweenCoffees;
        while (true)
        {
            yield return null;
            float xPos = Mathf.Lerp(transform.position.x, followCoffee.transform.position.x, 5f*Time.deltaTime);
            float zPos = (followCoffee.transform.position.z + offset);
            transform.position = new Vector3(xPos, followCoffee.transform.position.y, zPos);
        }
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
