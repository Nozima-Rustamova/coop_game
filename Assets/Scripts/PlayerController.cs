using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Pickup and Throw")]
    public float pickupRange = 2f; // Distance for raycast pickup
    public float throwForce = 10f; // Force for throwing
    public Vector3 holdOffset = new Vector3(0f, 0.5f, 1f); // Relative hold position (adjust as needed)
    public LayerMask pickupLayer; // Set to layer for pickups in Inspector

    [Header("Input Keys (Configure per player in Inspector)")]
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode throwKey = KeyCode.Q;

    private GameObject heldObject;
    private Rigidbody heldRigidbody;

    // References to player-specific scripts
    private Player_1 player1Script;
    private Player_2 player2Script;

    void Start()
    {
        // Get components from player-specific scripts if attached
        player1Script = GetComponent<Player_1>();
        player2Script = GetComponent<Player_2>();

        if (player1Script != null)
        {
            // Example: Pull data from Player_1 script (e.g., override throwForce or other custom values)
            // throwForce = player1Script.customThrowForce; // Uncomment and add public float customThrowForce to Player_1 if needed
            // Add more logic here, e.g., call methods or pull other variables from Player_1
        }
        else if (player2Script != null)
        {
            // Example: Pull from Player_2
            // throwForce = player2Script.customThrowForce; // Uncomment and add to Player_2 if needed
            // Add more logic here
        }
    }

    void Update()
    {
        // Note: Movement and rotation are handled in the player-specific scripts (Player_1 or Player_2)

        // Pickup logic
        if (Input.GetKeyDown(pickupKey))
        {
            if (heldObject == null)
            {
                TryPickup();
            }
        }

        // Throw or drop logic
        if (Input.GetKeyDown(throwKey))
        {
            if (heldObject != null)
            {
                ThrowObject();
            }
        }

        // Update held object position if holding
        if (heldObject != null)
        {
            heldObject.transform.position = transform.position + transform.TransformDirection(holdOffset);
            heldObject.transform.rotation = transform.rotation; // Optional: match player rotation
        }
    }

    private void TryPickup()
    {
        // Raycast forward to detect pickup
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange, pickupLayer))
        {
            if (hit.collider.CompareTag("Object"))
            {
                heldObject = hit.collider.gameObject;
                heldRigidbody = heldObject.GetComponent<Rigidbody>();

                if (heldRigidbody != null)
                {
                    heldRigidbody.isKinematic = true;
                    heldRigidbody.useGravity = false;
                }

                // Parent to player
                heldObject.transform.SetParent(transform);
            }
        }
    }

    private void ThrowObject()
    {
        if (heldObject != null)
        {
            // Unparent and re-enable physics
            heldObject.transform.SetParent(null);

            if (heldRigidbody != null)
            {
                heldRigidbody.isKinematic = false;
                heldRigidbody.useGravity = true;
                heldRigidbody.velocity = Vector3.zero; // Reset velocity
                heldRigidbody.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            }

            heldObject = null;
            heldRigidbody = null;
        }
    }
}