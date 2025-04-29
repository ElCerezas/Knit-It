using UnityEngine;

public class SubtleMouseCamera : MonoBehaviour
{
    public float maxOffset = 0.2f; 
    public float edgeThreshold = 0.1f;   
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

        float horizontalOffset = 0f;
        float verticalOffset = 0f;

        if (mouse.x < screenWidth * edgeThreshold)
            horizontalOffset = -1f;
        else if (mouse.x > screenWidth * (1f - edgeThreshold))
            horizontalOffset = 1f;

        if (mouse.y < screenHeight * edgeThreshold)
            verticalOffset = -1f;
        else if (mouse.y > screenHeight * (1f - edgeThreshold))
            verticalOffset = 1f;

        Vector3 offset = new Vector3(horizontalOffset, verticalOffset, 0f) * maxOffset;
        targetPosition = initialPosition + offset;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
    }
}
