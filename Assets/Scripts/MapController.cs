using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [Header("Niveles")]
    private int score1;
    private int score2;
    private int score3;
    public bool lvl2Unlocked;
    public bool lvl3Unlocked;
    [SerializeField] Button level2;
    [SerializeField] Button level3;

    [Header("Puntuacion Máxima")]
    public float scoreForThreeStars1;
    public float scoreForThreeStars2;
    public float scoreForThreeStars3;

    [Header("Trofeos")]
    public Sprite trophySimple1;
    public Sprite trophyPro1;
    public SpriteRenderer newRenderer1;
    public Sprite trophySimple2;
    public Sprite trophyPro2;
    public SpriteRenderer newRenderer2;
    public Sprite trophySimple3;
    public Sprite trophyPro3;
    public SpriteRenderer newRenderer3;
    Color shadowColor;
    Color basicColor;

    [Header("Ovillos")]
    Color bronze;
    Color silver;
    Color gold;
    public Image ovilloTemplate1;
    public Image ovilloTemplate2;
    public Image ovilloTemplate3;

    private void Start()
    {
        if (SoundManager.Instance != null) 
            if(SoundManager.Instance.firstStart == false)
            {
                SoundManager.Instance.PlaySound("MainMenu", true);
            }
            else
            {
                SoundManager.Instance.firstStart = false;
            }
        bronze = new Color(0.8f, 0.5f, 0.2f, 1f);
        silver = new Color(0.75f, 0.75f, 0.75f, 1f);
        gold = new Color(1.0f, 0.84f, 0.0f, 1f);
        shadowColor = Color.black;
        shadowColor.a = 0.5f;
        basicColor = Color.white;
        basicColor.a = 1f;
        score1 = PlayerPrefs.GetInt("Score1", 0);
        score2 = PlayerPrefs.GetInt("Score2", 0);
        score3 = PlayerPrefs.GetInt("Score3", 0);
        Level2Unlocked();
        Level3Unlocked();
        Trophy3Unlocked();
    }
    public void Level2Unlocked()
    {
        if (score1 <= 0)
        {
            lvl2Unlocked = false;
            level2.interactable = false;
            ovilloTemplate1.color = shadowColor;
            newRenderer1.sprite = trophySimple1;
            newRenderer1.color = shadowColor;
        }
        else
        {
            lvl2Unlocked = true;
            level2.interactable = true;
            if (score1 < scoreForThreeStars1)
            {
                newRenderer1.sprite = trophySimple1;
                newRenderer1.color = basicColor;
                if((score1 >= scoreForThreeStars1 / 3f && score1 < (2f * scoreForThreeStars1) / 3f))
                {
                    ovilloTemplate1.color = bronze;
                }
                else if (score1 >= (2f * scoreForThreeStars1) / 3f && score1 < scoreForThreeStars1)
                {
                    ovilloTemplate1.color = silver;
                }
            }
            else if (score1 >= scoreForThreeStars1)
            {
                ovilloTemplate1.color = gold;
                newRenderer1.sprite = trophyPro1;
                newRenderer1.color = basicColor;
            }
        }
    }
    public void Level3Unlocked()
    {
        if (score2 <= 0)
        {
            lvl3Unlocked = false;
            level3.interactable = false;
            ovilloTemplate2.color = shadowColor;
            newRenderer2.sprite = trophySimple2;
            newRenderer2.color = shadowColor;
        }
        else
        {
            lvl3Unlocked = true;
            level3.interactable = true;
            if (score2 < scoreForThreeStars2)
            {
                newRenderer2.sprite = trophySimple2;
                newRenderer2.color = basicColor;
                if ((score2 >= scoreForThreeStars2 / 3f && score2 < (2f * scoreForThreeStars2) / 3f))
                {
                    ovilloTemplate2.color = bronze;
                }
                else if(score2 >= (2f * scoreForThreeStars2) / 3f && score2 < scoreForThreeStars2)
                {
                    ovilloTemplate2.color = silver;
                }
            }
            else if (score2 >= scoreForThreeStars2)
            {
                ovilloTemplate2.color = gold;
                newRenderer2.sprite = trophyPro2;
                newRenderer2.color = basicColor;
            }
        }
    }
    public void Trophy3Unlocked()
    {
        if (score3 <= 0)
        {
            ovilloTemplate3.color = shadowColor;
            newRenderer3.sprite = trophySimple3;
            newRenderer3.color = shadowColor;
        }
        else
        {
            if (score3 < scoreForThreeStars3)
            {
                newRenderer3.sprite = trophySimple3;
                newRenderer3.color = basicColor;
                if ((score3 >= scoreForThreeStars3 / 3f && score3 < (2f * scoreForThreeStars3) / 3f))
                {
                    ovilloTemplate3.color = bronze;
                }
                else if(score3 >= (2f * scoreForThreeStars3) / 3f && score3 < scoreForThreeStars3)
                {
                    ovilloTemplate3.color = silver;
                }
            }
            else if (score3 >= scoreForThreeStars3)
            {
                ovilloTemplate3.color = gold;
                newRenderer3.sprite = trophyPro3;
                newRenderer3.color = basicColor;
            }
        }
    }
}
