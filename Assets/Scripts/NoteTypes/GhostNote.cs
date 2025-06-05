using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostNote : NoteLogic
{
    [SerializeField] private float fadeDuration = -1;
    public override void OnBeatMove()
    {
        if(fadeDuration == -1)
        {
            fadeDuration = beatTime / 4f;
        }
        if (base.actualBox.GetBoxRow() == SongManager.Instance.boxGrid.GetLength(0) - 2)
        {
            SongManager.OnHalfBeat -= OnBeatMove;
            StartCoroutine(FadeAndAscend());
        }
        else
        {
            base.OnBeatMove();
        }
    }

    IEnumerator FadeAndAscend()
    {
        float time = 0f;
        float startY = transform.position.y;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up;

        Color originalColor = spriteRenderer.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            transform.position = Vector3.Lerp(startPos, endPos, t);

            Color c = originalColor;
            c.a = Mathf.Lerp(originalColor.a, 0f, t);
            spriteRenderer.color = c;

            yield return null;
        }

        Destroy(gameObject);
    }
}
