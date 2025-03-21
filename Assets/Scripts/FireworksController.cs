using UnityEngine;

public class FireworksController : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireballSystem;
    [SerializeField] private ParticleSystem trailSystem;
    [SerializeField] private ParticleSystem sparksSystem;
    
    private Vector3 lastPosition;
    private bool isMoving = false;
    
    private void Start()
    {
        if (fireballSystem == null)
        {
            Debug.LogError("Fireball particle system not assigned!");
            return;
        }
        
        // Initialize the last position
        lastPosition = fireballSystem.transform.position;
        
        // Make sure trail and sparks are stopped initially
        if (trailSystem != null)
            trailSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        
        if (sparksSystem != null)
            sparksSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
    
    private void Update()
    {
        if (fireballSystem == null) return;
        
        // Check if the fireball has moved
        Vector3 currentPosition = fireballSystem.transform.position;
        bool isCurrentlyMoving = (currentPosition != lastPosition);
        
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
