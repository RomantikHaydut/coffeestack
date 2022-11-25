using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    Animator anim;

    public List<GameObject> CoffeeList = new List<GameObject>();  // Coffees adds themselves to this list.

    public GameObject firstCoffee; // First coffee in coffees.
    public GameObject lastCoffee; // Last coffee in coffees.

    public float distanceBetweenCoffees;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        GameManager.StartGame += FindFirstCoffee;
    }
    void Update()
    {
        if (GameManager.Instance.isGameStarted)
        {
            RotateHand();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            LevelFinishEvents();
        }
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

    private void FindFirstCoffee() // In start
    {
        GameObject[] coffees = GameObject.FindGameObjectsWithTag("Coffee");
        if (coffees.Length > 0)
        {
            firstCoffee = coffees[0];
            int coffeeCount = coffees.Length;
            for (int i = 0; i < coffeeCount; i++)
            {
                if (coffees[i].transform.position.z < firstCoffee.transform.position.z)
                {
                    firstCoffee = coffees[i];

                }
            }
            firstCoffee.GetComponent<CoffeeController>().firstCoffee = true;
            firstCoffee.GetComponent<CoffeeController>().StarEventsFirstCoffee();
        }
        else
        {
            Debug.LogWarning("There is no coffee in scene !!! Put some coffees");
        }

    }

    public void FirstCoffee() // Set first coffee in list.
    {
        if (CoffeeList.Count > 0)
        {
            firstCoffee = CoffeeList[0].gameObject; // Random first coffee

            for (int i = 0; i < CoffeeList.Count; i++) // if any coffee is near to player more than random one it is the first coffee.
            {
                CoffeeList[i].GetComponent<CoffeeController>().firstCoffee = false;
                if (CoffeeList[i].transform.position.z < firstCoffee.transform.position.z)
                {
                    firstCoffee = CoffeeList[i];
                }
            }
            firstCoffee.GetComponent<CoffeeController>().firstCoffee = true;
        }
    }
    public void LastCoffee() // Set last coffee in list.
    {
        if (CoffeeList.Count > 0)
        {
            lastCoffee = CoffeeList[0].gameObject; // Random last coffee
            for (int i = 0; i < CoffeeList.Count; i++) // if any coffee is fat to player more than random one it is the last coffee.
            {
                CoffeeList[i].GetComponent<CoffeeController>().lastCoffee = false;
                if (CoffeeList[i].transform.position.z > lastCoffee.transform.position.z)
                {
                    lastCoffee = CoffeeList[i];

                }
            }
            lastCoffee.GetComponent<CoffeeController>().lastCoffee = true;
        }

    }

    public void LevelFinishEvents()
    {
        for (int i = 0; i < CoffeeList.Count; i++)
        {
            CoffeeController coffee = CoffeeList[i].GetComponent<CoffeeController>();
            if (coffee != null)
            {
                coffee.CalculateScore();
            }
        }

        print("Total Score : "+ FindObjectOfType<ScoreManager>().score);
    }

    private void OnDisable()
    {
        GameManager.StartGame -= FindFirstCoffee;
    }

}
