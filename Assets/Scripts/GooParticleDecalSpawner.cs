using UnityEngine;

public class GooParticleDecalSpawner : MonoBehaviour
{
    [Header("Decal Settings")]
    [SerializeField] private GameObject decalPrefab;
    [SerializeField] private float decalSize = 0.5f;
    [SerializeField] private float minSize = 0.3f;
    [SerializeField] private float maxSize = 0.8f;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] private int maxDecals = 100;
    
    [Header("Offset Settings")]
    [SerializeField] private float surfaceOffset = 0.01f;
    
    private ParticleSystem particleSystem;
    private ParticleCollisionEvent[] collisionEvents;
    private int decalCount = 0;
    private GameObject[] decalPool;
    private int currentDecalIndex = 0;
    
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        
        if (particleSystem == null)
        {
            Debug.LogError("No Particle System found on this GameObject!");
            enabled = false;
            return;
        }
        
        if (decalPrefab == null)
        {
            Debug.LogError("Decal Prefab is not assigned!");
            enabled = false;
            return;
        }
        
        // Initialize collision events array
        collisionEvents = new ParticleCollisionEvent[16];
        
        // Initialize decal pool
        InitializeDecalPool();
    }
    
    private void InitializeDecalPool()
    {
        decalPool = new GameObject[maxDecals];
        
        for (int i = 0; i < maxDecals; i++)
        {
            GameObject decal = Instantiate(decalPrefab);
            decal.SetActive(false);
            decalPool[i] = decal;
        }
    }
    
    private void OnParticleCollision(GameObject other)
    {
        // Get collision events
        int collisionCount = particleSystem.GetCollisionEvents(other, collisionEvents);
        
        for (int i = 0; i < collisionCount; i++)
        {
            // Get a decal from the pool
            GameObject decal = decalPool[currentDecalIndex];
            currentDecalIndex = (currentDecalIndex + 1) % maxDecals;
            
            // Position and orient the decal
            Vector3 collisionPoint = collisionEvents[i].intersection;
            Vector3 collisionNormal = collisionEvents[i].normal;
            
            // Position slightly above the surface to prevent z-fighting
            collisionPoint += collisionNormal * surfaceOffset;
            
            // Set position and rotation
            decal.transform.position = collisionPoint;
            decal.transform.rotation = Quaternion.FromToRotation(Vector3.up, collisionNormal);
            
            // Randomize size
            float randomSize = Random.Range(minSize, maxSize) * decalSize;
            decal.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
            
            // Activate the decal
            decal.SetActive(true);
            
            // Schedule deactivation
            StartCoroutine(DeactivateAfterDelay(decal, lifeTime));
        }
    }
    
    private System.Collections.IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
