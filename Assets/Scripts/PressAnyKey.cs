using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour
{
    public bool pressed = false;
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                return;
            if (Input.GetKey(KeyCode.I))
            {
                PlayerPrefs.DeleteAll();
            }
            pressed = true;
            SoundManager.Instance.PlaySound("EnterGame");
            SceneController.Instance.LoadScene("Menu");
        }
    }
}
