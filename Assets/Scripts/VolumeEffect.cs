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

    [SerializeField] Volume ErrorVolume, NiceVolume, PerfectVolume;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        beatInterval = 60f / (SongManager.Instance.BPM);

        ErrorVolume.weight = 0;
        NiceVolume.weight = 0;
        PerfectVolume.weight = 0;
    }

    public void TriggerErrorEffect(bool isDespawned)
    {
        SoundManager.Instance.PlaySound("NoteWrong");
        StartCoroutine(PulseVolume(ErrorVolume));
    }

    public void TriggerHitEffect(bool isPerfect)
    {
        SoundManager.Instance.PlaySound("Right");
        StartCoroutine(PulseVolume(isPerfect ? PerfectVolume : NiceVolume));
    }

    private IEnumerator PulseVolume(Volume targetVolume)
    {
        float elapsed = 0f;
        float half = beatInterval / 2f;

        // Parte ascendente
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            float weight = Mathf.Lerp(maxDis, minDis, elapsed / half);
            targetVolume.weight = weight;
            yield return null;
        }

        // Parte descendente
        elapsed = 0f;
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            float weight = Mathf.Lerp(minDis, maxDis, elapsed / half);
            targetVolume.weight = weight;
            yield return null;
        }

        targetVolume.weight = maxDis; // Reset
    }
}
