using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float thrustForce = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float drag = 0.5f;

    [Header("Weapon Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 0.2f;
    
    [Header("Engine Visuals")]
    [SerializeField] private GameObject engineLeft;
    [SerializeField] private GameObject engineRight;

    private Rigidbody2D rb;
    private float nextFireTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // If there's no Rigidbody2D, add one
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.linearDamping = drag;
            rb.angularDamping = 0.5f;
        }
        
        // Ensure engine sprites are initially disabled
        if (engineLeft != null) engineLeft.SetActive(false);
        if (engineRight != null) engineRight.SetActive(false);
        
        // Check for multiple Audio Listeners
        EnsureSingleAudioListener();
    }
    
    private void EnsureSingleAudioListener()
    {
        // Find all Audio Listeners in the scene using the new method
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);

        if (listeners.Length > 1)
        {
            Debug.LogWarning("Multiple Audio Listeners detected in the scene. Disabling extras.");

            // Keep the first one (or the one on this GameObject if it exists)
            bool foundActiveListener = false;

            foreach (AudioListener listener in listeners)
            {
                // If this is the first listener we're processing or it's on the player ship
                if (!foundActiveListener || listener.gameObject == this.gameObject)
                {
                    listener.enabled = true;
                    foundActiveListener = true;
                }
                else
                {
                    // Disable any additional listeners
                    listener.enabled = false;
                }
            }
        }
        else if (listeners.Length == 0)
        {
            Debug.LogWarning("No Audio Listener found in the scene. Adding one to the player.");
            gameObject.AddComponent<AudioListener>();
        }
    }

    private void Update()
    {
        // Handle rotation
        float rotationInput = 0f;
        bool isPressingA = Input.GetKey(KeyCode.A);
        bool isPressingD = Input.GetKey(KeyCode.D);
        bool isPressingW = Input.GetKey(KeyCode.W);
        
        if (isPressingA)
            rotationInput += 1f;
        if (isPressingD)
            rotationInput -= 1f;
        
        // Apply rotation
        transform.Rotate(Vector3.forward * rotationInput * rotationSpeed * Time.deltaTime);

        // Handle weapon firing
        if (Input.GetKey(KeyCode.J) && Time.time >= nextFireTime)
        {
            FireWeapon();
            nextFireTime = Time.time + fireRate;
        }
        
        // Handle engine visuals
        UpdateEngineVisuals(isPressingW, isPressingA, isPressingD);
    }

    private void UpdateEngineVisuals(bool forward, bool left, bool right)
    {
        // Left engine: visible when turning right (D) or moving forward (W)
        if (engineLeft != null)
            engineLeft.SetActive(right || forward);
            
        // Right engine: visible when turning left (A) or moving forward (W)
        if (engineRight != null)
            engineRight.SetActive(left || forward);
    }

    private void FixedUpdate()
    {
        // Handle thrust
        float thrustInput = 0f;
        if (Input.GetKey(KeyCode.W))
            thrustInput += 1f;
        if (Input.GetKey(KeyCode.S))
            thrustInput -= 1f;

        // Apply thrust force in the forward direction of the ship
        if (thrustInput != 0f)
        {
            Vector2 thrustForceVector = transform.up * thrustInput * thrustForce;
            rb.AddForce(thrustForceVector);
        }

        // Limit maximum speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void FireWeapon()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Bullet prefab or fire point not assigned!");
            return;
        }

        // Instantiate bullet at the fire point position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Add velocity to the bullet
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            // Set the Rigidbody2D body type to Dynamic
            bulletRb.bodyType = RigidbodyType2D.Dynamic;

            // Add velocity in the forward direction of the bullet
            bulletRb.linearVelocity = transform.up * bulletSpeed;

            // Alternatively, you can use AddForce for more control
            // bulletRb.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
        }
        else
        {
            // If there's no Rigidbody2D, add one
            bulletRb = bullet.AddComponent<Rigidbody2D>();
            bulletRb.gravityScale = 0f;
            bulletRb.bodyType = RigidbodyType2D.Dynamic;
            bulletRb.linearVelocity = transform.up * bulletSpeed;
            Debug.Log("Added Rigidbody2D to bullet");
        }

        // Destroy the bullet after 2 seconds to prevent cluttering the scene
        Destroy(bullet, 2f);
    }
}
