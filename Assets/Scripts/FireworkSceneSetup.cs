using UnityEngine;

public class FireworkSceneSetup : MonoBehaviour
{
    public Camera mainCamera;
    public Light sceneLight;
    
    void Start()
    {
        // Set up dark scene
        RenderSettings.ambientLight = Color.black;
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fogDensity = 0.02f;
        
        // Configure camera
        if (mainCamera != null)
        {
            mainCamera.backgroundColor = Color.black;
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
        }
        
        // Configure lighting
        if (sceneLight != null)
        {
            sceneLight.intensity = 0.2f;
            sceneLight.color = new Color(0.1f, 0.1f, 0.2f);
        }
    }
}
