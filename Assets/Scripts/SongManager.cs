using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine.InputSystem;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public float songDelaySeconds;


    public string fileLocation;
    public static MidiFile midiFile;

    public LaneLogic[] lanes;
    public float noteTime; //Tiempo hasta la zona de hit
    public float noteSpawnY; //Altura del Spawn
    public float noteTapY; //Altura de la barra de tap
    public float noteDespawnY;

    bool songStarted = false;
    [SerializeField] GameObject finalScoreManager, gameplayCanvas;

    NoteTypes noteToSpawn = NoteTypes.Basic;
    private void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            Debug.Log(Instance.audioSource.timeSamples);
        }
        if(!audioSource.isPlaying && songStarted)
        {
            Debug.Log("SongEnded");
            songStarted = false;
            finalScoreManager.SetActive(true);
            gameplayCanvas.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    void Start()
    {
        Instance = this;
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation); //Cargar el archivo MIDI -- No funciona en WebGL
        GetDataFromMidi();
    }
    public void LoadNewMidi(string newFileName)
    {
        fileLocation = newFileName;
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
    }
    public void SetUpNewSong(AudioClip clip)
    {
        audioSource.clip = clip;
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
        songStarted = true;
    }
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
    public NoteTypes GetNoteTypeToSpawn()
    {
        return noteToSpawn;
    }
    public void SetNoteType(NoteTypes newNoteToSpawn)
    {
        noteToSpawn = newNoteToSpawn;
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
