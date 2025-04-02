using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScoreSongManager : MonoBehaviour
{
    public static ScoreSongManager instance;
    public AudioSource hitSFX, missSFX;

    int score = 0;
    public int hitIncrease = 10;
    float combo = 1;
    public float comboIncrease = 0.1f;
    int notesToCombo;
    public int comboNotes = 5;

    void Start()
    {
        instance = this;
        notesToCombo = comboNotes;
    }

    public void NoteHit(bool isPrefect)
    {
        notesToCombo--;
        if (notesToCombo == 0)
        {
            combo += comboIncrease;
            notesToCombo = comboNotes;
        }
        score += Convert.ToInt32(hitIncrease * combo);
    }
    public void NoteMiss()
    {
        notesToCombo = comboNotes;
        combo = 1;
        score -= 2;
    }
    public void NoteDespawned()
    {
        notesToCombo = comboNotes;
        combo = 1;
    }
}
