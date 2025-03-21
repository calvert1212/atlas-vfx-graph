using UnityEngine;

public class CameraMoveWithKeys : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Speed of camera movement

    private void Update()
    {
        // Get input from arrow keys
        float moveX = Input.GetAxis("Horizontal"); // Left/Right arrow keys or A/D
        float moveY = Input.GetAxis("Vertical");   // Up/Down arrow keys or W/S

        // Calculate movement vector
        Vector3 movement = new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;

        // Apply movement to the camera's position
        transform.position += movement;
    }
}