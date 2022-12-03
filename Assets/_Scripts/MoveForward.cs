using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public bool canMove;

    public bool canMoveOnlyForward;

    public float speed;

    HandController hand;

    [SerializeField] GameObject ground;
    [HideInInspector] public float boundry; // Grounds boundries.
    [SerializeField] float boundryOffset;
    private float sensitive; // Distance from camera.
    [SerializeField] [Range(1, 4)] float sensitiveMultiplier;

    private void Awake()
    {
        boundry = ground.GetComponent<BoxCollider>().bounds.extents.x;

        sensitive = transform.position.z - Camera.main.transform.position.z;

        canMove = true;

        canMoveOnlyForward = false;

        hand = FindObjectOfType<HandController>();
    }
    void Update()
    {
        if (GameManager.Instance.isGameStarted && !GameManager.Instance.isLevelFinished)
        {
            if (canMove)
            {
                if (!canMoveOnlyForward)
                {
                    Movement();
                }
                else
                {
                    transform.position += Vector3.forward * Time.deltaTime * speed;
                }
            }
            CoffeesMovement();
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

    void CoffeesMovement()
    {
        foreach (GameObject Coffee in hand.CoffeeList)
        {
            CoffeeController coffeeController = Coffee.GetComponent<CoffeeController>();
            if (coffeeController != null)
            {
                if (coffeeController.onGround)
                {
                    if (coffeeController.firstCoffee)
                    {
                        if (coffeeController.gameObject.tag == "Coffee")
                        {
                            coffeeController.FollowPlayer();
                        }


                    }
                    else
                    {
                        if (coffeeController.gameObject.tag == "Coffee")
                        {
                            coffeeController.FollowPreviousCoffee();
                        }
                    }
                }
            }

        }
    }
}
