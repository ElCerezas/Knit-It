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
    }

    private void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.instance.noteTime * 2));
        GetComponent<SpriteRenderer>().enabled = true;
        if (t> 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * SongManager.instance.noteSpawnY, Vector3.up * SongManager.instance.noteDespawnY, t);
        }
            
    }
}
