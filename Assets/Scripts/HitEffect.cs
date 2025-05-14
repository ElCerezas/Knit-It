using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitEffect : MonoBehaviour
{
    public static HitEffect Instance;

    [SerializeField] private Image flashImage;
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private Color flashColor = new Color(1, 1, 1, 1f);

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
        flashImage.gameObject.SetActive(true);

        float halfDuration = flashDuration / 2f;

        // Fade in
        for (float t = 0; t < halfDuration/2; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0, flashColor.a, t / halfDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        // Fade out
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(flashColor.a, 0, t / halfDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        flashImage.gameObject.SetActive(false);
    }
}
