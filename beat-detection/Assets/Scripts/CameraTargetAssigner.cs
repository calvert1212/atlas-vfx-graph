using UnityEngine;

public class CameraTargetAssigner : MonoBehaviour
{
    private void Start()
    {
        // Find the CameraFollow script on the Main Camera
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();

        // Find the NewPlayerShip_0 GameObject
        GameObject playerShip = GameObject.Find("NewPlayerShip_0");

        // Assign the player ship as the target for the camera
        if (cameraFollow != null && playerShip != null)
        {
            cameraFollow.SetTarget(playerShip.transform);
        }
        else
        {
            Debug.LogWarning("CameraFollow script or NewPlayerShip_0 not found!");
        }
    }
}