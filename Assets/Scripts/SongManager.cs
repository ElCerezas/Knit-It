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
    public float songDelaySeconds;
    public string fileLocation;

    public static MidiFile midiFile;

    [SerializeField] BoxLogic[] flatArray;
    public BoxLogic[,] boxGrid;

    public static event Action OnBeat, OnHalfBeat;

    private List<BeatData> beatMap = new List<BeatData>();
    private List<double> beatTimes = new List<double>();
    private TempoMap tempoMap;
    private bool songStarted = false;
    public float BPM = 120f;
    [SerializeField] int spawnOffsetBeats = 5;

    private double dspStartTime;
    private float newVolume = 0f;
    private bool checkForSound = false;

    int beatIndex = 0;
    int noteIndex = 0;
    [SerializeField] double beatThreshold = 0.1;
    double nextBeatTime = 0;
    double lastBeatTime;
    double beatInterval;

    // Half-beat control
    private double nextHalfBeatTime = 0;
    private bool wasHalfBeat = false;

    private bool wasPaused = false;
    private double pauseStartDSPTime = 0;

    private void Awake() => Instance = this;

    void Start()
    {
        newVolume = SoundManager.Instance.GetCategoryVolume(SoundManager.SoundCategory.Music);
        audioSource.volume = newVolume;

        boxGrid = ConvertTo2DArray(flatArray);
        string midiPath = Path.Combine(Application.streamingAssetsPath, fileLocation + ".mid");
        midiFile = MidiFile.Read(midiPath);
        GetDataFromMidi();

        beatInterval = 60.0 / BPM;
    }

    void Update()
    {
        if (!songStarted) return;

        if (GameManager.Instance.currentState == GameState.Paused)
        {
            if (!wasPaused)
            {
                wasPaused = true;
                pauseStartDSPTime = AudioSettings.dspTime;
            }
            return;
        }

        if (wasPaused && GameManager.Instance.currentState == GameState.Playing)
        {
            double pauseDuration = AudioSettings.dspTime - pauseStartDSPTime;
            nextBeatTime += pauseDuration;
            lastBeatTime += pauseDuration;
            nextHalfBeatTime += pauseDuration;
            dspStartTime += pauseDuration;
            wasPaused = false;
        }

        if (checkForSound)
        {
            newVolume = SoundManager.Instance.GetCategoryVolume(SoundManager.SoundCategory.Music);
            audioSource.volume = newVolume;
            checkForSound = false;
        }

        double currentDSPTime = AudioSettings.dspTime;

        //HALF BEAT
        if (currentDSPTime >= nextHalfBeatTime - (beatThreshold / 2f))
        {
            OnHalfBeat?.Invoke();
            nextHalfBeatTime += beatInterval;
        }

        //BEAT LOOP
        if (beatIndex < beatTimes.Count && audioSource.isPlaying)
        {
            if (currentDSPTime >= nextBeatTime - beatThreshold)
            {
                OnBeat?.Invoke();

                while (noteIndex < beatMap.Count &&
                       beatIndex + spawnOffsetBeats < beatTimes.Count &&
                       Mathf.Approximately((float)beatMap[noteIndex].time, (float)beatTimes[beatIndex + spawnOffsetBeats]))
                {
                    int col = beatMap[noteIndex].column;
                    NoteType type = beatMap[noteIndex].type;
                    boxGrid[0, col].SpawnNote(type, beatIndex, col);
                    noteIndex++;
                }

                beatIndex++;
                lastBeatTime = currentDSPTime;
                nextBeatTime += beatInterval;
            }
        }

        if (beatIndex >= beatTimes.Count && songStarted && !audioSource.isPlaying)
        {
            songStarted = false;
            ScoreSongManager.Instance.CheckGameWin();
        }
    }

    public void GetDataFromMidi()
    {
        beatInterval = 60.0 / BPM;
        tempoMap = midiFile.GetTempoMap();
        var notes = midiFile.GetNotes();

        beatTimes = new List<double>();
        beatMap = new List<BeatData>();

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

        for (int i = 0; i < beatMap.Count; i++)
        {
            double closest = beatTimes.OrderBy(bt => Math.Abs(bt - beatMap[i].time)).First();
            beatMap[i] = new BeatData
            {
                time = closest,
                column = beatMap[i].column,
                type = beatMap[i].type
            };
        }

        LoadOverrides();
        StartCoroutine(WaitAndStartSong(songDelaySeconds));
    }

    private IEnumerator WaitAndStartSong(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        StartSong();
    }

    private void LoadOverrides()
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileLocation + "_overrides.json");
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        try
        {
            BeatOverrideList wrapper = JsonUtility.FromJson<BeatOverrideList>(json);
            foreach (var ovr in wrapper.items)
            {
                if (!string.IsNullOrEmpty(ovr.type))
                {
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
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error parsing JSON overrides: " + ex.Message);
        }
    }

    public void StartSong()
    {
        dspStartTime = AudioSettings.dspTime;
        audioSource.Play();
        songStarted = true;

        nextBeatTime = dspStartTime;
        nextHalfBeatTime = dspStartTime + (beatInterval / 2.0);
        lastBeatTime = dspStartTime - beatInterval;
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
                gen2D[r, c] = flat[index];
                index++;
            }
        }

        return gen2D;
    }
}
