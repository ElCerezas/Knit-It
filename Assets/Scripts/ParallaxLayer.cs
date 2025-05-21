using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor = 0.1f; 
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        float normalizedX = (mousePos.x / Screen.width - 0.5f) * 2f; 
        transform.position = startPos + new Vector3(normalizedX * parallaxFactor, 0, 0);
    }
}
