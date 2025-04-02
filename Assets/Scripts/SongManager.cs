using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class SongManager : MonoBehaviour
{
    public static SongManager instance;
    public AudioSource audioSource;
    public float songDelaySeconds;
    public double marginOfError;
    public int inputDelayMiliseconds;
    

    public string fileLocation;
    public static MidiFile midiFile;

    public LaneLogic[] lanes;
    public float noteTime; //Tiempo hasta la zona de hit
    public float noteSpawnY; //Altura del Spawn
    public float noteTapY; //Altura de la barra de tap
    public float noteDespawnY;


    void Start()
    {
        instance = this;
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation); //Cargar el archivo MIDI -- No funciona en WebGL
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Note[notes.Count];
        notes.CopyTo(array, 0);
        
        foreach (LaneLogic lane in lanes)
        {
            lane.SetTimeStamps(array);
        }
        Debug.Log("pLAYINGsONG");
        Invoke(nameof(StartSong), songDelaySeconds);
    }
    public void StartSong()
    {
        audioSource.Play();
    }
    public static double GetAudioSourceTime()
    {
        return (double)instance.audioSource.timeSamples / instance.audioSource.clip.frequency;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(new Vector2(-1000, noteSpawnY), new Vector2(1000, noteSpawnY));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(-1000, noteDespawnY), new Vector2(1000, noteDespawnY));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(-1000, noteTapY), new Vector2(1000, noteTapY));
    }
}
