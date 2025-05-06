using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagNote : NoteLogic
{
    public bool toRight;
    public override void OnBeatMove()
    {
        int currentRow = actualBox.GetBoxRow();
        int currentCol = actualBox.GetBoxCol();
        int nextCol;
        int nextRow = currentRow + 1;

        toRight = !toRight;
        if (toRight)
        {
            nextCol = currentCol + 1;
        }
        else
        {
            nextCol = currentCol - 1;
        }

        int maxRows = SongManager.Instance.boxGrid.GetLength(0);
        int maxCols = SongManager.Instance.boxGrid.GetLength(1);

        if (nextRow < maxRows && nextCol >= 0 && nextCol < maxCols)
        {
            nextBox = SongManager.Instance.boxGrid[nextRow, nextCol];
            StartCoroutine(MoveTo(nextBox.transform.position));
        }
        else
        {
            OnNoteDespawn();
            Destroy(gameObject);
        }
    }

}
