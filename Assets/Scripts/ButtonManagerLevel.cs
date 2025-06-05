using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManagerLevel : MonoBehaviour
{
    public void OnLoadScene(string scene)
    {
        if (scene != null)
        {
            SceneController.Instance.LoadScene(scene);
        }

    }
    public void OnRestartLevel()
    {
        SceneController.Instance.ReloadCurrentScene();
    }
}
