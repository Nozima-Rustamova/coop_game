using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public string horizontalAxis = "HorizontalP1";
    public string verticalAxis = "VerticalP1";
    public float moveSpeed = 5f;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlayerMovement requires a Rigidbody component!");
        }
        rb.freezeRotation = true;

    }

    void Update()
    {
        float h = Input.GetAxis(horizontalAxis);
        float v = Input.GetAxis(verticalAxis);

        Vector3 movement = new Vector3(h, 0f, v) * moveSpeed;
        
        // Use Rigidbody for movement instead of transform.Translate
        if (rb != null)
        {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
    }
}
