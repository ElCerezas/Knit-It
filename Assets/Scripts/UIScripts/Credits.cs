using UnityEngine;
using UnityEngine.Video;

public class Credits : MonoBehaviour
{
    [SerializeField] private float maxY;            // Altura máxima
    [SerializeField] private float speed = 1f;      // Velocidad de subida
    [SerializeField] private VideoPlayer videoPlayer; // Referencia al VideoPlayer

    private RectTransform rectTransform;
    private float actualSpeed;
    private bool canScroll = false;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        actualSpeed = speed;

        if (videoPlayer != null)
        {
            // Espera al final del video
            videoPlayer.loopPointReached += OnVideoFinished;
            SoundManager.Instance.PlaySound("Cheer2");
        }
        else
        {
            // Si no hay video, empieza de inmediato
            canScroll = true;
        }
    }

    private void Update()
    {
        if (canScroll && rectTransform.position.y < maxY)
        {
            transform.position += Vector3.up * actualSpeed * Time.deltaTime;
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.Pause(); // Detiene en el último frame
        canScroll = true;
    }

}
