using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] TMP_Text score, combo;
    private void OnEnable()
    {
        ScoreSongManager.OnNewCombo += UpdateCombo;
        ScoreSongManager.OnNewScore += UpdateScore;
        UpdateCombo(1);
        UpdateScore(0);
    }
    private void OnDisable()
    {
        ScoreSongManager.OnNewCombo -= UpdateCombo;
        ScoreSongManager.OnNewScore -= UpdateScore;
    }

    void UpdateCombo(float c)
    {
        if(c > 1)
        {
            combo.text = $"x{c}";
        }
        else
        {
            combo.text = $"";
        }
        
    }
    void UpdateScore(float s)
    {
        score.text = $"Score: {s}";
    }
}
