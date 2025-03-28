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
    int inputIndex = 0;

    double timeStamp;
    double margainOfError;
    double audioTime;
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                TimeSpan metricTS = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTS.Minutes * 60f + metricTS.Seconds + metricTS.Milliseconds / 1000f);
            }
        }
    }
    private void Update()
    {
        if(spawnIndex < timeStamps.Count)
        {
            if(SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.instance.noteTime)
            {
                NoteLogic note =  Instantiate(notePrefab, transform).GetComponent<NoteLogic>();
                notes.Add(note);
                note.assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }
        if (inputIndex < timeStamps.Count)
        {
            timeStamp = timeStamps[inputIndex];
            margainOfError = SongManager.instance.marginOfError;
            audioTime = SongManager.GetAudioSourceTime() - (SongManager.instance.inputDelayMiliseconds / 1000.0);
        }
        if (timeStamp + margainOfError < audioTime)
        {

        }
    }

    void OnLaneButton()
    {
        if(Math.Abs(audioTime - timeStamp) < margainOfError) //Apretada a tiempo
        {
            Hit();
            Destroy(notes[inputIndex].gameObject);
            inputIndex++;
        }
        else //Fallo
        {
            Miss();
        }
    }
    void Hit()
    {
        Debug.Log("Nota hiteada");
        //SongScoreManager.instance.hitSFX.Play();
    }
    void Miss()
    {
        Debug.Log("Eres mas malo que pegarle a un padre");
        //SongScoreManager.instance.missSFX.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(transform.position.x, -10000), new Vector2(transform.position.x, 10000));
    }
}
