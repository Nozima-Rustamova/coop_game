using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public string ownerTag;
    public bool isThrown = false;
    public float hitForce = 5f;
    public float lifetime = 10f; // Food disappears after this time
    
    private float throwTime;
    private bool hasHitPlayer = false;
    
    void Start()
    {
        throwTime = Time.time;
    }
    
    void Update()
    {
        // Destroy food after lifetime
        if (Time.time - throwTime > lifetime)
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Check if food hits a player (but not the one who threw it)
        if (isThrown && (collision.gameObject.CompareTag("Player_1") || collision.gameObject.CompareTag("Player_2")))
        {
            if (collision.gameObject.tag != ownerTag && !hasHitPlayer)
            {
                hasHitPlayer = true;
                
                // Get the player interaction component
                PlayerInteraction playerInteraction = collision.gameObject.GetComponent<PlayerInteraction>();
                if (playerInteraction != null)
                {
                    // Calculate hit direction
                    Vector3 hitDirection = (collision.transform.position - transform.position).normalized;
                    playerInteraction.GetHitByFood(hitDirection, hitForce);
                    
                    Debug.Log($"Food hit {collision.gameObject.tag}!");
                }
                
                // Destroy the food after hitting a player
                Destroy(gameObject);
            }
        }
    }
}