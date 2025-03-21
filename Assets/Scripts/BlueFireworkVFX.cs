using UnityEngine;

public class BlueFireworkVFX : MonoBehaviour
{
    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem fireballSystem;
    [SerializeField] private ParticleSystem trailSystem;
    [SerializeField] private ParticleSystem sparksSystem;
    
    private Vector3 lastPosition;
    private bool isMoving = false;
    
    private void Start()
    {
        // Check if the required particle systems are assigned
        if (fireballSystem == null)
        {
            Debug.LogError("Fireball particle system is not assigned!");
            return;
        }
        
        // Initialize the last position
        lastPosition = fireballSystem.transform.position;
        
        // Make sure trail and sparks are properly initialized
        if (trailSystem != null)
        {
            trailSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        else
        {
            Debug.LogWarning("Trail particle system is not assigned!");
        }
        
        if (sparksSystem != null)
        {
            sparksSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        else
        {
            Debug.LogWarning("Sparks particle system is not assigned!");
        }
    }
    
    private void Update()
    {
        if (fireballSystem == null) return;
        
        // Check if the fireball has moved
        Vector3 currentPosition = fireballSystem.transform.position;
        bool isCurrentlyMoving = (Vector3.Distance(currentPosition, lastPosition) > 0.001f);
        
        // If movement state changed
        if (isCurrentlyMoving != isMoving)
        {
            isMoving = isCurrentlyMoving;
            
            if (isMoving)
            {
                // Fireball started moving, activate trail and sparks
                if (trailSystem != null)
                    trailSystem.Play(true);
                
                if (sparksSystem != null)
                    sparksSystem.Play(true);
            }
            else
            {
                // Fireball stopped moving, deactivate trail and sparks
                if (trailSystem != null)
                    trailSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                
                if (sparksSystem != null)
                    sparksSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
        
        // Update the last position
        lastPosition = currentPosition;
    }
}
