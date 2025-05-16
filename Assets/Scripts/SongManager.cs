using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using static GameManager;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public float songDelaySeconds = 0.5f;
    public string fileLocation;
    public static MidiFile midiFile;

    [SerializeField] BoxLogic[] flatArray;
    public BoxLogic[,] boxGrid;
    public static event Action OnBeat;

    private List<BeatData> beatMap = new();
    private List<double> beatTimes = new();
    private TempoMap tempoMap;
    private bool songStarted = false;

    public float BPM = 120f;
    private double beatInterval;
    private double dspStartTime;
    private double nextBeatTime;
    private int beatIndex = 0;
    private int noteIndex = 0;
    private double lastBeatDSP = 0;

    private float newVolume = 0f;
    private bool checkForSound = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        newVolume = SoundManager.Instance.GetCategoryVolume(SoundManager.SoundCategory.Music);
        audioSource.volume = newVolume;

        boxGrid = ConvertTo2DArray(flatArray);
        string midiPath = Path.Combine(Application.streamingAssetsPath, fileLocation + ".mid");
        midiFile = MidiFile.Read(midiPath);
        GetDataFromMidi();
    }

    void Update()
    {
        if (GameManager.Instance.currentState == GameState.Paused && !checkForSound)
            checkForSound = true;

        if (checkForSound && GameManager.Instance.currentState == GameState.Playing)
        {
            newVolume = SoundManager.Instance.GetCategoryVolume(SoundManager.SoundCategory.Music);
            audioSource.volume = newVolume;
            checkForSound = false;
        }

        if (!songStarted || GameManager.Instance.currentState == GameState.Paused)
            return;

        double dspNow = AudioSettings.dspTime;
        while (dspNow >= nextBeatTime)
        {
            double delta = nextBeatTime - lastBeatDSP;
            Debug.Log($"[{DateTime.Now:HH:mm:ss}] Beat:{beatIndex} // t desde anterior: {delta:F4}s -> desfase: {(0.6 - delta):F4}");

            OnBeat?.Invoke();

            while (noteIndex < beatMap.Count &&
                   Math.Abs(beatMap[noteIndex].time - (nextBeatTime - dspStartTime)) < 0.01)
            {
                int col = beatMap[noteIndex].column;
                NoteType type = beatMap[noteIndex].type;
                boxGrid[0, col].SpawnNote(type, beatIndex, col);
                noteIndex++;
            }

            lastBeatDSP = nextBeatTime;
            nextBeatTime += beatInterval;
            beatIndex++;
        }

        if (!audioSource.isPlaying && songStarted && GameManager.Instance.currentState != GameState.Paused)
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
        beatTimes = new();
        beatMap = new();

        foreach (var note in notes)
        {
            var metricTime = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, tempoMap);
            var beatTime = metricTime.Minutes * 60 + metricTime.Seconds + metricTime.Milliseconds / 1000f;

            if (note.NoteNumber == 67)
            {
                beatTimes.Add(beatTime);
                continue;
            }

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

        foreach (var b in beatMap)
        {
            double closest = beatTimes.OrderBy(bt => Math.Abs(bt - b.time)).First();
            SetTimeBeat(closest, b);
        }

        LoadOverrides();
        StartSong();
    }
    private void SetTimeBeat(double c, BeatData b)
    {
        b.time = c;
    }
    private void LoadOverrides()
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileLocation + "_overrides.json");
        if (!File.Exists(path)) return;

        try
        {
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<BeatOverrideList>(json);
            var overrides = wrapper.items;

            foreach (var ovr in overrides)
            {
                if (string.IsNullOrEmpty(ovr.type)) continue;

                var matches = beatMap
                    .Select((b, i) => new { Beat = b, Index = i })
                    .Where(x =>
                        Mathf.Approximately((float)x.Beat.time, (float)beatTimes[ovr.beatIndex]) &&
                        (!ovr.column.HasValue || x.Beat.column == ovr.column.Value))
                    .ToList();

                foreach (var match in matches)
                {
                    var bd = match.Beat;
                    bd.type = Enum.TryParse<NoteType>(ovr.type, out var parsedType) ? parsedType : bd.type;
                    beatMap[match.Index] = bd;
                }
            }

            Debug.Log("Applied " + overrides.Count + " overrides.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error parsing JSON overrides: " + ex.Message);
        }
    }
    public void StartSong()
    {
        beatInterval = 60.0 / BPM;
        dspStartTime = AudioSettings.dspTime + songDelaySeconds;
        nextBeatTime = dspStartTime;
        lastBeatDSP = dspStartTime - beatInterval;

        audioSource.PlayScheduled(dspStartTime);
        songStarted = true;
    }
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
    public BoxLogic[,] ConvertTo2DArray(BoxLogic[] flat)
    {
        int row = 5;
        int col = 4;
        BoxLogic[,] gen2D = new BoxLogic[row, col];
        int index = 0;
        for (int c = 0; c < col; c++)
        {
            for (int r = 0; r < row; r++)
            {
                gen2D[r, c] = flat[index++];
            }
        }
        return gen2D;
    }
}
