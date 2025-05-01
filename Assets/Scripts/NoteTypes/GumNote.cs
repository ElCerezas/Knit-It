public class GumNote : NoteLogic
{
    private bool firstHitDone = false;

    public override void OnNoteHit(bool isPerfect)
    {
        if (!firstHitDone)
        {
            firstHitDone = true;
            scoreManager.NotesToCombo--; 
            if (scoreManager.NotesToCombo <= 0)
                scoreManager.AddCombo();
            scoreManager.AddScore(5);
            MoveUpOneBox();
        }
        else
        {
            base.OnNoteHit(isPerfect);
        }
    }

    private void MoveUpOneBox()
    {
        int prevRow = actualBox.GetBoxRow() - 1; 
        int col = actualBox.GetBoxCol();

        if (prevRow >= 0)
        {
            BoxLogic prevBox = SongManager.Instance.boxGrid[prevRow, col];
            actualBox = prevBox;
            StartCoroutine(MoveTo(prevBox.transform.position));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}