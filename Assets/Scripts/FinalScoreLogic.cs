using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreLogic : MonoBehaviour
{
    public static FinalScoreLogic Instance;
    [SerializeField] Slider scoreBar;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] GameObject ContinueButton, newHighScore;
    [SerializeField] Image star1, star2, star3;

    int finalScore = 0;
    [SerializeField] int scoreFor3Stars = 1000;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        finalScore = ScoreSongManager.Instance.GetScore();

        StartCoroutine(AnimateScoreText(finalScore, 2f)); // 2 segundos de animación
    }
    IEnumerator AnimateScoreText(int targetScore, float duration)
    {
        float elapsed = 0f;
        int startScore = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
            score.text = currentScore.ToString();
            scoreBar.value = currentScore / scoreFor3Stars;
            yield return null;
        }

        // Asegurarse que el score final es exacto
        score.text = targetScore.ToString();
    }

}
