using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLogic : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    ScoreSongManager ScoreSongManager;

    private void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        ScoreSongManager = ScoreSongManager.instance;
        ScoreSongManager.papa.Add(gameObject);
    }

    private void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.instance.noteTime));
        if (t > 1)
        {
            ScoreSongManager.NoteDespawned();
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.Lerp(new Vector2(transform.position.x, SongManager.instance.noteSpawnY), new Vector2(transform.position.x, SongManager.instance.noteDespawnY), t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
            
    }
}
