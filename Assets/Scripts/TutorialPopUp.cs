using System.Collections;
using UnityEngine;
using UnityEngine.Video; // Asegúrate de tener esto

public class TutorialPopUp : MonoBehaviour
{
    [SerializeField] private GameObject dropdownPanel, Cinematic1;
    [SerializeField] private Animator otherAnim;
    private VideoPlayer videoPlayer;
    private float check;
    private bool stopRepeat;

    void Start()
    {
        CozyCameraLook cameraLook = FindObjectOfType<CozyCameraLook>();
        cameraLook.SetCameraMove(true);

        if (dropdownPanel != null)
            dropdownPanel.SetActive(false);

        if (Cinematic1 != null)
            Cinematic1.SetActive(false);

        check = PlayerPrefs.GetFloat("Tutorial", 0);
        stopRepeat = false;

        if (check == 0)
        {
            DeactivateParallax();
            cameraLook.SetCameraMove(false);
            SoundManager.Instance.PlaySound("Paper");

            if (Cinematic1 != null)
            {
                Cinematic1.SetActive(true);
                videoPlayer = Cinematic1.GetComponent<VideoPlayer>();
                if (videoPlayer != null)
                {
                    videoPlayer.loopPointReached += OnVideoFinished;
                    videoPlayer.Play();
                }
                else
                {
                    Debug.LogError("VideoPlayer no encontrado en Cinematic1");
                    ShowDropdownPanel(); // fallback
                }
            }
            else
            {
                ShowDropdownPanel(); // fallback
            }
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Cinematic1.SetActive(false);
        ShowDropdownPanel();
    }

    void ShowDropdownPanel()
    {
        dropdownPanel.SetActive(true);
    }

    public void HidePopUp()
    {
        if (!stopRepeat)
        {
            CozyCameraLook cameraLook = FindObjectOfType<CozyCameraLook>();
            cameraLook.SetCameraMove(true);
            PlayerPrefs.SetFloat("Tutorial", 1);
            SoundManager.Instance.PlaySound("Paper");
            otherAnim.SetTrigger("Out");
            stopRepeat = true;
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        dropdownPanel.SetActive(false);
        ActivateParallax();
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
}
