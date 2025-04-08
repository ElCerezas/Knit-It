using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalScoreLogic : MonoBehaviour
{
    public static FinalScoreLogic Instance;
    [SerializeField] Slider scoreBar;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] GameObject ContinueButton, newHighScore;
    [SerializeField] Image star1, star2, star3;

    int finalScore = 0;
    [SerializeField] float scoreFor3Stars = 1000;
    bool pressToNext = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        finalScore = ScoreSongManager.Instance.GetScore();

        SetImageAlpha(star1, 0f);
        SetImageAlpha(star2, 0f);   
        SetImageAlpha(star3, 0f);

        StartCoroutine(AnimateScoreText(finalScore, 2f)); // 2 segundos de animación
    }
    void Update()
    {
        if (pressToNext && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Menu");
        }
    }
    IEnumerator AnimateScoreText(int targetScore, float duration)
    {
        float elapsed = 0f;
        int startScore = 0;

        bool star1Shown = false;
        bool star2Shown = false;
        bool star3Shown = false;

        float star1Threshold = scoreFor3Stars * 0.25f;
        float star2Threshold = scoreFor3Stars * 0.50f;
        float star3Threshold = scoreFor3Stars;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = Mathf.SmoothStep(0, 1, t);

            int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, easedT));
            score.text = currentScore.ToString();
            scoreBar.value = currentScore;

            if (!star1Shown && currentScore >= star1Threshold)
            {
                StartCoroutine(FadeInImage(star1));
                star1Shown = true;
            }
            if (!star2Shown && currentScore >= star2Threshold)
            {
                StartCoroutine(FadeInImage(star2));
                star2Shown = true;
            }
            if (!star3Shown && currentScore >= star3Threshold)
            {
                StartCoroutine(FadeInImage(star3));
                star3Shown = true;
            }

            yield return null;
        }

        score.text = targetScore.ToString();
        scoreBar.value = targetScore;
        pressToNext = true;
    }

    void SetImageAlpha(Image image, float alpha)
    {
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }
    IEnumerator FadeInImage(Image image, float fadeTime = 0.2f)
    {
        float elapsed = 0f;
        Color c = image.color;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeTime);
            c.a = alpha;
            image.color = c;
            yield return null;
        }
    }

}
