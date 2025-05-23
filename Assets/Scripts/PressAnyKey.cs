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

        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.I))
            {
                PlayerPrefs.DeleteAll();
            }
            pressed = true;
            //SoundManager.Instance.PlaySound("EnterGame");
            SceneController.Instance.LoadScene("Menu");
        }
    }

}
