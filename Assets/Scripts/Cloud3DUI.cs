using System.Collections.Generic;
using UnityEngine;

public class Cloud3DUI : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;   // Drag your player here
    public Transform itemSpawnPoint;    // Drag ItemDisplay here
    public List<GameObject> availableItems; // assign in Inspector

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 2.5f, 0);
    public float rotationSpeed = 30f;   // for slow spinning effect

    private GameObject currentItem;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (playerTransform != null)
            transform.position = playerTransform.position + offset;

        if (mainCam != null)
            transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward);

        if (currentItem != null)
            currentItem.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    public void UpdateItemModel(GameObject newItemPrefab)
    {
        if (currentItem != null)
            Destroy(currentItem);

        currentItem = Instantiate(newItemPrefab, itemSpawnPoint.position, Quaternion.identity, itemSpawnPoint);
        currentItem.transform.localScale *= 0.3f;
    }
}
