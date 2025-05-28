using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeintNote : NoteLogic
{
    bool itStoped = false;
    public override void OnBeatMove()
    {
        if (!itStoped && base.actualBox.GetBoxRow() == SongManager.Instance.boxGrid.GetLength(0) - 1)
        {
            itStoped = true;
        }
        else
        {
            base.OnBeatMove();
        }
    }
}
