using Unity.Mathematics;
using UnityEngine;
using System.Collections;

public class GumNote : NoteLogic
{
    private bool firstHitDone = false;
    private bool returned = false;
    private int originalRow;
    public override void OnNoteHit(bool isPerfect)
    {
        if (!firstHitDone)
        {
            firstHitDone = true;
            selfCollider = GetComponent<Collider2D>();
            selfCollider.enabled = false;

            scoreManager.NotesToCombo--;
            if (scoreManager.NotesToCombo <= 0)
                scoreManager.AddCombo();

            scoreManager.AddScore(5);
  
            SongManager.OnHalfBeat += ReturnBack;
            SongManager.OnHalfBeat -= OnBeatMove;
        }
        else if (returned)
        {
            base.OnNoteHit(isPerfect);
        }
    }
    public override void OnBeatMove()
    {
        selfCollider.enabled = true;
        base.OnBeatMove();
    }

    private void ReturnBack()
    {
        if (!returned)
        {
            returned = true;
            selfCollider.enabled = false;
            int nextRow = actualBox.GetBoxRow() - 1;
            int col = actualBox.GetBoxCol();
            nextBox = SongManager.Instance.boxGrid[nextRow, col];
            StartCoroutine(MoveTo(nextBox.transform.position));

            // Desuscribirse para evitar multiples llamadas
            SongManager.OnHalfBeat -= ReturnBack;
            SongManager.OnHalfBeat += OnBeatMove;
        }
    }
}