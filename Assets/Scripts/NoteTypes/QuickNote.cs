using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickNote : NoteLogic
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
}
