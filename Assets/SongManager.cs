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

    public float noteTime; //Tiempo hasta la zona de hit
    public float noteSpawnY; //Altura del Spawn
    public float noteTapY; //Altura de la barra de tap
    public float noteDespawnY
    {
        get { return noteTapY - (noteSpawnY - noteTapY); }
    } //Cuando ha de despawnear la nota


    void Start()
    {
        instance = this;
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation); //Cargar el archivo MIDI -- No funciona en WebGL
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        Debug.Log(notes.GetType());
        var array = new Note[notes.Count];
        notes.CopyTo(array, 0);

        Invoke(nameof(StartSong), songDelaySeconds);
    }
    public void StartSong()
    {
        audioSource.Play();
    }
    public static double GetAudioSourceTime()
    {
        return (double) (instance.audioSource.timeSamples / instance.audioSource.clip.frequency);
    }
}
