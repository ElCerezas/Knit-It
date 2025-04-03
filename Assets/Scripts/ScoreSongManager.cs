using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScoreSongManager : MonoBehaviour
{
    public static ScoreSongManager Instance;
    public AudioSource hitSFX, missSFX;

    int score = 0;
    public int hitIncrease = 10;
    float combo = 1;
    public float comboIncrease = 0.1f;
    int notesToCombo;
    public int comboNotes = 5;

    public delegate void ScoreUpdate(float newScore);
    public static event ScoreUpdate OnNewScore;
    public static event ScoreUpdate OnNewCombo;
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
        OnNewScore?.Invoke(score);
        OnNewCombo?.Invoke(combo);
    }
    public void NoteDespawned()
    {
        notesToCombo = comboNotes;
        combo = 1;
        OnNewScore?.Invoke(score);
        OnNewCombo?.Invoke(combo);
    }
}
