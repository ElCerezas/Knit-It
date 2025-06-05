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
    [SerializeField] GameObject healthBar;

    [SerializeField]int life = 100;
    int score = 0;
    float combo = 1;
    private float comboIncrease = 0.1f;
    int notesToCombo;
    private int comboNotes = 5;

    public delegate void ScoreUpdate(float newScore);
    public static event ScoreUpdate OnNewScore;
    public static event ScoreUpdate OnNewCombo;

    public delegate void RetryLevel();
    public static event RetryLevel OnLostLevel;

    [SerializeField] GameObject finalScoreManager, gameplayCanvas, sngManager, GameLostCanvas;

    public int NotesToCombo { get => notesToCombo; set => notesToCombo = value; }

    void Start()
    {
        Instance = this;
        NotesToCombo = comboNotes;
    }
    public void NoteMiss()
    {
        ResetCombo();
        AddLife(-2);
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
        combo = Math.Min(combo, 5);
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
            healthBar.GetComponent<Animator>().SetInteger("Health", life);
        }
        if (life <= 0)
        {
            GameLostCanvas.SetActive(true);
            gameplayCanvas.SetActive(false);
            sngManager.SetActive(false);
            OnLostLevel?.Invoke();
        }
    }
    public void CheckGameWin()
    {
        if (score > 0)
        {
            finalScoreManager.SetActive(true);
            gameplayCanvas.SetActive(false);
            if (score > PlayerPrefs.GetInt($"Score{SongManager.Instance.level}", 0))
            {
                PlayerPrefs.SetInt($"Score{SongManager.Instance.level}", score);
            }
            sngManager.SetActive(false);

        }
        else
        {
            GameLostCanvas.SetActive(true);
            gameplayCanvas.SetActive(false);
            sngManager.SetActive(false);
            SoundManager.Instance.PlaySound("Failed");
            OnLostLevel?.Invoke();
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
