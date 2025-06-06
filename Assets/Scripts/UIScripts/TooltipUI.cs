using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public GameObject panelTooltip;
    public TextMeshProUGUI textoTooltip;
    private void Awake()
    {
        Instance = this;
        OcultarTooltip();
    }

    void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panelTooltip.transform.parent as RectTransform,
            Input.mousePosition, null, out pos);
        panelTooltip.transform.localPosition = pos + new Vector2(40, 15); 
    }

    public void MostrarTooltip(string texto)
    {
        textoTooltip.text = texto;
        panelTooltip.SetActive(true);
    }

    public void OcultarTooltip()
    {
        panelTooltip.SetActive(false);
    }
}
