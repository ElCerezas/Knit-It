using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostNote : NoteLogic
{
    bool itStoped = false;
    public override void OnBeatMove()
    {
        if (base.actualBox.GetBoxRow() == SongManager.Instance.boxGrid.GetLength(0) - 1)
        {
            DestroyAfterDelay(beatTime / 4f);
        }
        else
        {
            base.OnBeatMove();
        }
    }
    public override void OnNoteDespawn()
    {
        
    }
}
