using Unity.Mathematics;
using UnityEngine;

public class GumNote : NoteLogic
{
    private bool firstHitDone = false;
    private bool returned = false;
    private int originalRow;
    private int col;
    Collider2D selfCollider;
    public override void OnNoteHit(bool isPerfect)
    {
        if (!firstHitDone)
        {
            firstHitDone = true;
            selfCollider = GetComponent<Collider2D>();
            selfCollider.enabled = false;

            scoreManager.NotesToCombo--;
            if (scoreManager.NotesToCombo <= 0)
                scoreManager.AddCombo();

            scoreManager.AddScore(5);

            originalRow = actualBox.GetBoxRow();
            col = actualBox.GetBoxCol();
            int nextRow = math.max(0, originalRow - 1);
            nextBox = SongManager.Instance.boxGrid[nextRow, col];

            StartCoroutine(MoveTo(nextBox.transform.position));
            // Subscribirse a un beat para regresar
            SongManager.OnBeat += ReturnToOriginalBox;
        }
        else
        {
            base.OnNoteHit(isPerfect);
        }
    }

    private void ReturnToOriginalBox()
    {
        if (!returned)
        {
            returned = true;
            selfCollider.enabled = true;
            nextBox = SongManager.Instance.boxGrid[originalRow, col];
            StartCoroutine(MoveTo(nextBox.transform.position));

            // Desuscribirse para evitar múltiples llamadas
            SongManager.OnBeat -= ReturnToOriginalBox;
        }
    }
}
