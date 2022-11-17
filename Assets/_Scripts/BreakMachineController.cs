using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakMachineController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Coffee"))
        {
            //BreakCoffees();
            Destroy(gameObject);
        }
        
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
}
