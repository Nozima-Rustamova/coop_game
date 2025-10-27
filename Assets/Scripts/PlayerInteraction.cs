using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Slap Settings")]
    public float slapRange = 2f;
    public float slapForce = 10f;
    public float slapCooldown = 1f;
    public KeyCode slapKey = KeyCode.Space;
    
    [Header("Knockback Settings")]
    public float knockbackForce = 8f;
    public float knockbackDuration = 0.5f;
    
    [Header("Audio")]
    public AudioClip slapSound;
    public AudioClip hitSound;
    
    private float lastSlapTime;
    private bool isKnockedBack = false;
    private Vector3 knockbackDirection;
    private float knockbackTimer;
    private AudioSource audioSource;
    private Rigidbody rb;
    private PlayerMovement playerMovement;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    void Update()
    {
        // Handle slap input
        if (Input.GetKeyDown(slapKey) && Time.time - lastSlapTime > slapCooldown)
        {
            TrySlap();
        }
        
        // Handle knockback
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
                if (playerMovement != null)
                    playerMovement.enabled = true;
            }
        }
    }
    
    void FixedUpdate()
    {
        // Apply knockback force
        if (isKnockedBack)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Force);
        }
    }
    
    void TrySlap()
    {
        Collider[] nearbyPlayers = Physics.OverlapSphere(transform.position, slapRange);
        
        foreach (Collider col in nearbyPlayers)
        {
            if (col.CompareTag("Player_1") || col.CompareTag("Player_2"))
            {
                if (col.gameObject != gameObject) // Don't slap yourself
                {
                    PlayerInteraction otherPlayer = col.GetComponent<PlayerInteraction>();
                    if (otherPlayer != null)
                    {
                        // Apply knockback to the slapped player
                        Vector3 slapDirection = (col.transform.position - transform.position).normalized;
                        otherPlayer.GetSlapped(slapDirection);
                        
                        // Play slap sound
                        if (slapSound != null)
                            audioSource.PlayOneShot(slapSound);
                        
                        lastSlapTime = Time.time;
                        Debug.Log($"{gameObject.tag} slapped {col.tag}!");
                        break; // Only slap one player at a time
                    }
                }
            }
        }
    }
    
    public void GetSlapped(Vector3 direction)
    {
        if (isKnockedBack) return; // Prevent multiple knockbacks
        
        isKnockedBack = true;
        knockbackDirection = direction;
        knockbackTimer = knockbackDuration;
        
        // Disable movement during knockback
        if (playerMovement != null)
            playerMovement.enabled = false;
        
        // Play hit sound
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }
    
    public void GetHitByFood(Vector3 hitDirection, float force = 5f)
    {
        if (isKnockedBack) return; // Prevent multiple knockbacks
        
        isKnockedBack = true;
        knockbackDirection = hitDirection;
        knockbackTimer = knockbackDuration * 0.7f; // Shorter knockback for food hits
        
        // Disable movement during knockback
        if (playerMovement != null)
            playerMovement.enabled = false;
        
        // Apply additional force from the food
        rb.AddForce(hitDirection * force, ForceMode.Impulse);
        
        // Play hit sound
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw slap range in scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slapRange);
    }
}
