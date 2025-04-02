using UnityEngine;

public class ScoreSongManager : MonoBehaviour
{
    public static ScoreSongManager instance;
    public AudioSource hitSFX, missSFX;
    int score;
    void Start()
    {
        instance = this;
    }

    public void NoteHit()
    {

    }
    public void NoteMiss()
    {

    }
    public void NoteDespawned()
    {

    }
}
