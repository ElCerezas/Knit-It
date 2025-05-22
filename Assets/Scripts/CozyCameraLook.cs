using UnityEngine;

public class SubtleMouseCamera : MonoBehaviour
{
    public float maxOffset = 0.2f;     
    public float smoothSpeed = 2f;    
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;
    }

    void Update()
    {
        Vector2 mouse = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float normalizedX = (mouse.x / screenWidth - 0.5f) * 2f;
        float normalizedY = (mouse.y / screenHeight - 0.5f) * 2f;

        Vector3 offset = new Vector3(normalizedX, normalizedY, 0f) * maxOffset;
        targetPosition = initialPosition + offset;

        // Interpolación suave
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
