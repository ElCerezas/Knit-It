using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneLogic : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;

    public GameObject notePrefab;
    List<NoteLogic> notes = new List<NoteLogic>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    void Update()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<NoteLogic>());
                note.GetComponent<NoteLogic>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(transform.position.x, -10000), new Vector2(transform.position.x, 10000));
    }
}
