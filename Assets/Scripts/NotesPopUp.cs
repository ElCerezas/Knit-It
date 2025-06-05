using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesPopUp : MonoBehaviour
{
    [SerializeField] private GameObject dropdownPanel;
    [SerializeField] private Animator otherAnim;
    private bool stopRepeat;

    private void Start()
    {
        if (dropdownPanel != null)
            dropdownPanel.SetActive(false);
    }
    public void OpenPopUp()
    {
        CozyCameraLook cameraLook = FindObjectOfType<CozyCameraLook>();
        cameraLook.SetCameraMove(true);
            DeactivateParallax();
            cameraLook.SetCameraMove(false);
            SoundManager.Instance.PlaySound("Paper");
            dropdownPanel.SetActive(true);
        stopRepeat = false;
            StartCoroutine(Timer2());
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
    IEnumerator Timer2()
    {
        yield return new WaitForSeconds(1f);
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
