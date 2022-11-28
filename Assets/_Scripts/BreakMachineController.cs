using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakMachineController : MonoBehaviour
{
    [SerializeField] float xForce;

    [SerializeField] GameObject trambolinePrefab;

    private bool collided = false;

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

            for (int i = 0; i < coffeeAmount; i++)
            {
                GameObject trambolineClone = Instantiate(trambolinePrefab, hand.CoffeeList[i].transform.position, Quaternion.identity);
                TrambolineController trambolineController = trambolineClone.GetComponent<TrambolineController>();
                if (trambolineController != null)
                {
                    trambolineController.target = hand.CoffeeList[i].gameObject;
                }
                Destroy(trambolineClone, 0.3f);
            }

            BreakCoffees();
        }

        yield break;
        
    }

    /*void BreakCoffees()
    {
        HandController handController = FindObjectOfType<HandController>();
        if (handController != null)
        {
            while (handController.CoffeeList.Count > 1)
            {
                for (int i = 1; i < handController.CoffeeList.Count; i++) // i = 1 because of we dont want to break first coffee.
                {
                    BonusCoffeeController bonusCoffeeController = handController.CoffeeList[i].gameObject.GetComponent<BonusCoffeeController>();
                    if (bonusCoffeeController != null)
                    {
                        bonusCoffeeController.StopFollow();
                        handController.CoffeeList.Remove(bonusCoffeeController.gameObject);
                        break;
                    }
                }
            }
            CoffeeController coffeeController = FindObjectOfType<CoffeeController>();
            coffeeController.SetLastCoffee();
            print("Exit from while loop");
        }
    }*/
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
            print("Exit from while loop");
        }
    }

    void ForceCoffee(Rigidbody rb , float multiplier)
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        float randomForceX = Random.Range(-xForce, xForce);
        rb.AddForce((Vector3.forward * 6 * multiplier + Vector3.up * 5 + new Vector3(randomForceX,0,0)) , ForceMode.Impulse);
    }
}
