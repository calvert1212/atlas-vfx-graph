using UnityEngine;

public class CameraFollowDelta : MonoBehaviour
{
    [SerializeField] private Transform target; // Reference to NewPlayerShip_0
    private Vector3 lastTargetPosition; // Stores the target's position from the previous frame

    private void Start()
    {
        if (target != null)
        {
            lastTargetPosition = target.position; // Initialize the last position
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate the delta (change in position) of the target
        Vector3 deltaPosition = target.position - lastTargetPosition;

        // Apply the delta to the camera's position
        transform.position += new Vector3(deltaPosition.x, deltaPosition.y, 0);

        // Update the last target position for the next frame
        lastTargetPosition = target.position;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            lastTargetPosition = target.position; // Reset the last position when changing targets
        }
    }
}