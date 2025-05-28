using System;
using System.Collections;
using UnityEngine;

public class NoteLogic : MonoBehaviour
{
    protected ScoreSongManager scoreManager;
    protected BoxLogic actualBox, nextBox;
    private SpriteRenderer spriteRenderer;
    private bool spriteActivated = false;
    private float beatTime;
    protected Collider2D selfCollider;
    protected NoteJumpHit jumpHit;

    // Tilt control
    protected bool tiltingInitialized = false;
    protected int tiltDirection = 1; // 1 = derecha, -1 = izquierda

    private void OnEnable()
    {
        SongManager.OnHalfBeat += OnBeatMove;
    }

    private void OnDisable()
    {
        SongManager.OnHalfBeat -= OnBeatMove;
    }

    void Start()
    {
        scoreManager = ScoreSongManager.Instance;
        actualBox = GetComponentInParent<BoxLogic>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selfCollider = GetComponent<Collider2D>();
        jumpHit = GetComponent<NoteJumpHit>();

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        beatTime = 60f / SongManager.Instance.BPM;
    }

    public virtual void OnBeatMove()
    {
        int nextRow = actualBox.GetBoxRow() + 1;
        int col = actualBox.GetBoxCol();

        if (nextRow < SongManager.Instance.boxGrid.GetLength(0))
        {
            nextBox = SongManager.Instance.boxGrid[nextRow, col];
            if (gameObject.activeSelf)
            {
                StartCoroutine(MoveTo(nextBox.transform.position));
            }
        }
        else
        {
            OnNoteDespawn();
            StartCoroutine(DestroyAfterDelay(beatTime / 4f));
        }
    }

    public virtual void OnNoteHit(bool isPerfect)
    {
        if (isPerfect)
        {
            scoreManager.AddCombo();
        }
        else
        {
            scoreManager.NotesToCombo--;
            if (scoreManager.NotesToCombo <= 0)
                scoreManager.AddCombo();
        }

        scoreManager.AddScore(10);
        jumpHit.StartJump();
    }

    public virtual void OnNoteDespawn()
    {
        scoreManager.ResetCombo();
        scoreManager.AddLife(-1);
        VolumeEffect.Instance.TriggerErrorEffect(true);
    }

    protected IEnumerator MoveTo(Vector2 targetPos, float quickMultiply = 1f, float tiltAngle = 15f)
    {
        if (!spriteActivated && spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteActivated = true;
        }

        Vector2 startPos = transform.position;
        float elapsed = 0f;
        float moveDuration = (beatTime / 2f) / quickMultiply;

        // Inicializar dirección aleatoria la primera vez
        if (!tiltingInitialized)
        {
            tiltDirection = UnityEngine.Random.value > 0.5f ? 1 : -1;
            tiltingInitialized = true;
        }
        else
        {
            tiltDirection *= -1;
        }

        // Ángulos de rotación
        Quaternion initialRotation = transform.rotation;
        Quaternion tiltedRotation = Quaternion.Euler(0, 0, tiltAngle * tiltDirection);

        while (elapsed < moveDuration / 2f)
        {
            float t = elapsed / (moveDuration / 2f);
            transform.position = Vector2.Lerp(startPos, targetPos, elapsed / moveDuration);
            transform.rotation = Quaternion.Lerp(initialRotation, tiltedRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        while (elapsed < moveDuration)
        {
            float t = (elapsed - moveDuration / 2f) / (moveDuration / 2f);
            transform.position = Vector2.Lerp(startPos, targetPos, elapsed / moveDuration);
            transform.rotation = Quaternion.Lerp(tiltedRotation, initialRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = initialRotation;
        actualBox = nextBox;
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public virtual void ShutDown()
    {
        StopAllCoroutines();
        SongManager.OnHalfBeat -= OnBeatMove;
    }
}
