using UnityEngine.EventSystems;
using UnityEngine;
public class CozyButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool inUI = false;
    public bool playButton = false;
    public float hoverScale = 1.1f;
    public float speed = 5f;
    private Vector3 originalScale;
    private bool hovering = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        Vector3 target = hovering ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySound("UI3");
        hovering = true;
    } 

    public void OnPointerExit(PointerEventData eventData) => hovering = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inUI)
        {
            SoundManager.Instance.PlaySound("UI4");
        }
        else if (playButton)
        {
            SoundManager.Instance.PlaySound("Play");
            SoundManager.Instance.StopSound("MainMenu");
        }
        else
        {
            SoundManager.Instance.PlaySound("UI2");
        }
        hovering = false;
    }
}
