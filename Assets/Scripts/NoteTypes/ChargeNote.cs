using Unity.Mathematics;
using UnityEngine;
using System.Collections;

public class ChargeNote : NoteLogic
{
    bool itStoped = false;
    public override void OnBeatMove()
    {
        if (!itStoped && base.actualBox.GetBoxRow() == SongManager.Instance.boxGrid.GetLength(0) - 2)
        {
            itStoped = true;
            animator.SetTrigger("Charge");
        } else if (itStoped)
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
        else
        {
            base.OnBeatMove();
        }
    }
}