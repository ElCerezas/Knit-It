using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagNote : NoteLogic
{
    private int direction;
    void Start()
    {
        direction = UnityEngine.Random.value < 0.5f ? -1 : 1;
    }
    public override void OnBeatMove()
    {
        int currentRow = actualBox.GetBoxRow();
        int currentCol = actualBox.GetBoxCol();

        int nextRow = currentRow + 1;
        int nextCol = currentCol + direction;

        if (nextCol < 0 || nextCol >= SongManager.Instance.boxGrid.GetLength(1))
        {
            direction *= -1; 
            nextCol = currentCol + direction; 
        }

        if (nextRow < SongManager.Instance.boxGrid.GetLength(0))
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
