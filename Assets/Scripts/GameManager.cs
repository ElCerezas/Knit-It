using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { Playing, Paused, GameOver, Victory, Countdown }
    public GameState currentState = GameState.Playing;

    public delegate void UpdateState();
    public static event UpdateState retry;

    public Image blockingPanel;
    public Button returnMenu;
    public Button restartMenu;
    public Canvas pauseMenu;
    public GameObject noteTuto;
    public GameObject pause;
    public TMP_Text countdownText; 
    public float countdownTime = 3f;
    private bool check;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        check = false;
    }

    private void Update()
    {
        if ((currentState == GameState.Playing || currentState == GameState.Paused) && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResumeGame();
        }
    }

    public void ReturnButtonCheck()
    {
        check = true;
    }
    public void StartPlaying()
    {
        check = false;
    }
    public void SetGameState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                AudioListener.pause = false;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                break;
        }
    }
    public void PauseResumeGame()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentState == GameState.Playing)
        {
            SetGameState(GameState.Paused);
            DeactivateParallax();
            noteTuto.SetActive(false);
            pause.SetActive(true);
            pauseMenu.gameObject.SetActive(true);
            if (sceneName != "Title" && sceneName != "Menu" && sceneName != "Credits")
            {
                returnMenu.gameObject.SetActive(true);
                restartMenu.gameObject.SetActive(true);
            }
            else
            {
                returnMenu.gameObject.SetActive(false);
                restartMenu.gameObject.SetActive(false);
            }
        }
        else if (currentState == GameState.Paused)
        {
            if (sceneName != "Title" && sceneName != "Menu" && sceneName != "Credits" && check == false)
            {
                pauseMenu.gameObject.SetActive(false);
                StartCoroutine(CountdownBeforeResume());
            }
            else
            {
                SetGameState(GameState.Playing);
                ActivateParallax();
                pauseMenu.gameObject.SetActive(false);
            }
        }
    }
    public void GameOver()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
    }
    public void victory()
    {
        currentState = GameState.Victory;
    }
    public void retryGame()
    {
        currentState = GameState.Playing;
        retry?.Invoke();
        Time.timeScale = 1f;
    }
    private IEnumerator CountdownBeforeResume()
    {
        
        countdownText.gameObject.SetActive(true);
        blockingPanel.gameObject.SetActive(true);
        float timeLeft = countdownTime;

        while (timeLeft > 0)
        {
            countdownText.text = Mathf.Ceil(timeLeft).ToString();

            countdownText.transform.localScale = Vector3.one * 2f;

            float t = 0f;
            while (t < 0.5f)
            {
                t += Time.unscaledDeltaTime;
                float scale = Mathf.Lerp(2f, 1f, t / 0.5f);
                countdownText.transform.localScale = Vector3.one * scale;
                yield return null;
            }

            yield return new WaitForSecondsRealtime(0.5f);
            timeLeft--;
        }

        countdownText.gameObject.SetActive(false);
        blockingPanel.gameObject.SetActive(false);
        SetGameState(GameState.Playing);
    }
    void ActivateParallax()
    {
        ParallaxLayer[] layers = FindObjectsOfType<ParallaxLayer>();
        foreach (ParallaxLayer layer in layers)
        {
            layer.SetMove(true);
        }
    }
    void DeactivateParallax()
    {
        ParallaxLayer[] layers = FindObjectsOfType<ParallaxLayer>();
        foreach (ParallaxLayer layer in layers)
        {
            layer.SetMove(false);
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && currentState == GameState.Playing)
        {
            PauseResumeGame();
        }
    }
}
