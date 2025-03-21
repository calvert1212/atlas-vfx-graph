// filepath: c:\Users\calve\Fireworks\Assets\Scripts\ParticleSystem.cs
using UnityEngine;

public class BlueFireworkEffect : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Sprite particleSprite;

    void Start()
    {
        if (particleSystem != null && particleSprite != null)
        {
            // Get the Particle System Renderer
            var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();

            // Create a new Material with a Sprite Shader
            Material spriteMaterial = new Material(Shader.Find("Sprites/Default"));
            spriteMaterial.mainTexture = particleSprite.texture;

            // Assign the material to the Particle System Renderer
            renderer.material = spriteMaterial;

            // Ensure the particle system uses the texture
            var textureSheetAnimation = particleSystem.textureSheetAnimation;
            textureSheetAnimation.enabled = true;
            textureSheetAnimation.mode = ParticleSystemAnimationMode.Sprites;
            textureSheetAnimation.AddSprite(particleSprite);
        }
        else
        {
            Debug.LogError("Particle System or Sprite is not assigned!");
        }
    }
}