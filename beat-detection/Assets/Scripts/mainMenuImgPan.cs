using UnityEngine;

public class BackgroundPan : MonoBehaviour
{
    public float panSpeed = 0.5f; // Speed at which the background pans
    private bool isPanning = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        EventManager.StartListening("mainMenuStart", StartPanning);
    }

    void Update()
    {
        if (isPanning)
        {
            float newPosition = Mathf.Repeat(Time.time * panSpeed, 1);
            transform.position = startPosition + Vector3.right * newPosition;
        }
    }

    void StartPanning()
    {
        isPanning = true;
    }

    void OnDestroy()
    {
        EventManager.StopListening("mainMenuStart", StartPanning);
    }
}