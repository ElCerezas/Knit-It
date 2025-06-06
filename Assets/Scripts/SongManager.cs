using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static GameManager;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public float songDelaySeconds;
    public string fileLocation;
    [SerializeField] AudioClip vocals, beat;

    public static event Action OnBeat, OnHalfBeat;

    public float BPM = 120f;
    public int level = 1;
    [SerializeField] int spawnOffsetBeats = 5;

    private List<double> beatTimes = new List<double>();
    private Dictionary<int, List<BeatData>> spawnMap = new();

    [SerializeField] BoxLogic[] flatArray;
    public BoxLogic[,] boxGrid;

    private bool songStarted = false;
    private double dspStartTime;
    private double beatInterval;
    private int beatIndex = 0;
    private double nextBeatTime = 0;
    private double nextHalfBeatTime = 0;
    private double lastBeatTime = 0;
    [SerializeField] double beatThreshold = 0.1f;

    private bool wasPaused = false;
    private double pauseStartDSPTime = 0;

    private void Awake() => Instance = this;

    void Start()
    {
        audioSource.volume = SoundManager.Instance.GetCategoryVolume(SoundManager.SoundCategory.Music);
        boxGrid = ConvertTo2DArray(flatArray);
        beatInterval = 60.0 / BPM;
        if (PlayerPrefs.GetInt("Vocals", 1) == 1)
        {
            audioSource.clip = vocals;
        }
        else
        {
            audioSource.clip = beat;
        }
        LoadJSONNotes();
        StartCoroutine(WaitAndStartSong(songDelaySeconds));
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
            nextHalfBeatTime += pauseDuration;
            dspStartTime += pauseDuration;
            wasPaused = false;
        }

        double currentDSPTime = AudioSettings.dspTime;

        if (currentDSPTime >= nextHalfBeatTime - (beatThreshold / 2.0))
        {
            OnHalfBeat?.Invoke();
            nextHalfBeatTime += beatInterval;
        }

        if (currentDSPTime >= nextBeatTime - beatThreshold)
        {
            OnBeat?.Invoke();

            if (spawnMap.TryGetValue(beatIndex, out var notes))
            {
                int spawnRow = boxGrid.GetLength(0) - 1;

                foreach (var bd in notes)
                {
                    if (bd.column >= 0 && bd.column < boxGrid.GetLength(1))
                    {
                        boxGrid[0, bd.column].SpawnNote(bd.type, beatIndex, bd.column);
                    }
                    else
                    {
                        Debug.LogWarning($"[SongManager] Columna inválida: {bd.column}");
                    }
                }
            }

            beatIndex++;
            lastBeatTime = currentDSPTime;
            nextBeatTime += beatInterval;
        }

        if (songStarted && !audioSource.isPlaying && GameManager.Instance.currentState == GameState.Playing)
        {
            songStarted = false;
            ScoreSongManager.Instance.CheckGameWin();
        }
    }

    void LoadJSONNotes()
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileLocation + "_overrides.json");
        if (!File.Exists(path))
        {
            Debug.LogError("JSON de notas no encontrado en: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        BeatOverrideList wrapper = JsonUtility.FromJson<BeatOverrideList>(json);

        spawnMap.Clear();
        beatTimes.Clear();

        foreach (var ovr in wrapper.items)
        {
            if (!Enum.TryParse(ovr.type, out NoteType parsedType)) continue;

            int beat = ovr.beatIndex - spawnOffsetBeats;
            if (beat < 0) continue;

            if (!spawnMap.ContainsKey(beat))
                spawnMap[beat] = new List<BeatData>();

            spawnMap[beat].Add(new BeatData
            {
                time = beat * beatInterval,
                column = ovr.column,
                type = parsedType
            });
        }
    }

    private IEnumerator WaitAndStartSong(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        StartSong();
    }

    public void StartSong()
    {
        dspStartTime = AudioSettings.dspTime;
        audioSource.Play();
        songStarted = true;

        nextBeatTime = dspStartTime + beatInterval;
        nextHalfBeatTime = dspStartTime + (beatInterval / 2.0);
        lastBeatTime = dspStartTime;
    }

    public BoxLogic[,] ConvertTo2DArray(BoxLogic[] flat)
    {
        int row = 5;
        int col = 4;
        BoxLogic[,] grid = new BoxLogic[row, col];
        int index = 0;

        for (int c = 0; c < col; c++)
        {
            for (int r = 0; r < row; r++)
            {
                grid[r, c] = flat[index];
                index++;
            }
        }

        return grid;
    }
}
