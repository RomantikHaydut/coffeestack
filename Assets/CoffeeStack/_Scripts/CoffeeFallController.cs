using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeFallController : MonoBehaviour
{
    float size;

    GameObject coffee; // Random coffee for calculate size.
    private void Start()
    {
        coffee = GameObject.FindGameObjectWithTag("Coffee");
        size = coffee.transform.localScale.z * coffee.GetComponent<BoxCollider>().size.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coffee"))
        {
            CoffeeController coffeeController = other.gameObject.GetComponent<CoffeeController>();
            if (coffeeController != null)
            {
                coffeeController.DefineStartPoint();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Coffee"))
        {
            CoffeeController coffeeController = other.gameObject.GetComponent<CoffeeController>();
            if (coffeeController != null)
            {
                coffeeController.CalculateTakenDistanceWithCoffeeMachine();

                float takenDistance = coffeeController.distanceTakenWithCoffeeMachine;
                if (takenDistance / (size * 2) <= 0.33f)
                {
                    if (coffeeController.coffeeAmount <= 1)
                    {
                        coffeeController.coffeeAmount = 1;
                        coffeeController.FillCoffee();
                    }
                    
                }
                else if (takenDistance / (size * 2) > 0.33f && takenDistance / (size * 2) <= 0.66f)
                {
                    if (coffeeController.coffeeAmount <= 2)
                    {
                        coffeeController.coffeeAmount = 2;
                        coffeeController.FillCoffee();
                    }
                    
                }
                else
                {
                    if (coffeeController.coffeeAmount <= 3)
                    {
                        coffeeController.coffeeAmount = 3;
                        coffeeController.FillCoffee();
                    }
                    
                }

            }
        }
    }
}
