using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BeatTimedAnimator : MonoBehaviour
{
    public float animationDuration = 1f; // Duración real del clip en segundos

    private Animator animator;

    void Start()
    {
        float bpm = SongManager.Instance.BPM;
        animator = GetComponent<Animator>();

        // Calcular la duración de un beat en segundos
        float beatDuration = 60f / bpm;

        // Calcular la velocidad del Animator para ajustar el tiempo
        float speed = animationDuration / beatDuration;
        animator.speed = speed;
    }
}
