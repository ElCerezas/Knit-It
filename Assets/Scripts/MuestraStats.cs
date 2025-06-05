using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MuestraStats : MonoBehaviour
{
    [Header("FotoIaia")]
    [SerializeField] private GameObject dropdownPanel;
    private int finalScore = 0;
    [SerializeField] private Animation otherAnim;
    [SerializeField] private Animator otherAnim2;
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
    [Header("PelusasPorLevel")]
    [SerializeField] private Image pelusaPlaceholderImage1;
    [SerializeField] private Image pelusaPlaceholderImage2;
    [SerializeField] private Sprite pelusa2;
    [SerializeField] private Sprite pelusa3;
    [SerializeField] private Sprite pelusa4;
    [SerializeField] private Sprite pelusa5;
    [SerializeField] private Sprite pelusa6;
    [SerializeField] private Sprite pelusa7;
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
        pelusaPlaceholderImage1.sprite = pelusa2;
        pelusaPlaceholderImage2.sprite = pelusa3;
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
        pelusaPlaceholderImage1.sprite = pelusa4;
        pelusaPlaceholderImage2.sprite = pelusa5;
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
        pelusaPlaceholderImage1.sprite = pelusa6;
        pelusaPlaceholderImage2.sprite = pelusa7;
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
        otherAnim2.SetTrigger("Out");
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        dropdownPanel.SetActive(false);
    }
}
