using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    private int score1;
    private int score2;
    private int score3;
    public bool lvl2Unlocked;
    public bool lvl3Unlocked;
    [SerializeField] Button level2;
    [SerializeField] Button level3;

    public float scoreForThreeStars1;
    public float scoreForThreeStars2;
    public float scoreForThreeStars3;
    public Sprite trophySimple1;
    public Sprite trophyPro1;
    public SpriteRenderer newRenderer1;
    public Sprite trophySimple2;
    public Sprite trophyPro2;
    public SpriteRenderer newRenderer2;
    public Sprite trophySimple3;
    public Sprite trophyPro3;
    public SpriteRenderer newRenderer3;
    public bool Test = false;
    Color shadowColor;
    Color basicColor;

    private void Start()
    {
        scoreForThreeStars1 = 1000;
        scoreForThreeStars2 = 2000;
        scoreForThreeStars3 = 3000;
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
    private void Awake()
    {
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
            }
            else if (score1 >= scoreForThreeStars1)
            {
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
            }
            else if (score1 >= scoreForThreeStars2)
            {
                newRenderer2.sprite = trophyPro2;
                newRenderer2.color = basicColor;
            }
        }
    }
    public void Trophy3Unlocked()
    {
        if (score3 <= 0)
        {
            newRenderer3.sprite = trophySimple3;
            newRenderer3.color = shadowColor;
        }
        else
        {
            if (score3 < scoreForThreeStars3)
            {
                newRenderer3.sprite = trophySimple3;
                newRenderer3.color = basicColor;
            }
            else if (score3 >= scoreForThreeStars3)
            {
                newRenderer3.sprite = trophyPro3;
                newRenderer3.color = basicColor;
            }
        }
    }
}
