using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {
        score1 = PlayerPrefs.GetInt("MaxScoreLevel1");
        score2 = PlayerPrefs.GetInt("MaxScoreLevel2");
        score3 = PlayerPrefs.GetInt("MaxScoreLevel3");
        Level2Unlocked();
        Level3Unlocked();
    }
    
    public void Level2Unlocked()
    {
        if (score1 == 0)
        {
            lvl2Unlocked = false;
            level2.interactable = false;
        }
        else
        {
            lvl2Unlocked = true;
            level2.interactable = true;
        }
    }
    public void Level3Unlocked()
    {
        if (score2 == 0)
        {
            lvl3Unlocked = false;
            level3.interactable = false;
        }
        else
        {
            lvl3Unlocked = true;
            level3.interactable = true;
        }
    }
}
