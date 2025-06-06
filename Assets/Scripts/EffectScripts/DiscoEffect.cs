using System.Collections;
using UnityEngine;

public class DiscoEffect : MonoBehaviour
{
    [SerializeField] float comboMin = 1f;
    [SerializeField] float comboMax = 2f;
    [SerializeField] Material material;
    float beat;

    float actualTransparency = 0f;
    SpriteRenderer spriteRenderer;
    Coroutine fadeCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ScoreSongManager.OnNewCombo += TransparencyAdjust;
        beat = 60/SongManager.Instance.BPM;
    }

    private void OnDisable()
    {
        ScoreSongManager.OnNewCombo -= TransparencyAdjust;
    }
    private void Update()
    {
        if (material != null && beat > 0f)
        {
            float offsetY = Mathf.Repeat(Time.time / beat, 1f);
            Vector2 offset = new Vector2(0f, offsetY);
            material.SetTextureOffset("_MainTex", offset);
        }

    }
    void TransparencyAdjust(float combo)
    {
        if (combo <= comboMin)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeToZero());
        }
        else
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }
            actualTransparency = Mathf.InverseLerp(comboMin, comboMax, combo);
            SetAlpha(actualTransparency);
        }
    }

    IEnumerator FadeToZero()
    {
        float duration = 1f;
        float startAlpha = actualTransparency;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            actualTransparency = Mathf.Lerp(startAlpha, 0f, time / duration);
            SetAlpha(actualTransparency);
            yield return null;
        }

        actualTransparency = 0f;
        SetAlpha(0f);
        fadeCoroutine = null;
    }

    void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
