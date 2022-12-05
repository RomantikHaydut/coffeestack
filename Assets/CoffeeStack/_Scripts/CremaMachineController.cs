using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CremaMachineController : MonoBehaviour
{
    Animator anim;

    public float xOffset;

    private bool collided = false;

    private GameObject arm;

    [SerializeField] GameObject stopPoint;

    private List<GameObject> CoffeesAndHand = new List<GameObject>();

    public GameObject targetCoffee;
    void Awake()
    {
        anim = GetComponent<Animator>();
        arm = transform.GetChild(0).gameObject;
    }

    private void StopMovement()
    {
        FindObjectOfType<MoveForward>().canMove = false;
        FindObjectOfType<MoveForward>().canMoveOnlyForward = true;
    }

    private void ContinueMovement()
    {
        FindObjectOfType<MoveForward>().canMove = true;
    }

    private void MakeLine()
    {
        CoffeesAndHand.Add(FindObjectOfType<MoveForward>().gameObject); // Hand..
        int coffeeCount = FindObjectOfType<HandController>().CoffeeList.Count;
        for (int i = 0; i < coffeeCount; i++) // Coffees in hand...
        {
            CoffeesAndHand.Add(FindObjectOfType<HandController>().CoffeeList[i].gameObject);
        }

        for (int i = 0; i < CoffeesAndHand.Count; i++)
        {
            CoffeesAndHand[i].gameObject.transform.position = new Vector3(arm.transform.position.x - xOffset, CoffeesAndHand[i].gameObject.transform.position.y, CoffeesAndHand[i].gameObject.transform.position.z);
        }
    }

    private void SetTargetCoffee()
    {
        HandController hand = FindObjectOfType<HandController>();
        int coffeeCount = hand.CoffeeList.Count;
        targetCoffee = hand.CoffeeList[0].gameObject;
        foreach (GameObject Coffee in hand.CoffeeList)
        {
            if (!Coffee.GetComponent<CoffeeController>().isCoffeeCreamed)
            {
                float thisDistance = Vector3.Distance(arm.transform.position, Coffee.transform.position);
                if (Coffee.transform.position.z < arm.transform.position.z) // If coffee hasnt pass the machine...
                {
                    if (thisDistance < Vector3.Distance(arm.transform.position, targetCoffee.transform.position))
                    {
                        targetCoffee = Coffee.gameObject;
                    }
                }
            }
        }

        
    }

    public void MoveCoffee()
    {
        StartCoroutine(MoveCoffee_Coroutine());
    }
    IEnumerator MoveCoffee_Coroutine()
    {
        ContinueMovement();
        while (true)
        {
            float distance = Mathf.Abs(arm.transform.position.z - targetCoffee.transform.position.z);
            if (targetCoffee.transform.position.z > stopPoint.transform.position.z)
            {
                // Start Animator here...
                anim.SetTrigger("Crema");
                StopMovement();
                float offset = targetCoffee.transform.position.z - stopPoint.transform.position.z;
                for (int i = 0; i < CoffeesAndHand.Count; i++)
                {
                    CoffeesAndHand[i].gameObject.transform.position = new Vector3(CoffeesAndHand[i].gameObject.transform.position.x, CoffeesAndHand[i].gameObject.transform.position.y, CoffeesAndHand[i].gameObject.transform.position.z - offset);
                }
                yield break;
            }

            yield return null;

        }
    }



    // Animation Events....

    private void MakeCream()
    {
        CoffeeController coffeeController = targetCoffee.GetComponent<CoffeeController>();
        if (coffeeController != null)
        {
            coffeeController.MakeCream();
        }
    }
    public void FinishCrema() // Calling in GoUp animation.
    {
        anim.ResetTrigger("Crema");
        SetTargetCoffee();
        MoveCoffee();
    }

    public void CheckFinish()
    {
        if (targetCoffee == FindObjectOfType<HandController>().CoffeeList[0].gameObject)// End.
        {
            FindObjectOfType<MoveForward>().canMove = true;
            FindObjectOfType<MoveForward>().canMoveOnlyForward = false;
            anim.enabled = false;
            this.enabled = false;
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coffee"))
        {
            CoffeeController coffee = other.gameObject.GetComponent<CoffeeController>();
            if (coffee.following || coffee.firstCoffee)
            {
                if (!collided)
                {
                    StopMovement();
                    SetTargetCoffee();
                    MakeLine();
                    MoveCoffee();
                    collided = true;
                }
            }
            
        }
    }

}
