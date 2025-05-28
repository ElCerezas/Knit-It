using UnityEngine;

public class KnittersMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float smoothTime = 0.1f;

    private float targetX;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        targetX = transform.position.x;
    }

    void Update()
    {
        bool keyPressed = false;

        if (Input.GetKeyDown(KeyCode.D))
        {
            targetX = -0.27f;
            keyPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            targetX = -1.52f;
            keyPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            targetX = 3.18f;
            keyPressed = true;
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            targetX = 5.22f;
            keyPressed = true;
        }

        // Si no se presionó ninguna tecla, mover hacia el centro
        if (!keyPressed)
        {
            targetX = 2.3f;
        }

        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime * Time.deltaTime * moveSpeed);
    }
}
