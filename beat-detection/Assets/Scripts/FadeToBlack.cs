using UnityEngine;
using System.Collections;

public class FadeToBlack : MonoBehaviour
{
    public float fadeDuration = 2f; // Duration of the fade in seconds

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void StartFade()
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r * alpha, originalColor.g * alpha, originalColor.b * alpha, alpha);
            yield return null;
        }
        spriteRenderer.color = Color.black;
    }
}