using UnityEngine;
using UnityEngine.UI;

public class ButtonSetup : MonoBehaviour
{
    public string sceneName;

    void Start()
    {
        Button boton = GetComponent<Button>();

        if (SceneController.Instance != null)
        {
            boton.onClick.AddListener(() => SceneController.Instance.LoadScene(sceneName));
        }
        else
        {
        }
    }
}
