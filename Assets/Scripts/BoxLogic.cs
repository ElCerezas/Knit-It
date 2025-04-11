using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLogic : MonoBehaviour
{
    private void OnEnable()
    {
        SongManager.OnBeat += HandleBeat;
    }

    private void OnDisable()
    {
        SongManager.OnBeat -= HandleBeat;
    }

    void HandleBeat()
    {
        //Que hacer al beat
    }

    public void SpawnNote(NoteType type)
    {
        //Spawnear la nota que toca en la linea 0
    }

}
