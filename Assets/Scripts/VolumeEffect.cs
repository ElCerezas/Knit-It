using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeEffect : MonoBehaviour
{
    [Header("Timing")]
    public float maxDis = 0;
    public float minDis = 1;

    public static VolumeEffect Instance;

    private float beatInterval;
    private float disIntensity = 0;
    [SerializeField] Volume ErrorVolume, NiceVolume, PerfectVolume;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        beatInterval = 60f / (SongManager.Instance.BPM);
    }

    void Update()
    {
        ErrorVolume.weight = disIntensity;
    }

    public void TriggerErrorEffect(bool isDespawned)
    {
        if (!isDespawned)
        {
            SoundManager.Instance.PlaySound("NoteWrong");
            //PulseError Volume
            StartCoroutine(PulseRoutine());
        }
        else
        {
            SoundManager.Instance.PlaySound("NoteWrong");
            //PulseError Volume
        }
        
    }
    public void TriggerHit(bool isPerfect)
    {
        if (!isPerfect)
        {
            SoundManager.Instance.PlaySound("Right");
            //PulseNice Volume
        }
        else
        {
            SoundManager.Instance.PlaySound("Right");
            //PulsePerfect Volume
        }
        StartCoroutine(PulseRoutine());
    }

    IEnumerator PulseRoutine()
    {
        float elapsed = 0f;
        float half = beatInterval / 2f;

        // Parte ascendente: sube hasta max
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            disIntensity = Mathf.Lerp(maxDis, minDis, elapsed / half);
            yield return null;
        }

        // Parte descendente: baja de nuevo hasta max
        elapsed = 0f;
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            disIntensity = Mathf.Lerp(minDis, maxDis, elapsed / half);
            yield return null;
        }
    }
}
