using UnityEngine;
public class BeatSync : MonoBehaviour
{
    public AudioSource audioSource;
    public float bpm = 100f;         // BPM de la canción
    private double nextBeatTime;     // próximo tiempo de beat (en dspTime)
    private double beatInterval;     // segundos por beat

    void Start()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        beatInterval = 60.0 / bpm;                  // 0.6 s por beat
        double dspStartTime = AudioSettings.dspTime + 0.5; // buffer inicial de 0.5s
        audioSource.PlayScheduled(dspStartTime);   // reproducir en dsp exacto
        nextBeatTime = dspStartTime;               // primer beat en inicio
    }

    void Update()
    {
        double dspNow = AudioSettings.dspTime;
        // Mientras el tiempo actual supere el siguiente beat, se dispara evento
        while (dspNow >= nextBeatTime)
        {
            OnBeat();                              // Acción al ocurrir el beat
            nextBeatTime += beatInterval;          // programar siguiente beat
        }
    }

    void OnBeat()
    {
        // Lógica de evento en beat (puede ser emitir sonido, spawnear nota, etc.)
        Debug.Log("¡Beat! " + Time.unscaledTime);
    }
}
