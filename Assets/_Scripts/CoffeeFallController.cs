using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeFallController : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Coffee"))
        {
            CoffeeController coffeeController = other.gameObject.GetComponent<CoffeeController>();
            if (coffeeController != null)
            {
                coffeeController.coffeeAmount = 1;
                coffeeController.FillCoffee();
            }
        }
    }
}
