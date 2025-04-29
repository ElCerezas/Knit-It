using System;
using System.Collections;
using UnityEngine;

public class NoteLogic : MonoBehaviour
{
    protected ScoreSongManager scoreManager;
    protected BoxLogic actualBox, nextBox;
    private SpriteRenderer spriteRenderer;
    private bool spriteActivated = false;

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

        if (spriteRenderer != null)
            spriteRenderer.enabled = false; // Apagar sprite al inicio
    }

    public virtual void OnBeatMove()
    {
        Debug.Log("NoteBeated");
        int nextRow = actualBox.GetBoxRow() + 1; // Movimiento básico: bajar fila
        int col = actualBox.GetBoxCol();         // Mantener la columna

        if (nextRow < SongManager.Instance.boxGrid.GetLength(0))
        {
            nextBox = SongManager.Instance.boxGrid[nextRow, col];
            StartCoroutine(MoveTo(nextBox.transform.position));
        }
        else
        {
            // No hay siguiente casilla -> nota perdida
            OnNoteDespawn();
            Destroy(gameObject);
        }
    }

    public virtual void OnNoteHit(bool isPerfect)
    {
        scoreManager.NotesToCombo--;
        if (scoreManager.NotesToCombo <= 0)
        {
            scoreManager.AddCombo();
        }
        scoreManager.AddScore(10);
        Destroy(gameObject); // Destruir nota al acertar
    }

    public virtual void OnNoteDespawn()
    {
        scoreManager.ResetCombo();
        scoreManager.AddLife(-2);
    }

    protected IEnumerator MoveTo(Vector2 targetPos)
    {
        if (!spriteActivated && spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteActivated = true;
        }

        Vector2 startPos = transform.position;
        float elapsed = 0f;
        float moveDuration = 0.3f; // Duración de movimiento por beat

        while (elapsed < moveDuration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        actualBox = nextBox;
    }
}
