using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private bool canPickup = false;
    public Transform playerLocation;
    private GameObject currentItem;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canPickup = true;
            currentItem = this.gameObject;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            {
            canPickup = false;
            currentItem = null;
        }


    }

    private void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            if (currentItem != null)
            {
                currentItem.transform.SetParent(playerLocation);
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentItem != null && currentItem.transform.parent == playerLocation)
            {
                currentItem.transform.SetParent(null);
                currentItem.GetComponent<Rigidbody>().isKinematic = false;
            }

        }
    }
}
