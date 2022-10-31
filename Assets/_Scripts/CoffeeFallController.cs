using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeFallController : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Coffee"))
        {
            BonusCoffeeController bonusCoffeeController = other.gameObject.GetComponent<BonusCoffeeController>();
            if (bonusCoffeeController != null)
            {
                bonusCoffeeController.coffeeAmount = 1;
                bonusCoffeeController.FillCoffee();
            }
        }
    }
}
