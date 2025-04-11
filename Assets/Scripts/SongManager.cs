using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Linq;
using System.IO;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public float songDelaySeconds;

    public string level; //Subcarpeta de streaming Assets
    public string fileLocation; //Archivos

    public static MidiFile midiFile;

    [SerializeField] BoxLogic[] flatArray;
    public BoxLogic[,] boxGrid;

    public static event Action OnBeat;

    private List<BeatData> beatMap = new List<BeatData>();
    private TempoMap tempoMap;
    private bool songStarted = false;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        boxGrid = ConvertTo2DArray(flatArray);
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + level + "/" + fileLocation + ".mid");
        GetDataFromMidi();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && songStarted)
        {
            Debug.Log("SongEnded");
            songStarted = false;
            ScoreSongManager.Instance.CheckGameWin();
        }
    }

    public void GetDataFromMidi()
    {
        tempoMap = midiFile.GetTempoMap();
        var notes = midiFile.GetNotes();

        List<double> beatTimes = new List<double>();

        foreach (var note in notes)
        {
            var metricTime = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, tempoMap);
            var beatTime = metricTime.Minutes * 60 + metricTime.Seconds + metricTime.Milliseconds / 1000f;

            // Si es nota de beat
            if (note.NoteNumber == 67)
            {
                beatTimes.Add(beatTime);
                continue;
            }
            // Si es una nota válida para spawnear
            if (note.NoteNumber >= 60 && note.NoteNumber <= 63)
            {
                int column = note.NoteNumber - 60;
                beatMap.Add(new BeatData
                {
                    time = beatTime,
                    column = column,
                    type = NoteType.Basic
                });
            }
        }

        beatMap = beatMap.OrderBy(b => b.time).ToList();
        beatTimes = beatTimes.OrderBy(t => t).ToList();

        Debug.Log("Loaded beat map with " + beatMap.Count + " notes");
        Debug.Log("Loaded " + beatTimes.Count + " beat events");

        StartCoroutine(BeatLoop(beatTimes));
        Invoke(nameof(StartSong), songDelaySeconds);
    }
    private void LoadOverrides()
    {
        string path = Application.streamingAssetsPath + "/" + level + "/" + fileLocation + "_overrides.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            List<BeatOverrideData> overrides = JsonUtilityWrapper.FromJsonList<BeatOverrideData>(json);

            foreach (var ovr in overrides)
            {
                if (ovr.beatIndex >= 0 && ovr.beatIndex < beatMap.Count)
                {
                    BeatData bd = beatMap[ovr.beatIndex];

                    if (ovr.column.HasValue)
                        bd.column = ovr.column.Value;

                    if (!string.IsNullOrEmpty(ovr.type))
                        bd.type = Enum.TryParse<NoteType>(ovr.type, out var parsedType) ? parsedType : bd.type;

                    beatMap[ovr.beatIndex] = bd;
                }
            }

            Debug.Log("Applied " + overrides.Count + " overrides.");
        }
        else
        {
            Debug.Log("No overrides found for this level.");
        }
    }
    public void StartSong()
    {
        audioSource.Play();
        songStarted = true;
    }
    private IEnumerator BeatLoop(List<double> beatTimes)
    {
        int beatIndex = 0;

        while (beatIndex < beatTimes.Count)
        {
            double songTime = GetAudioSourceTime();
            double nextBeat = beatTimes[beatIndex];

            if (songTime >= nextBeat)
            {
                OnBeat?.Invoke(); //Disparar beat global
                beatIndex++;
            }

            yield return null;
        }
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
    public BoxLogic[,] ConvertTo2DArray(BoxLogic[] flat)
    {
        int row = 5;
        int col = 4;
        BoxLogic[,] Gen2d = new BoxLogic[row, col];
        int index = 0;
        for (int c = 0; c < col; c++)
        {
            for (int r = 0; r < row; r++)
            {
                Gen2d[r, c] = flat[index];
                index++;
            }
        }
        return Gen2d;
    }
}
