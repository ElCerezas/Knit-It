using System;
using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class NoteLogic : MonoBehaviour
{
    protected ScoreSongManager scoreManager;
    protected BoxLogic actualBox, nextBox;

    void Start()
    {
        scoreManager = ScoreSongManager.Instance;
        actualBox = GetComponentInParent<BoxLogic>();
    }

    public virtual void OnBeatMove()
    {
        int nextRow = actualBox.GetBoxRow() + 1; //Movimiento basico de bajar
        int col = actualBox.GetBoxCol(); //Se mantiene la columna

        if (nextRow < SongManager.Instance.boxGrid.GetLength(0))
        {
            nextBox = SongManager.Instance.boxGrid[nextRow, col];
            StartCoroutine(MoveTo(nextBox.transform.position));
        }
        else
        {
            // Eliminar nota + despawn
            OnNoteDespawn();
            Destroy(gameObject);
        }
    }

    public virtual void OnNoteHit()
    {
        scoreManager.NotesToCombo--;
        if (scoreManager.NotesToCombo <= 0)
        {
            scoreManager.AddCombo();
        }
        scoreManager.AddScore(10);
    }
    public virtual void OnNoteDespawn()
    {
        scoreManager.ResetCombo();
        scoreManager.AddLife(-2);
    }

    protected IEnumerator MoveTo(Vector2 targetPos)
    {
        Vector2 startPos = transform.position;
        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        actualBox = nextBox;
    }
}
