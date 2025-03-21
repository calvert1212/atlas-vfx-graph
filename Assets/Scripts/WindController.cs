using UnityEngine;

[ExecuteInEditMode]
public class WindController : MonoBehaviour
{
    [Header("Wind Settings")]
    [Tooltip("Direction of the wind")]
    public Vector3 windDirection = new Vector3(1, 0, 0);
    
    [Range(0, 10)]
    [Tooltip("Overall strength of the wind")]
    public float windIntensity = 1.0f;
    
    [Range(0, 5)]
    [Tooltip("Speed of wind variation")]
    public float windVariationSpeed = 1.0f;
    
    [Range(0, 1)]
    [Tooltip("Amount of random variation in wind strength")]
    public float windTurbulence = 0.2f;
    
    [Range(0, 10)]
    [Tooltip("Size of wind gusts")]
    public float gustScale = 2.0f;
    
    // Properties accessible to the shader graph
    private Vector4 _windParams;
    private Vector4 _windDirection;
    
    // Material property IDs
    private int _windParamsID;
    private int _windDirectionID;
    
    private void OnEnable()
    {
        _windParamsID = Shader.PropertyToID("_WindParams");
        _windDirectionID = Shader.PropertyToID("_WindDirection");
        
        UpdateWindParameters();
    }
    
    private void Update()
    {
        UpdateWindParameters();
    }
    
    private void UpdateWindParameters()
    {
        // Normalize wind direction
        Vector3 normalizedDirection = windDirection.normalized;
        
        // Pack wind parameters
        _windParams = new Vector4(windIntensity, windVariationSpeed, windTurbulence, gustScale);
        _windDirection = new Vector4(normalizedDirection.x, normalizedDirection.y, normalizedDirection.z, 0);
        
        // Set global shader parameters
        Shader.SetGlobalVector(_windParamsID, _windParams);
        Shader.SetGlobalVector(_windDirectionID, _windDirection);
    }
}
