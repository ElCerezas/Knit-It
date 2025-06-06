using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class PressAnyKey : MonoBehaviour
{
    private bool canCheckInput = false;
    bool pressed = false;

    IEnumerator Start()
    {
        yield return null; // Espera un frame
        SoundManager.Instance.PlaySound("MainMenu", true);
        SoundManager.Instance.firstStart = true;
        canCheckInput = true;
    }

    void Update()
    {
        if (GameManager.Instance.currentState == GameState.Playing)
        {
            if (!canCheckInput || pressed) return;

            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key) && key != KeyCode.Escape)
                {
                    if (Input.GetKey(KeyCode.I))
                    {
                        PlayerPrefs.DeleteAll();
                    }
                    pressed = true;
                    SoundManager.Instance.PlaySound("EnterGame");
                    SceneController.Instance.LoadSceneAsync("Menu");
                    break;
                }
            }
        }
    }

}
