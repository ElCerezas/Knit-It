using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(currentScene.buildIndex);
    }

    public void LoadSceneAsync(string sceneName) // para pantallas de carga
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }
    private System.Collections.IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
            yield break;

        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.Fade(1f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
            yield return null;

        yield return null;

        if (ScreenFader.Instance != null)
            yield return ScreenFader.Instance.Fade(0f);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
