using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFXController : MonoBehaviour
{
    [SerializeField] ParticleSystem clickFX, moveFX;

    private Vector2 mousePos;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;

        if (Input.GetMouseButtonDown(0))
        {
            clickFX.Play();
        }
    }
}
