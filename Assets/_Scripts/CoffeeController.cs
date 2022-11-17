using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeController : MonoBehaviour
{
    GameObject player; // Hand

    HandController handController;

    GameObject followCoffee;

    GameObject firstCoffeePos;

    public int coffeeAmount = 0; // For fill with coffee
    List<GameObject> coffeeGrades = new List<GameObject>();

    float offset; // Distance between coffees.

    public bool following = false;

    public bool firstCoffee;
    public bool lastCoffee;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        handController = player.GetComponent<HandController>();
        firstCoffeePos = GameObject.FindGameObjectWithTag("FirstCoffeePos");
    }
    void Start()
    {
        SetCoffeeGrades();
        FillCoffee();
    }

    public void StarEventsFirstCoffee()
    {
        handController.CoffeeList.Add(gameObject);
    }

    
    void Update()
    {
        if (firstCoffee)
        {
            FollowPlayer();
        }
    }

    public void SetLastCoffee()
    {
        handController.lastCoffee = gameObject;
    }
    public void SetFirstCoffee()
    {
        handController.firstCoffee = gameObject;
    }

    void FollowPlayer() // For first coffee
    {
        transform.position = firstCoffeePos.transform.position;
    }

    IEnumerator Follow() // For other coffees.
    {
        offset = handController.distanceBetweenCoffees;
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (!following)
            {
                yield break;
            }
            float xPos = Mathf.Lerp(transform.position.x, followCoffee.transform.position.x, 5f * Time.deltaTime);
            float zPos = (followCoffee.transform.position.z + offset);
            transform.position = new Vector3(xPos, followCoffee.transform.position.y, zPos);
        }
    }

    public void StopFollow() // For other coffees.
    {
        following = false;
    }

    public void SetCoffeeGrades()
    {
        for (int i = 0; i < transform.childCount; i++)  //Inactive child coffees and get them in a list.
        {
            coffeeGrades.Add(transform.GetChild(i).gameObject);
            coffeeGrades[i].gameObject.SetActive(false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameStarted)
        {
            if (!firstCoffee)
            {
                if (other.gameObject.CompareTag("Coffee") || other.gameObject.CompareTag("Player"))
                {
                    if (!following)
                    {
                        followCoffee = handController.LastCoffee();
                        StartCoroutine(Follow());
                        handController.CoffeeList.Add(gameObject);
                        handController.lastCoffee = gameObject;
                        following = true;
                    }
                }
            }
            else
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    lastCoffee = true;
                    handController.lastCoffee = gameObject;
                }
            }
        }
        
    }
}
