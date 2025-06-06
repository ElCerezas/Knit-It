using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeringEffects : MonoBehaviour
{
    [SerializeField] public string[] cheering, booing;
    SoundManager soundManager;
    float previousCombo = 1;
    int aditivePercentage = 5;
    private void Start()
    {
        soundManager = SoundManager.Instance;
        ScoreSongManager.OnNewCombo += CheerOrBoo;
    }
    private void OnDisable()
    {
        ScoreSongManager.OnNewCombo -= CheerOrBoo;
    }
    void CheerOrBoo(float combo)
    {
        int randomRange = Random.Range(0, 100);
        if (previousCombo > combo)
        {
            soundManager.PlaySound(booing[Random.Range(0, booing.Length)]);
            aditivePercentage++;
            Debug.Log("Booo");
        }
        else if (previousCombo <= combo && combo != 1)
        {
            if(randomRange <= aditivePercentage)
            {
                soundManager.PlaySound(cheering[Random.Range(0, booing.Length)]);
                aditivePercentage = 5;
            }
            else
            {
                aditivePercentage++;
            }
        }
        previousCombo = combo;
    }

}
