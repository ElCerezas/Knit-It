using UnityEngine;
using UnityEngine.UI;

public class SliderParticleController : MonoBehaviour
{
    [Header("UI")]
    public Slider targetSlider;
    public RectTransform fillTransform; // Asigna el Fill o el Handle aquí

    [Header("Particle Systems")]
    public ParticleSystem continuousParticles;
    public ParticleSystem burst33;
    public ParticleSystem burst66;
    public ParticleSystem burst100;

    private float previousValue = 0f;

    private bool triggered33 = false;
    private bool triggered66 = false;
    private bool triggered100 = false;

    void Update()
    {
        float currentValue = targetSlider.value;

        // Posicionar las partículas en el borde del fill
        if (fillTransform != null)
        {
            Vector3 worldPos = fillTransform.position;
            continuousParticles.transform.position = worldPos;
        }

        // Activar partículas continuas si el valor sube
        if (currentValue > previousValue)
        {
            if (!continuousParticles.isPlaying)
                continuousParticles.Play();
        }
        else
        {
            if (continuousParticles.isPlaying)
                continuousParticles.Stop();
        }

        // Triggers por porcentaje
        if (!triggered33 && currentValue >= 0.33f)
        {
            burst33.Play();
            triggered33 = true;
        }

        if (!triggered66 && currentValue >= 0.66f)
        {
            burst66.Play();
            triggered66 = true;
        }

        if (!triggered100 && currentValue >= 1f)
        {
            burst100.Play();
            triggered100 = true;
        }

        previousValue = currentValue;
    }

    public void ResetBursts()
    {
        triggered33 = false;
        triggered66 = false;
        triggered100 = false;
    }
}
