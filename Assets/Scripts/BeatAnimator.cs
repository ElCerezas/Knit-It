using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BeatTimedAnimator : MonoBehaviour
{
    public float animationDurationInBeats = 1f; // Cuántos beats debe durar 1 ciclo de animación

    private Animator animator;
    private bool hasStarted = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0f;

        SongManager.OnBeat += StartAnim;
    }

    private void OnDisable()
    {
        SongManager.OnBeat -= StartAnim;
    }

    private void StartAnim()
    {
        if (hasStarted) return; // Solo se activa una vez
        hasStarted = true;

        float bpm = SongManager.Instance.BPM;
        float beatDuration = 60f / bpm;

        // Ajustar la velocidad para que el ciclo dure X beats
        animator.speed = animationDurationInBeats / beatDuration;

        animator.Play(0, 0, 0f); // Empezar desde el principio
    }
}
