using Unity.Mathematics;
using UnityEngine;
using System.Collections;

public class ChargeNote : NoteLogic
{
    bool itStopped = false;

    public override void OnBeatMove()
    {
        if (!itStopped && base.actualBox.GetBoxRow() == SongManager.Instance.boxGrid.GetLength(0) - 3)
        {
            itStopped = true;
            animator.SetTrigger("Charge");
        }
        else if (itStopped)
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
        else if (!itStopped)
        {
            base.OnBeatMove();
        }
    }
}
