using UnityEngine;
using UnityEngine.UI;

public class ButtonSetup : MonoBehaviour
{
    public string sceneName;
    public bool hasQuit = false;
    public bool hasFade = false;

    void Start()
    {
        Button boton = GetComponent<Button>();

        if (SceneController.Instance != null)
        {
            if (hasQuit)
            {
                boton.onClick.AddListener(() => SceneController.Instance.QuitGame());
            }
            else
            {
                if (!hasFade)
                    boton.onClick.AddListener(() => SceneController.Instance.LoadScene(sceneName));
                else boton.onClick.AddListener(() => SceneController.Instance.LoadSceneAsync(sceneName));
            }
            boton.onClick.AddListener(GameManager.Instance.StartPlaying);
        }
        else
        {
        }
    }
}
