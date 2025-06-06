using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenNote : NoteLogic
{
    public override void OnNoteHit(bool isPerfect)
    {
        if (isPerfect)
        {
            scoreManager.AddCombo();
            scoreManager.AddCombo();
        }
        else
        {
            scoreManager.AddCombo();
        }
        StopAllCoroutines();
        scoreManager.AddScore(50);
        jumpHit.StartJump();
        SoundManager.Instance.PlaySound("NoteRight");
    }
}
