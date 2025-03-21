using UnityEngine;
using UnityEngine.VFX;

public class FireworkController : MonoBehaviour
{
    private VisualEffect vfx;
    
    void Awake()
    {
        vfx = GetComponent<VisualEffect>();
    }
    
    void Start()
    {
        // Auto-destroy after effect is complete
        float totalDuration = vfx.GetFloat("TotalDuration");
        Destroy(gameObject, totalDuration);
    }
}
