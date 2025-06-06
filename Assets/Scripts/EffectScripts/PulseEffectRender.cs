using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PulseEffectRender : MonoBehaviour
{
    [Header("Timing")]
    public float maxDis = 1;
    public float minDis = 0;

    private float beatInterval;
    private float disIntensity = 0;
    [SerializeField] Volume vc;

    private void Start()
    {
        beatInterval = 60f / (SongManager.Instance.BPM);
        SongManager.OnBeat += TriggerPulse;
    }

    private void OnDisable()
    {
        SongManager.OnBeat -= TriggerPulse;
    }

    void Update()
    {
        vc.weight = disIntensity;
    }

    void TriggerPulse()
    {
        StopAllCoroutines(); // Reinicia si el beat se dispara de nuevo
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

    public void ShutDown()
    {
        SongManager.OnBeat -= TriggerPulse;
    }
}
