using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CartZone:MonoBehaviour
{
    public string playerTag;
    public int itemsCollected = 0;
    public Cloud3DUI cloudUI;
    
    [Header("Consumption Animation")]
    public float consumptionTime = 1f;
    public AnimationCurve shrinkCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public Vector3 consumptionPoint = Vector3.zero; // Point where items get consumed
    
    [Header("Visual Feedback")]
    public ParticleSystem collectEffect;
    public AudioClip collectSound;
    public Transform itemDisplayParent; // Parent for showing collected items
    public int maxDisplayItems = 5; // Max items to show in cart
    public float itemSpacing = 0.5f;
    
    private AudioSource audioSource;
    private List<GameObject> displayedItems = new List<GameObject>();
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            FoodItem food=other.GetComponent<FoodItem>();
            if (food != null && food.isThrown && food.ownerTag == playerTag)
            {
                StartCoroutine(ConsumeItem(other.gameObject));
            }
        }
    }

    private IEnumerator ConsumeItem(GameObject foodObject)
    {
        // Disable physics and movement
        Rigidbody rb = foodObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;
        
        Collider col = foodObject.GetComponent<Collider>();
        if (col != null) col.enabled = false;
        
        Vector3 startPosition = foodObject.transform.position;
        Vector3 startScale = foodObject.transform.localScale;
        Vector3 targetPosition = transform.position + consumptionPoint;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < consumptionTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / consumptionTime;
            
            // Move towards consumption point
            foodObject.transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
            
            // Shrink the item
            float scaleMultiplier = shrinkCurve.Evaluate(progress);
            foodObject.transform.localScale = startScale * scaleMultiplier;
            
            yield return null;
        }
        
        // Item is fully consumed
        itemsCollected++;
        cloudUI.UpdateItemModel(GetRandomNewItem()); // next target
        
        // Play collection effects
        PlayCollectionEffects();
        
        // Update cart display
        UpdateCartDisplay();
        
        Debug.Log($"{playerTag} scored! Total: {itemsCollected}");
        
        // Destroy the consumed item
        Destroy(foodObject);
    }

    private GameObject GetRandomNewItem()
    {
        // You can later replace this with a real list of models or prefabs
        int randomIndex = Random.Range(0, cloudUI.availableItems.Count);
        return cloudUI.availableItems[randomIndex];
    }
    
    private void PlayCollectionEffects()
    {
        // Play particle effect
        if (collectEffect != null)
            collectEffect.Play();
        
        // Play sound
        if (collectSound != null && audioSource != null)
            audioSource.PlayOneShot(collectSound);
    }
    
    private void UpdateCartDisplay()
    {
        if (itemDisplayParent == null) return;
        
        // Remove old displayed items if we have too many
        while (displayedItems.Count >= maxDisplayItems)
        {
            GameObject oldItem = displayedItems[0];
            displayedItems.RemoveAt(0);
            if (oldItem != null)
                Destroy(oldItem);
        }
        
        // Add new item to display
        GameObject newItem = GetRandomNewItem();
        if (newItem != null)
        {
            Vector3 displayPosition = itemDisplayParent.position + Vector3.up * (displayedItems.Count * itemSpacing);
            GameObject displayItem = Instantiate(newItem, displayPosition, Quaternion.identity, itemDisplayParent);
            displayItem.transform.localScale *= 0.5f; // Make display items smaller
            
            // Add a simple floating animation
            StartCoroutine(FloatItem(displayItem));
            
            displayedItems.Add(displayItem);
        }
    }
    
    private System.Collections.IEnumerator FloatItem(GameObject item)
    {
        Vector3 startPos = item.transform.localPosition;
        float time = 0f;
        
        while (item != null)
        {
            time += Time.deltaTime;
            item.transform.localPosition = startPos + Vector3.up * Mathf.Sin(time * 2f) * 0.1f;
            yield return null;
        }
    }
}
