using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustamorHandController : MonoBehaviour
{
    [SerializeField] bool isHandOnLeftSide;
    [SerializeField] bool isHandOnRightSide;

    [SerializeField] float speed;
    private Animator anim;

    [SerializeField] GameObject endPoint;
    [SerializeField] GameObject startPoint;

    private Vector3 endPointPos;
    private Vector3 startPointPos;

    private bool goForward;
    private bool goBack;

    public bool takedCoffee; // When takes a coffee for end this script.
    private bool grabbedCoffee; // When grabbed coffee.
    public bool lastCoffee;
    public GameObject targetCoffee;
    [SerializeField] GameObject coffeePos;
    void Start()
    {
        goForward = true;
        goBack = false;

        endPointPos = endPoint.transform.position;
        startPointPos = startPoint.transform.position;
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!grabbedCoffee)
        {
            Movement();
        }
        else
        {
            Vector3 dir = (transform.position - coffeePos.transform.position).normalized;
            transform.position += dir * Time.deltaTime;
        }

    }

    private void Movement()
    {
        if (!takedCoffee)
        {
            if (isHandOnRightSide)
            {
                if (goForward)
                {
                    if (transform.position.x <= endPointPos.x)
                    {
                        goForward = false;
                        goBack = true;
                    }
                    else
                    {
                        transform.position -= Vector3.right * speed * Time.deltaTime;
                    }
                }
                else if (goBack)
                {
                    if (transform.position.x >= startPointPos.x)
                    {
                        goForward = true;
                        goBack = false;
                    }
                    else
                    {
                        transform.position += Vector3.right * speed * Time.deltaTime;
                    }
                }
            }
            else if (isHandOnLeftSide)
            {
                if (goForward)
                {
                    if (transform.position.x >= endPointPos.x)
                    {
                        goForward = false;
                        goBack = true;
                    }
                    else
                    {
                        transform.position += Vector3.right * speed * Time.deltaTime;
                    }
                }
                else if (goBack)
                {
                    if (transform.position.x <= startPointPos.x)
                    {
                        goForward = true;
                        goBack = false;
                    }
                    else
                    {
                        transform.position -= Vector3.right * speed * Time.deltaTime;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Pick a bool , is free hand on left/right side from road ?");
            }
        }


    }

    public void Grabbed() // Calculate score here.
    {
        grabbedCoffee = true;
        targetCoffee.transform.parent = coffeePos.transform;
        targetCoffee.GetComponent<CoffeeController>().CalculateScore();
        FindObjectOfType<MoveForward>().canMove = true;
        if (lastCoffee)
        {
            FindObjectOfType<UIManager>().DisplayScore();
            GameManager.Instance.isLevelFinished = true;
        }
        
    }



    public IEnumerator MoveCoffeeToHoldPosition()
    {
        FindObjectOfType<MoveForward>().canMove = false;
        while (true)
        {
            yield return null;
            if (targetCoffee != null)
            {
                float distance = Vector3.Distance(coffeePos.transform.position, targetCoffee.transform.position);
                targetCoffee.transform.position = Vector3.Lerp(targetCoffee.transform.position, coffeePos.transform.position, 10f * Time.deltaTime);
                if (distance <= 0.01f)
                {
                    anim.SetTrigger("Grab");
                    yield break;
                }
            }
                
        }
    }


}
