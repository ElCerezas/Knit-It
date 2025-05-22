using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp : MonoBehaviour
{
    [SerializeField] private GameObject dropdownPanel;
    [SerializeField] private Animator otherAnim;
    private float check;
    private bool stopRepeat;
    void Start()
    {
        CozyCameraLook cameraLook = FindObjectOfType<CozyCameraLook>();
        cameraLook.SetCameraMove(true);
        if (dropdownPanel != null)
            dropdownPanel.SetActive(false);
        check = PlayerPrefs.GetFloat("Tutorial",0);
        stopRepeat = false;
        if (check == 0)
        {
            DeactivateParallax();
            cameraLook.SetCameraMove(false);
            SoundManager.Instance.PlaySound("Paper");
            dropdownPanel.SetActive(true);
            StartCoroutine(Timer2());
        }
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
