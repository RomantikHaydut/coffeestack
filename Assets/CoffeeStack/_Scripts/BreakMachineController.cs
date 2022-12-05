using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakMachineController : MonoBehaviour
{
    [SerializeField] float xForce;

    // Scale values.

    [SerializeField] float growSpeed;

    private bool goingBigger;

    private bool goingSmaller;

    private float startSize;

    private float goalSize;

    private bool collided = false;


    private void Start()
    {
        startSize = transform.localScale.x;
        goalSize = startSize * 2.5f;

        goingBigger = true;
        goingSmaller = false;

        StartCoroutine(ChangeSize_Coroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!collided)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Coffee"))
            {
                StartCoroutine(JumpCoffees());
                collided = true;
            }
        }
    }

    IEnumerator JumpCoffees()
    {
        HandController hand = FindObjectOfType<HandController>();
        int coffeeAmount;
        if (hand != null)
        {
            coffeeAmount = hand.CoffeeList.Count;
            BreakCoffees();
        }

        yield break;
        
    }

    void BreakCoffees()
    {
        HandController handController = FindObjectOfType<HandController>();
        if (handController != null)
        {
            float forceMultiplier = 1;
            while (handController.CoffeeList.Count > 0)
            {
                for (int i = 0; i < handController.CoffeeList.Count; i++) // i = 1 because of we dont want to break first coffee.
                {
                    CoffeeController coffee = handController.CoffeeList[i].gameObject.GetComponent<CoffeeController>();
                    if (coffee != null)
                    {
                        coffee.StopFollow();
                        coffee.onGround = false;
                        coffee.firstCoffee = false;
                        handController.CoffeeList.Remove(coffee.gameObject);
                        ForceCoffee(coffee.GetComponent<Rigidbody>() , forceMultiplier);
                        forceMultiplier += 0.3f;
                    }
                }
            }
            handController.firstCoffee = null;
            handController.lastCoffee = null;
        }
    }

    void ForceCoffee(Rigidbody rb , float multiplier)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        float randomForceX = Random.Range(-xForce, xForce);
        rb.AddForce((Vector3.forward * 6 * multiplier + Vector3.up * 5 + new Vector3(randomForceX,0,0)) , ForceMode.Impulse);
    }

    IEnumerator ChangeSize_Coroutine()
    {
        while (true)
        {
            yield return null;

            if (goingBigger && !goingSmaller)
            {
                if (transform.localScale.x > goalSize)
                {
                    goingSmaller = true;
                    goingBigger = false;
                }
                transform.localScale += new Vector3(transform.localScale.x, 0, transform.localScale.z) * growSpeed * Time.deltaTime;
            }
            else if (goingSmaller && !goingBigger)
            {
                if (transform.localScale.x < startSize)
                {
                    goingSmaller = false;
                    goingBigger = true;
                }
                transform.localScale -= new Vector3(transform.localScale.x, 0, transform.localScale.z) * growSpeed * Time.deltaTime;
            }
        }
    }
}
