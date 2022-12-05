using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrambolineController : MonoBehaviour
{
    public GameObject target;

    private void Update()
    {
        if (target != null)
        {
            // Tramboline follows coffee's y and z position.
            //transform.position = new Vector3(transform.position.x, target.transform.position.y, target.transform.position.z);

            // When coffee starts falling in air , tramboline will be destroy.
            Rigidbody targetRb = target.GetComponent<Rigidbody>();

            if (targetRb.velocity.y < 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("There is no coffee for tramboline !!!");
        }
    }

}
