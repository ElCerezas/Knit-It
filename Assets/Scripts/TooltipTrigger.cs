using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string mensajeTooltip;
    private bool check;

    private void Start()
    {
        check = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!check)
        {
            TooltipUI.Instance.MostrarTooltip(mensajeTooltip);
            check = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (check)
        {
            TooltipUI.Instance.OcultarTooltip();
            check = false;
        }
    }
}
