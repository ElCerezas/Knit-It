using UnityEngine;
using UnityEngine.UI;

public class ButtonSetup : MonoBehaviour
{
    public string sceneName;
    public bool hasFade = false;

    void Start()
    {
        Button boton = GetComponent<Button>();

        if (SceneController.Instance != null)
        {
            if(!hasFade)
            boton.onClick.AddListener(() => SceneController.Instance.LoadScene(sceneName));
            else boton.onClick.AddListener(() => SceneController.Instance.LoadSceneAsync(sceneName));
        }
        else
        {
        }
    }
}
