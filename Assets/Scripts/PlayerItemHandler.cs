using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    public float pickupRange = 2f;
    public float throwForce = 12f;
    public Vector3 holdOffset = new Vector3(0, 1, 1);
    public LayerMask pickupLayer;

    public KeyCode pickupKey = KeyCode.E;
    public KeyCode throwKey = KeyCode.Q;

    private GameObject heldObject;
    private Rigidbody heldRb;

    void Update()
    {
        if (Input.GetKeyDown(pickupKey) && heldObject == null)
            TryPickup();

        if (Input.GetKeyDown(throwKey) && heldObject != null)
            ThrowObject();

        if (heldObject != null)
{
        Vector3 targetPosition = transform.position + transform.forward + holdOffset;
        heldObject.transform.position = Vector3.Lerp(heldObject.transform.position, targetPosition, Time.deltaTime * 10f);
        heldObject.transform.rotation = transform.rotation;
}
    }

    void TryPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange, pickupLayer))
        {
            heldObject = hit.collider.gameObject;
            heldRb = heldObject.GetComponent<Rigidbody>();

            heldRb.isKinematic = true;
            heldObject.transform.SetParent(transform);
        }
    }

    void ThrowObject()
    {
        heldObject.transform.SetParent(null);
        heldRb.isKinematic = false;
        heldRb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);

        // IMPORTANT: Mark as thrown for scoring
        FoodItem fi = heldObject.GetComponent<FoodItem>();
        if (fi != null)
        {
            fi.isThrown = true;
            fi.ownerTag = gameObject.tag;
        }

        heldObject = null;
        heldRb = null;
    }
}
