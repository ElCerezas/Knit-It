using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreSongManager : MonoBehaviour
{
    public static ScoreSongManager Instance;
    public AudioSource hitSFX, missSFX;
    [SerializeField] Slider healthBar;

    int life = 100;
    int score = 0;
    public int hitIncrease = 10;
    float combo = 1;
    public float comboIncrease = 0.1f;
    int notesToCombo;
    public int comboNotes = 5;

    public delegate void ScoreUpdate(float newScore);
    public static event ScoreUpdate OnNewScore;
    public static event ScoreUpdate OnNewCombo;

    public delegate void RetryLevel();
    public static event RetryLevel OnRetryLevel;

    [SerializeField] GameObject finalScoreManager, gameplayCanvas, SongManager, GameLostCanvas;
    void Start()
    {
        Instance = this;
        notesToCombo = comboNotes;
    }

    public void NoteHit(bool isPrefect)
    {
        notesToCombo--;
        if (isPrefect)
        {
            Debug.Log("Perfect");
            notesToCombo--;
        }
        if (notesToCombo <= 0)
        {
            combo += comboIncrease;
            notesToCombo = comboNotes + notesToCombo;
            OnNewCombo?.Invoke(combo);
        }
        score += Convert.ToInt32(hitIncrease * combo);
        OnNewScore?.Invoke(score);
    }
    public void NoteMiss()
    {
        notesToCombo = comboNotes;
        combo = 1;
        score -= 2;
        life -= 10;
        OnNewScore?.Invoke(score);
        OnNewCombo?.Invoke(combo);
    }
    public void NoteDespawned()
    {
        notesToCombo = comboNotes;
        combo = 1;
        life--;
        OnNewScore?.Invoke(score);
        OnNewCombo?.Invoke(combo);
    }
    public int GetScore()
    {
        return score;
    }
    public int GetLife()
    {
        return life;
    }

    void ChechLife()
    {
        if (life <= 0)
        {
            GameLostCanvas.SetActive(true);
            OnRetryLevel?.Invoke();
        }
    }
    public void CheckGameWin()
    {
        if (score > 0)
        {
            finalScoreManager.SetActive(true);
            gameplayCanvas.SetActive(false);
            SongManager.SetActive(false);
        }
    }

    void OnRestart()
    {
        SceneController.Instance.LoadScene("SceneManager.loadedSceneCount");
    }
}
