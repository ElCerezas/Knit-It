using UnityEngine;

public class SongScoreManager : MonoBehaviour
{
    public static SongScoreManager instance;
    public AudioSource hitSFX, missSFX;
    int score;
    void Start()
    {
        instance = this;
    }
}
