using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour
{
    private bool canCheckInput = false;
    bool pressed = false;

    IEnumerator Start()
    {
        yield return null; // Espera un frame
        canCheckInput = true;
    }

    void Update()
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
