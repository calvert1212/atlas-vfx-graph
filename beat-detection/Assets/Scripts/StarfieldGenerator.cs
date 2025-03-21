using UnityEngine;

public class StarfieldGenerator : MonoBehaviour
{
    public int starCount = 100; // Number of stars to generate
    public float starSize = 1f; // Size of each star
    public float starSizeRange = 0.5f; // Range of star sizes
    public Color starColor = Color.white; // Color of the stars
    public Vector2 screenBounds; // Screen bounds for star placement

    void Start()
    {
        GenerateStarfield();
    }

    void GenerateStarfield()
    {
        for (int i = 0; i < starCount; i++)
        {
            GameObject star = new GameObject("Star");
            star.transform.SetParent(transform);

            SpriteRenderer sr = star.AddComponent<SpriteRenderer>();
            sr.sprite = CreateStarSprite();
            sr.color = starColor;

            float randomSize = starSize + Random.Range(-starSizeRange, starSizeRange);
            star.transform.localScale = new Vector3(randomSize, randomSize, 1);

            float randomX = Random.Range(-screenBounds.x, screenBounds.x);
            float randomY = Random.Range(-screenBounds.y, screenBounds.y);
            star.transform.position = new Vector3(randomX, randomY, 0);
        }
    }

    Sprite CreateStarSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
    }
}