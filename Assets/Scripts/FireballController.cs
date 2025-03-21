using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float speed = 10f; // Speed of the fireball movement

    void Update()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the fireball stays in the 2D plane

        // Smoothly move the fireball towards the mouse position
        transform.position = Vector3.Lerp(transform.position, mousePosition, speed * Time.deltaTime);
    }
}