using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTester : MonoBehaviour
{
    bool swap = false;
    SpriteRenderer sp;
    private void Start()
    {
        SongManager.OnBeat += Test;
        sp = GetComponent<SpriteRenderer>();
    }
    private void OnDisable()
    {
        SongManager.OnBeat -= Test;
    }
    private void Test()
    {
        swap = !swap;
        if (swap)
        {
            sp.color = Color.white;

        }

        else
        {
            sp.color = Color.black;
        }
    }
}
