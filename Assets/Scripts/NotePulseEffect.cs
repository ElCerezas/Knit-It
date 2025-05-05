using System.Collections;
using UnityEngine;

public class NotePulseEffect : MonoBehaviour
{
    [Header("Timing")]
    public float pulseFactor = 1.1f;

    private float beatInterval;         // Tiempo entre beats
    private bool pulsing = false;
    private float pulseStrength = 0f;

    private Vector3 baseScale = Vector3.one;


    private void OnEnable()
    {
        beatInterval = 60f / SongManager.Instance.BPM;
        SongManager.OnBeat += TriggerPulse;
    }

    private void OnDisable()
    {
        SongManager.OnBeat -= TriggerPulse;
    }
    void Update()
    {
        // Este valor lo modifica NoteScalerLogic:
        float yScale = transform.localScale.y;
        float scaleFromPerspective = yScale / baseScale.y;

        // Aplicamos el efecto de pulso multiplicativamente sobre la escala original
        float finalScale = scaleFromPerspective * Mathf.Lerp(1f, pulseFactor, pulseStrength);
        transform.localScale = new Vector3(finalScale, finalScale, 1f);
    }
    void TriggerPulse()
    {
        if (!pulsing)
            StartCoroutine(PulseRoutine());
    }
    IEnumerator PulseRoutine()
    {
        pulsing = true;
        pulseStrength = 1f;

        float elapsed = 0f;
        while (elapsed < beatInterval)
        {
            elapsed += Time.deltaTime;
            pulseStrength = Mathf.Lerp(1f, 0f, elapsed / beatInterval);
            yield return null;
        }

        pulseStrength = 0f;
        pulsing = false;
    }
}
