using Unity.VisualScripting;
using UnityEngine;

public class CartZone:MonoBehaviour
{
    public string playerTag;
    public int itemsCollected = 0;
    public Cloud3DUI cloudUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            FoodItem food=other.GetComponent<FoodItem>();
            if (food != null && food.isThrown && food.ownerTag == playerTag)
            {
                itemsCollected++;
                cloudUI.UpdateItemModel(GetRandomNewItem()); // next target

                Destroy(other.gameObject); // remove food

                Debug.Log($"{playerTag} scored! Total: {itemsCollected}");

                
            }
        }
    }


    private GameObject GetRandomNewItem()
    {
        // You can later replace this with a real list of models or prefabs
        int randomIndex = Random.Range(0, cloudUI.availableItems.Count);
        return cloudUI.availableItems[randomIndex];
    }

}
