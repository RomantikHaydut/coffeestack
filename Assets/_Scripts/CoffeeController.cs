using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeController : MonoBehaviour
{
    GameObject player; // Hand

    HandController handController;

    GameObject followCoffee;

    GameObject firstCoffeePos;

    // For fill with coffee
    public int coffeeAmount = 0; 
    List<GameObject> coffeeGrades = new List<GameObject>();
    List<GameObject> coffeeHeats = new List<GameObject>();
    public bool triggerWithCoffeeMachine = false;
    public float distanceTakenWithCoffeeMachine;
    private float triggerStartPoint;
    private bool isCoffeeHeat;

    float offset; // Distance between coffees.

    public bool following = false;

    public bool onGround;

    public bool firstCoffee;
    public bool lastCoffee;

    public float boundry;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        handController = player.GetComponent<HandController>();
        firstCoffeePos = GameObject.FindGameObjectWithTag("FirstCoffeePos");

        onGround = true;

        print("Coffe Scale : " + transform.localScale.z);
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

    
    void LateUpdate()
    {
        if (onGround)
        {
            if (firstCoffee)
            {
                FollowPlayer();
            }
            else
            {
                FollowPreviousCoffee();
            }
        }

        CheckBoundry();
    }

    public void CalculateTakenDistanceWithCoffeeMachine()
    {
        distanceTakenWithCoffeeMachine = Mathf.Abs(TriggerEndPoint() - triggerStartPoint);
    }

    public void DefineStartPoint()
    {
        triggerStartPoint = TriggerStartPoint();
    }
    public float TriggerStartPoint()
    {
        float startPoint = transform.position.z;
        return startPoint;
    }

    public float TriggerEndPoint()
    {
        float endPoint = transform.position.z;
        return endPoint;
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

    void FollowPreviousCoffee()
    {
        if (following)
        {
            offset = handController.distanceBetweenCoffees;
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
        for (int i = 0; i < transform.childCount; i++)  //Inactive child coffees and heats and get them in a list.
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == "Heat")
            {
                coffeeHeats.Add(child);
                child.SetActive(false);
            }
            else
            {
                coffeeGrades.Add(child);
                child.gameObject.SetActive(false);
            }
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

    public void HeatCoffee()
    {
        if (coffeeAmount == 1)
        {
            coffeeHeats[0].gameObject.SetActive(true);
            isCoffeeHeat = true;
        }
        else if (coffeeAmount == 2)
        {
            coffeeHeats[1].gameObject.SetActive(true);
            isCoffeeHeat = true;
        }
        else if (coffeeAmount == 3)
        {
            coffeeHeats[2].gameObject.SetActive(true);
            isCoffeeHeat = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameStarted)
        {
            if (onGround)
            {
                if (!firstCoffee)
                {
                    if (handController.firstCoffee != null)
                    {
                        if (other.gameObject.CompareTag("Coffee"))
                        {
                            if (!following)
                            {
                                CoffeeController coffee = other.gameObject.GetComponent<CoffeeController>();
                                if (coffee.following || coffee.firstCoffee)
                                {
                                    handController.LastCoffee();
                                    followCoffee = handController.lastCoffee;
                                    handController.CoffeeList.Add(gameObject);
                                    handController.LastCoffee();
                                    following = true;
                                }
                            }
                        }
                        else if (other.gameObject.CompareTag("Player"))
                        {
                            if (!following)
                            {
                                handController.LastCoffee();
                                followCoffee = handController.lastCoffee;
                                handController.CoffeeList.Add(gameObject);
                                handController.LastCoffee();
                                following = true;
                            }
                        }
                    }
                    else
                    {
                        if (other.gameObject.CompareTag("Player"))
                        {
                            firstCoffee = true;
                            handController.firstCoffee = gameObject;
                            handController.CoffeeList.Add(gameObject);
                            handController.FirstCoffee();
                        }
                        
                    }
                    
                }

            }

            // Falling
            if (other.gameObject.CompareTag("Ground"))
            {
                if (!onGround)
                {
                    StopFall();
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.Instance.isGameStarted)
        {
            if (other.gameObject.CompareTag("Fire"))
            {
                HeatCoffee();
            }
        }
    }

    void StopFall()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb.velocity.y < 0)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            onGround = true;
        }
        
    }

    void CheckBoundry() // If we exit from road...
    {
        boundry = FindObjectOfType<MoveForward>().boundry;
        if (transform.position.x > boundry)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb.velocity.x != 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
                transform.position = new Vector3(boundry, transform.position.y, transform.position.z);
            }
        }
        else if (transform.position.x < -boundry)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb.velocity.x != 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
                transform.position = new Vector3(-boundry, transform.position.y, transform.position.z);
            }
        }
    }

    public void CalculateScore()
    {
        FindObjectOfType<ScoreManager>().AddScore(coffeeAmount, isCoffeeHeat);
    }


}
