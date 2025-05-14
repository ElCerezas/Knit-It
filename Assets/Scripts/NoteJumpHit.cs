using System.Collections;
using UnityEngine;

public class NoteJumpHit : MonoBehaviour
{
    NoteLogic noteLogic;
    NoteScalerLogic scalerLogic;
    PulseEffectRender pulseEffect;
    Rigidbody2D rb2;
    public Sprite particleSpecific;

    private bool isDestroying = false;

    void Start()
    {
        noteLogic = GetComponent<NoteLogic>();
        scalerLogic = GetComponent<NoteScalerLogic>();
        pulseEffect = GetComponent<PulseEffectRender>();
        rb2 = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isDestroying && (transform.position.x > 8 || transform.position.x < -8))
        {
            isDestroying = true;
            StartCoroutine(PlayAndDestroy());
        }
    }

    Vector2 GenerateForce()
    {
        float x = transform.position.x > 0 ? Random.Range(2f, 5f) : Random.Range(-5f, -2f);
        float y = Random.Range(3f, 7f);
        return new Vector2(x, y) * 1.5f;
    }

    public void StartJump()
    {
        noteLogic.ShutDown();
        noteLogic.enabled = false;
        scalerLogic.enabled = false;

        rb2.bodyType = RigidbodyType2D.Dynamic;
        rb2.gravityScale = 1f;
        rb2.mass = 0.5f;
        rb2.velocity = Vector2.zero;

        Vector2 force = GenerateForce();
        rb2.AddForce(force, ForceMode2D.Impulse);
    }

    IEnumerator PlayAndDestroy()
    {
        // Instanciar partículas
        GameObject explosion = Instantiate(Resources.Load<GameObject>("NoteExplosion"), transform.position, Quaternion.identity);

        if (particleSpecific != null)
        {
            var renderer = explosion.GetComponent<ParticleSystemRenderer>();
            renderer.material = new Material(Shader.Find("Sprites/Default"));
            renderer.material.mainTexture = particleSpecific.texture;
        }

        // Esperar a que terminen (0.5s por defecto)
        yield return new WaitForSeconds(0.5f);

        Destroy(explosion);
        Destroy(gameObject);
    }
}
