using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeController : MonoBehaviour
{
    GameObject player; // Hand

    HandController handController;

    public GameObject followCoffee;

    GameObject firstCoffeePos;

    // For fill with coffee
    public int coffeeAmount = 0;
    List<GameObject> coffeeGrades = new List<GameObject>();
    List<GameObject> coffeeHeats = new List<GameObject>();
    List<GameObject> coffeeCremas = new List<GameObject>();
    public bool triggerWithCoffeeMachine = false;
    public float distanceTakenWithCoffeeMachine;
    private float triggerStartPoint;
    private bool isCoffeeHeat;
    public bool isCoffeeCreamed = false;

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
    }
    void Start()
    {
        SetCoffeeGrades();
        FillCoffee();
        IgnoreCollisionWithOtherCoffees();
    }

    public void StarEventsFirstCoffee()
    {
        handController.CoffeeList.Add(gameObject);
    }


    void Update()
    {
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

    public void FollowPlayer() // For first coffee
    {
        transform.position = firstCoffeePos.transform.position;
    }

    public void FollowPreviousCoffee()
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
            else if (child.tag == "CoffeeGrade")
            {
                coffeeGrades.Add(child);
                child.gameObject.SetActive(false);
            }
            else if (child.tag == "Crema")
            {
                coffeeCremas.Add(child);
                child.gameObject.SetActive(false);
            }
        }
    }

    public void FillCoffee()
    {
        if (coffeeAmount == 1)
        {
            if (!coffeeGrades[1].gameObject.activeInHierarchy && !coffeeGrades[2].gameObject.activeInHierarchy)
            {
                coffeeGrades[0].gameObject.SetActive(true);
            }
        }
        else if (coffeeAmount == 2)
        {
            if (!coffeeGrades[2].gameObject.activeInHierarchy)
            {
                coffeeGrades[1].gameObject.SetActive(true);
            }

            if (coffeeGrades[0].gameObject.activeInHierarchy)
            {
                coffeeGrades[0].gameObject.SetActive(false);
            }
        }
        else if (coffeeAmount == 3)
        {
            coffeeGrades[2].gameObject.SetActive(true);
            if (coffeeGrades[0].gameObject.activeInHierarchy)
            {
                coffeeGrades[0].gameObject.SetActive(false);
            }

            if (coffeeGrades[1].gameObject.activeInHierarchy)
            {
                coffeeGrades[1].gameObject.SetActive(false);
            }


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

    public void MakeCream()
    {
        if (coffeeAmount == 1)
        {
            coffeeCremas[0].gameObject.SetActive(true);
            isCoffeeCreamed = true;
        }
        else if (coffeeAmount == 2)
        {
            coffeeCremas[1].gameObject.SetActive(true);
            isCoffeeCreamed = true;
        }
        else if (coffeeAmount == 3)
        {
            coffeeCremas[2].gameObject.SetActive(true);
            isCoffeeCreamed = true;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.isGameStarted)
        {
            if (transform.tag == "Coffee")
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
                            else if (other.gameObject.CompareTag("Player") && transform.tag == "Coffee")
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
                            if (other.gameObject.CompareTag("Player") && transform.tag == "Coffee")
                            {
                                firstCoffee = true;
                                handController.firstCoffee = gameObject;
                                handController.CoffeeList.Add(gameObject);
                                handController.FirstCoffee();
                            }

                        }

                        // Freehand
                        if (other.gameObject.CompareTag("FreeHand"))
                        {
                            if (following)
                            {
                                print("AAAAA");
                                FreeHandController freeHandController = other.gameObject.transform.GetComponentInParent(typeof(FreeHandController)) as FreeHandController;
                                if (!freeHandController.takedCoffee)
                                {
                                    freeHandController.targetCoffee = gameObject;
                                    StartCoroutine(freeHandController.MoveCoffeeToHoldPosition());
                                    transform.tag = "TakenCoffee";
                                    freeHandController.takedCoffee = true;
                                    RemoveFromCoffeeList();
                                    other.gameObject.tag = "Untagged";
                                }

                            }
                        }

                        // Custamor hand
                        if (other.gameObject.CompareTag("CustamorHand"))
                        {
                            if (following)
                            {
                                CustamorHandController custamorHandController = other.gameObject.transform.GetComponentInParent(typeof(CustamorHandController)) as CustamorHandController;
                                if (!custamorHandController.takedCoffee)
                                {
                                    custamorHandController.targetCoffee = gameObject;
                                    StartCoroutine(custamorHandController.MoveCoffeeToHoldPosition());
                                    transform.tag = "TakenCoffee";
                                    custamorHandController.takedCoffee = true;
                                    RemoveFromCoffeeList();
                                    other.gameObject.tag = "Untagged";
                                }

                            }
                        }

                        if (other.gameObject.CompareTag("Finish"))
                        {
                            if (lastCoffee)
                            {
                                handController.LevelFinish();
                            }
                        }

                    }
                    else // Here if coffee is first coffeee.
                    {
                        // Freehand
                        if (other.gameObject.CompareTag("FreeHand"))
                        {

                            print("AAAAA");
                            FreeHandController freeHandController = other.gameObject.transform.GetComponentInParent(typeof(FreeHandController)) as FreeHandController;
                            if (!freeHandController.takedCoffee)
                            {
                                freeHandController.targetCoffee = gameObject;
                                StartCoroutine(freeHandController.MoveCoffeeToHoldPosition());
                                transform.tag = "TakenCoffee";
                                freeHandController.takedCoffee = true;
                                RemoveFromCoffeeList();
                                other.gameObject.tag = "Untagged";
                            }

                        }

                        // Custamor hand it means custamor takes last coffee.
                        if (other.gameObject.CompareTag("CustamorHand"))
                        {
                            CustamorHandController custamorHandController = other.gameObject.transform.GetComponentInParent(typeof(CustamorHandController)) as CustamorHandController;
                            if (!custamorHandController.takedCoffee)
                            {
                                custamorHandController.targetCoffee = gameObject;
                                StartCoroutine(custamorHandController.MoveCoffeeToHoldPosition());
                                transform.tag = "TakenCoffee";
                                custamorHandController.takedCoffee = true;
                                RemoveFromCoffeeList();
                                if (firstCoffee)
                                {
                                    custamorHandController.lastCoffee = true;
                                }
                                other.gameObject.tag = "Untagged";
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

    }

    private void RemoveFromCoffeeList()
    {
        // Here we find this coffees index.
        int index = 0;
        if (handController.CoffeeList.Count > 1)
        {
            for (int i = 0; i < handController.CoffeeList.Count; i++)
            {
                if (handController.CoffeeList[i].gameObject == gameObject)
                {
                    index = i;
                }
            }
            // Here we find the this coffee's follower coffee;
            List<GameObject> ActiveFollowCoffees = new List<GameObject>();

            GameObject myFollower = handController.CoffeeList[0];
            foreach (GameObject Coffee in handController.CoffeeList)
            {
                CoffeeController coffeeController = Coffee.GetComponent<CoffeeController>();
                if (coffeeController.followCoffee == gameObject)
                {
                    myFollower = coffeeController.gameObject;
                }

            }
            if (myFollower != null)
            {
                if (!firstCoffee)
                {
                    myFollower.GetComponent<CoffeeController>().followCoffee = handController.CoffeeList[index - 1].gameObject;
                }
                else
                {
                    myFollower.GetComponent<CoffeeController>().followCoffee = null;
                    myFollower.GetComponent<CoffeeController>().firstCoffee = true;
                }
                
            }

            for (int i = handController.CoffeeList.IndexOf(myFollower) + 1; i < handController.CoffeeList.Count; i++)
            {
                handController.CoffeeList[i].gameObject.GetComponent<CoffeeController>().followCoffee = handController.CoffeeList[i - 1].gameObject;
            }

        }

        handController.CoffeeList.Remove(gameObject);

        if (handController.firstCoffee == gameObject)
        {
            handController.firstCoffee = null;
            handController.FirstCoffee();
        }

        if (handController.lastCoffee == gameObject)
        {
            handController.lastCoffee = null;
            handController.LastCoffee();
        }

        if (firstCoffee)
        {
            firstCoffee = false;
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

    public void CheckBoundry() // If we exit from road...
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

    private void IgnoreCollisionWithOtherCoffees()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        GameObject[] coffees = GameObject.FindGameObjectsWithTag("Coffee");
        for (int i = 0; i < coffees.Length; i++)
        {
            if (coffees[i].gameObject != gameObject)
            {
                BoxCollider otherCoffeesCollider = coffees[i].GetComponent<BoxCollider>();
                Physics.IgnoreCollision(collider, otherCoffeesCollider);
            }
        }

    }

    public void CalculateScore()
    {
        FindObjectOfType<ScoreManager>().AddScore(coffeeAmount, isCoffeeHeat, isCoffeeCreamed);
    }


}
