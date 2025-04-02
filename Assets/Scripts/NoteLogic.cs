using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLogic : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    private void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
        Debug.Log("NoteSpawned");
    }

    private void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.instance.noteTime));
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.Lerp(Vector2.up * SongManager.instance.noteSpawnY, Vector2.up * SongManager.instance.noteDespawnY, t);
            GetComponent<SpriteRenderer>().enabled = true;
        }
            
    }
}
