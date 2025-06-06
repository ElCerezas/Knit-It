using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ButtonManagerLevel : MonoBehaviour
{
    private float check;
    private void Start()
    {
        check = PlayerPrefs.GetFloat("Credits", 0);
    }
    public void OnLoadScene(string scene)
    {
        if (scene != null)
        {
            SceneController.Instance.LoadSceneAsync(scene);
        }

    }
    public void LoadMenuOrNot()
    {
            if (check == 0 && PlayerPrefs.GetInt("Score3") > 0)
            {
                PlayerPrefs.SetFloat("Credits", 1);
                if(GameManager.Instance.currentState == GameManager.GameState.Paused)
                {
                    GameManager.Instance.PauseResumeGame();
                }
                SceneController.Instance.LoadSceneAsync("Credits");
            }
            else
            {
                SceneController.Instance.LoadSceneAsync("Menu");
            }
    }
    public void OnRestartLevel()
    {
        SceneController.Instance.ReloadCurrentScene();
    }
}
