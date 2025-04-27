using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreSongManager : MonoBehaviour
{
    public static ScoreSongManager Instance;
    public AudioSource hitSFX, missSFX;
    [SerializeField] Slider healthBar;

    int life = 100;
    int score = 0;
    private int hitIncrease = 10;
    float combo = 1;
    private float comboIncrease = 0.1f;
    int notesToCombo;
    private int comboNotes = 5;

    public delegate void ScoreUpdate(float newScore);
    public static event ScoreUpdate OnNewScore;
    public static event ScoreUpdate OnNewCombo;

    public delegate void RetryLevel();
    public static event RetryLevel OnLostLevel;

    [SerializeField] GameObject finalScoreManager, gameplayCanvas, SongManager, GameLostCanvas;

    public int NotesToCombo { get => notesToCombo; set => notesToCombo = value; }

    void Start()
    {
        Instance = this;
        NotesToCombo = comboNotes;
    }
    public void NoteMiss()
    {
        ResetCombo();
        AddLife(-5);
    }
    public int GetScore()
    {
        return score;
    }
    public int GetLife()
    {
        return life;
    }
    public void AddCombo()
    {
        combo += comboIncrease;
        NotesToCombo = comboNotes;
        OnNewCombo?.Invoke(combo);
    }
    public void AddScore(int howMuch)
    {
        if (howMuch >=0)
        {
            score += Convert.ToInt32(howMuch * combo);
        } 
        else
        {
            score -= howMuch;
        }
        OnNewScore?.Invoke(score);
    }
    public void AddLife(int howMuch)
    {
        life += howMuch;
        CheckLife();
    }

    public void ResetCombo()
    {
        NotesToCombo = comboNotes;
        combo = 1;
        OnNewCombo?.Invoke(combo);
    }
    void CheckLife()
    {
        if (healthBar != null)
        {
            healthBar.value = life;
        }
        if (life <= 0)
        {
            GameLostCanvas.SetActive(true);
            gameplayCanvas.SetActive(false);
            SongManager.SetActive(false);
            OnLostLevel?.Invoke();
        }
    }
    public void CheckGameWin()
    {
        if (score > 0)
        {
            finalScoreManager.SetActive(true);
            gameplayCanvas.SetActive(false);
            SongManager.SetActive(false);

            PlayerPrefs.SetInt("Score1", score);
        }
    }
   public void OnRestart()
    {
        if(life <= 0)
        {
            SceneController.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
