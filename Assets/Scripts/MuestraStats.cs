using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MuestraStats : MonoBehaviour
{
    [SerializeField] private GameObject dropdownPanel;
    private int finalScore = 0;
    [SerializeField] private Animation otherAnim;
    [SerializeField] private Image placeholderImage;
    [SerializeField] private Sprite iaia1;
    [SerializeField] private Sprite shadowIaia1;
    [SerializeField] private Sprite iaia2;
    [SerializeField] private Sprite shadowIaia2;
    [SerializeField] private Sprite iaia3;
    [SerializeField] private Sprite shadowIaia3;
    [SerializeField] private TextMeshProUGUI HighScore;
    [Header("PlayButton")]
    public ButtonSetup playButton;
    [SerializeField] private string nivel1;
    [SerializeField] private string nivel2;
    [SerializeField] private string nivel3;
    void Start()
    {
        if (dropdownPanel != null)
            dropdownPanel.SetActive(false);
    }

    public void ToggleDropdown1()
    {
        SoundManager.Instance.PlaySound("Paper");
        dropdownPanel.SetActive(true);
        playButton.sceneName = nivel1;
        finalScore = PlayerPrefs.GetInt("Score1");
        HighScore.text = finalScore.ToString();
        if (finalScore == 0)
        {
            placeholderImage.sprite = shadowIaia1;
        }
        else
        {
            placeholderImage.sprite = iaia1;
        }
    }
    public void ToggleDropdown2()
    {
        SoundManager.Instance.PlaySound("Paper");
        dropdownPanel.SetActive(true);
        playButton.sceneName = nivel2;
        finalScore = PlayerPrefs.GetInt("Score2");
        HighScore.text = finalScore.ToString();
        if (finalScore == 0)
        {
            placeholderImage.sprite = shadowIaia2;
        }
        else
        {
            placeholderImage.sprite = iaia2;
        }
    }
    public void ToggleDropdown3()
    {
        SoundManager.Instance.PlaySound("Paper");
        dropdownPanel.SetActive(true);
        playButton.sceneName = nivel3;
        finalScore = PlayerPrefs.GetInt("Score3");
        HighScore.text = finalScore.ToString();
        if (finalScore == 0)
        {
            placeholderImage.sprite = shadowIaia3;
        }
        else
        {
            placeholderImage.sprite = iaia3;
        }
    }
    public void HideDropdown()
    {
        SoundManager.Instance.PlaySound("Paper");
        otherAnim.Play("PolaroidOut");
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        dropdownPanel.SetActive(false);
    }
}
