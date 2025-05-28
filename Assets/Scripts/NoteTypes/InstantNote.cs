using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantNote : NoteLogic
{
    public override void OnBeatMove()
    {
        int nextRow = SongManager.Instance.boxGrid.GetLength(0)-1;
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
            StartCoroutine(base.DestroyAfterDelay(base.beatTime / 4f));
        }
    }
}
