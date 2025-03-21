using UnityEngine;
using UnityEngine.VFX;

public class PatrioticFireworkLauncher : MonoBehaviour
{
    [System.Serializable]
    public class FireworkType
    {
        public string name;
        public VisualEffect prefab;
        public Color primaryColor = Color.white;
        public Texture2D customSprite;
        public FireworkPattern pattern = FireworkPattern.Standard;
        [Range(0, 100)]
        public int spawnWeight = 33; // Relative chance of spawning
    }
    
    public enum FireworkPattern
    {
        Standard,
        Star,
        Ring,
        Heart,
        Logo
    }
    
    [Header("Firework Types")]
    public FireworkType[] fireworkTypes;
    
    [Header("Launch Settings")]
    public KeyCode manualLaunchKey = KeyCode.Space;
    public bool enableAutoLaunch = true;
    public float autoLaunchInterval = 2f;
    public int maxFireworksPerInterval = 3;
    public int minFireworksPerInterval = 1;
    public Vector2 randomPositionRange = new Vector2(20f, 20f);
    public float minLaunchForce = 5f;
    public float maxLaunchForce = 15f;
    
    [Header("Thematic Settings")]
    public bool patrioticMode = true;
    public Color[] patrioticColors = new Color[3] { 
        new Color(0.9f, 0.1f, 0.1f), // Red
        Color.white,                  // White
        new Color(0.0f, 0.2f, 0.9f)   // Blue
    };
    
    private float nextLaunchTime;
    private int totalWeight;
    
    void Start()
    {
        // Initialize the next launch time
        nextLaunchTime = Time.time + autoLaunchInterval;
        
        // Calculate total weight for weighted random selection
        CalculateTotalWeight();
        
        // Validate firework types
        if (fireworkTypes == null || fireworkTypes.Length == 0)
        {
            Debug.LogError("No firework types defined! Please add at least one firework type in the inspector.");
            enabled = false;
            return;
        }
    }
    
    void CalculateTotalWeight()
    {
        totalWeight = 0;
        foreach (var type in fireworkTypes)
        {
            totalWeight += type.spawnWeight;
        }
    }
    
    void Update()
    {
        // Manual launch with key press
        if (Input.GetKeyDown(manualLaunchKey))
        {
            LaunchRandomFirework();
        }
        
        // Auto launch based on timer
        if (enableAutoLaunch && Time.time >= nextLaunchTime)
        {
            // Launch multiple fireworks at once
            int fireworkCount = Random.Range(minFireworksPerInterval, maxFireworksPerInterval + 1);
            
            for (int i = 0; i < fireworkCount; i++)
            {
                LaunchRandomFirework();
            }
            
            // Reset the timer
            nextLaunchTime = Time.time + autoLaunchInterval;
        }
    }
    
    void LaunchRandomFirework()
    {
        // Select a random firework type based on weights
        FireworkType selectedType = GetRandomFireworkType();
        
        if (selectedType == null || selectedType.prefab == null)
        {
            Debug.LogError("Selected firework type or prefab is null!");
            return;
        }
        
        // Create instance at random position
        Vector3 position = transform.position + new Vector3(
            Random.Range(-randomPositionRange.x, randomPositionRange.x),
            0,
            Random.Range(-randomPositionRange.y, randomPositionRange.y)
        );
        
        // Instantiate the Visual Effect prefab
        VisualEffect instance = Instantiate(selectedType.prefab, position, Quaternion.identity);
        
        // Set random properties
        float launchForce = Random.Range(minLaunchForce, maxLaunchForce);
        
        // Set color based on patriotic mode or defined color
        Color color = selectedType.primaryColor;
        if (patrioticMode)
        {
            color = patrioticColors[Random.Range(0, patrioticColors.Length)];
        }
        
        // Apply properties to the VFX
        instance.SetFloat("LaunchForce", launchForce);
        instance.SetVector4("FireworkColor", color);
        
        // Set pattern if the VFX supports it
        if (instance.HasInt("PatternType"))
        {
            instance.SetInt("PatternType", (int)selectedType.pattern);
        }
        
        // Set custom sprite if the VFX supports it and sprite is assigned
        if (selectedType.customSprite != null && instance.HasTexture("ParticleTexture"))
        {
            instance.SetTexture("ParticleTexture", selectedType.customSprite);
        }
        
        // Auto-destroy the GameObject after effect is complete
        Destroy(instance.gameObject, 8f);
    }
    
    FireworkType GetRandomFireworkType()
    {
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;
        
        foreach (var type in fireworkTypes)
        {
            cumulativeWeight += type.spawnWeight;
            if (randomValue < cumulativeWeight)
            {
                return type;
            }
        }
        
        // Fallback to first type if something goes wrong
        return fireworkTypes[0];
    }
    
    // For debugging and testing specific firework types
    public void LaunchSpecificFirework(int index)
    {
        if (index >= 0 && index < fireworkTypes.Length)
        {
            FireworkType selectedType = fireworkTypes[index];
            
            // Create instance at center position
            Vector3 position = transform.position;
            
            // Instantiate the Visual Effect prefab
            VisualEffect instance = Instantiate(selectedType.prefab, position, Quaternion.identity);
            
            // Set properties
            float launchForce = (minLaunchForce + maxLaunchForce) / 2f; // Use average force
            
            // Apply properties to the VFX
            instance.SetFloat("LaunchForce", launchForce);
            instance.SetVector4("FireworkColor", selectedType.primaryColor);
            
            // Set pattern if the VFX supports it
            if (instance.HasInt("PatternType"))
            {
                instance.SetInt("PatternType", (int)selectedType.pattern);
            }
            
            // Set custom sprite if the VFX supports it and sprite is assigned
            if (selectedType.customSprite != null && instance.HasTexture("ParticleTexture"))
            {
                instance.SetTexture("ParticleTexture", selectedType.customSprite);
            }
            
            // Auto-destroy the GameObject after effect is complete
            Destroy(instance.gameObject, 8f);
        }
    }
}
