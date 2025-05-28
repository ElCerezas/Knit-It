using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingNote : NoteLogic
{
    public override void OnBeatMove()
    {
        int nextRow = actualBox.GetBoxRow() + 2;
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
    public override void OnNoteHit(bool isPerfect)
    {
        base.OnNoteHit(isPerfect);
        if (isPerfect)
        {
            scoreManager.AddLife(10);
        }
        else
        {
            scoreManager.AddLife(5);
        }
    }
}
