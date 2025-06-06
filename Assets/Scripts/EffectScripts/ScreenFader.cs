using System.Collections;
using UnityEngine.UI;
using UnityEngine;
public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        StartCoroutine(Fade(0f));
    }

    public IEnumerator Fade(float targetAlpha)
    {
        fadeImage.raycastTarget = true;
        Color color = fadeImage.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
        fadeImage.raycastTarget = targetAlpha != 0;
    }
}