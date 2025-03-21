using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we hit an enemy or asteroid
        if (other.CompareTag("Enemy") || other.CompareTag("Asteroid"))
        {
            // Apply damage or destroy the object
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            
            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}