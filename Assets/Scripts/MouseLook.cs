using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public float smoothing = 2.0f;
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public Vector2 defaultRotation;
    public bool lockCursor = true;
    
    private Vector2 rotation;
    private Vector2 currentMouseLook;
    private Vector2 smoothMouseLook;
    
    void Start()
    {
        rotation = defaultRotation;
        
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    void Update()
    {
        // Allow unlocking cursor with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            lockCursor = false;
        }
        
        // Re-lock cursor when clicking
        if (!lockCursor && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            lockCursor = true;
        }
        
        // Get mouse movement
        currentMouseLook = new Vector2(
            Input.GetAxis("Mouse X"), 
            Input.GetAxis("Mouse Y")
        );
        
        // Apply smoothing
        currentMouseLook = Vector2.Scale(currentMouseLook, new Vector2(
            sensitivity * smoothing,
            sensitivity * smoothing
        ));
        
        // Interpolate mouse movement over time
        smoothMouseLook.x = Mathf.Lerp(smoothMouseLook.x, currentMouseLook.x, 1f / smoothing);
        smoothMouseLook.y = Mathf.Lerp(smoothMouseLook.y, currentMouseLook.y, 1f / smoothing);
        
        // Find the absolute mouse movement value from frame to frame
        rotation += smoothMouseLook;
        
        // Clamp rotation
        if (clampInDegrees.x < 360)
            rotation.x = Mathf.Clamp(rotation.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
        
        if (clampInDegrees.y < 360)
            rotation.y = Mathf.Clamp(rotation.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
        
        // Apply rotation
        transform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up) * 
                                 Quaternion.AngleAxis(-rotation.y, Vector3.right);
    }
}