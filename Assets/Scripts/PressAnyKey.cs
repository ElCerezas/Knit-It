using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour
{
    public bool pressed = false;
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                return;
            if (Input.GetKeyDown(KeyCode.Escape))
                return;
            pressed = true;
            SoundManager.Instance.PlaySound("EnterGame");
            SceneController.Instance.LoadSceneAsync("Menu");
        }
    }
}
