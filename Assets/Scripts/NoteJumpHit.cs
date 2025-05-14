using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteJumpHit : MonoBehaviour
{
    NoteLogic noteLogic;
    NoteScalerLogic scalerLogic;
    NotePulseEffect pulseEffect;
    Rigidbody2D rb2;

    void Start()
    {
        noteLogic = GetComponent<NoteLogic>();
        scalerLogic = GetComponent<NoteScalerLogic>();
        pulseEffect = GetComponent<NotePulseEffect>();
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.x > 8)
        {
            Destroy(gameObject);
        }
        else if (transform.position.x < -8 )
        {
            Destroy(gameObject);
        }
    }
    Vector2 GenerateForce()
    {
        float x, y;
        if (transform.position.x > 0)
        {
            x = Random.Range(2f, 5f);
        }
        else
        {
            x = Random.Range(2f, 5f) * -1;
        }
        y = Random.Range(3f, 7f);
        return new Vector2(x, y)*1.5f;
    }

    public void StartJump()
    {
        noteLogic.enabled = false;
        scalerLogic.enabled = false;
        pulseEffect.enabled = false;

        rb2.bodyType = RigidbodyType2D.Dynamic;
        rb2.gravityScale = 1f; // Asegura que la gravedad esté activa
        rb2.mass = 0.5f;
        rb2.velocity = Vector2.zero;

        Vector2 force = GenerateForce();
        rb2.AddForce(force, ForceMode2D.Impulse); // Usa Impulse para que se note como un salto
    }
}
