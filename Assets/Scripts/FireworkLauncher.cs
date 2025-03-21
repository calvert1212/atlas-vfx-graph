using UnityEngine;
using UnityEngine.VFX; // Make sure this is included

public class FireworkLauncher : MonoBehaviour
{
    // Change this line if it's not already using VisualEffect
    public VisualEffect fireworkVFXPrefab; // Changed from fireworkVFX to fireworkVFXPrefab for clarity
    public KeyCode launchKey = KeyCode.Space;
    public float minLaunchForce = 5f;
    public float maxLaunchForce = 15f;
    public Vector2 randomPositionRange = new Vector2(10f, 10f);
    
    void Update()
    {
        if (Input.GetKeyDown(launchKey))
        {
            LaunchFirework();
        }
    }
    
    void LaunchFirework()
    {
        // Create instance at random position
        Vector3 position = transform.position + new Vector3(
            Random.Range(-randomPositionRange.x, randomPositionRange.x),
            0,
            Random.Range(-randomPositionRange.y, randomPositionRange.y)
        );
        
        // Instantiate the Visual Effect prefab
        VisualEffect instance = Instantiate(fireworkVFXPrefab, position, Quaternion.identity);
        
        // Set random properties
        float launchForce = Random.Range(minLaunchForce, maxLaunchForce);
        Color color = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
        
        instance.SetFloat("LaunchForce", launchForce);
        instance.SetVector4("FireworkColor", color);
    }
}
