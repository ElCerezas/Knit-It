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

    private void OnEnable()
    {
        SongManager.OnBeat += OnBeatMove;
    }
    private void OnDisable()
    {
        SongManager.OnBeat -= OnBeatMove;
    }
    void Start()
    {
        scoreManager = ScoreSongManager.Instance;
        actualBox = GetComponentInParent<BoxLogic>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selfCollider = GetComponent<Collider2D>();
        jumpHit = GetComponent<NoteJumpHit>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = false; // Apagar sprite al inicio
        beatTime = 60/SongManager.Instance.BPM;
    }
    public virtual void OnBeatMove()
    {
        int nextRow = actualBox.GetBoxRow() + 1;
        int col = actualBox.GetBoxCol();    

        if (nextRow < SongManager.Instance.boxGrid.GetLength(0))
        {
            nextBox = SongManager.Instance.boxGrid[nextRow, col];
            StartCoroutine(MoveTo(nextBox.transform.position));
        }
        else
        {
            OnNoteDespawn();
            Destroy(gameObject);
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
            {
                scoreManager.AddCombo();
            }
        }
        scoreManager.AddScore(10);
        jumpHit.StartJump();
    }
    public virtual void OnNoteDespawn()
    {
        scoreManager.ResetCombo();
        scoreManager.AddLife(-1);
    }
    protected IEnumerator MoveTo(Vector2 targetPos, float quickMultiply = 1)
    {
        if (!spriteActivated && spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteActivated = true;
        }

        Vector2 startPos = transform.position;
        float elapsed = 0f;
        float moveDuration = (beatTime/2)/quickMultiply; // Duración de movimiento por beat

        while (elapsed < moveDuration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        actualBox = nextBox;
    }
    public void ShutDown()
    {
        StopAllCoroutines();
        SongManager.OnBeat -= OnBeatMove;
    }
}
