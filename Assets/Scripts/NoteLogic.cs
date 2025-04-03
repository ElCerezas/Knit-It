using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLogic : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;
    ScoreSongManager ScoreSongManager;

    NoteTypes noteType;

    private void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        ScoreSongManager = ScoreSongManager.Instance;
    }
    public void SetNoteType(NoteTypes type)
    {
        noteType = type;
    }
    private void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime));
        if (t > 1)
        {
            ScoreSongManager.NoteDespawned();
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.Lerp(new Vector2(transform.position.x, SongManager.Instance.noteSpawnY), new Vector2(transform.position.x, SongManager.Instance.noteDespawnY), t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
            
    }
}
