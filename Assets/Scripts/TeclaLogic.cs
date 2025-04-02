using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeclaLogic : MonoBehaviour
{
    ScoreSongManager ScoreSongManager;
    GameObject nota;
    private void Start()
    {
        ScoreSongManager = ScoreSongManager.instance;
    }
    public void TeclaPulsada()
    {
        if (nota == null)
        {
            ScoreSongManager.NoteMiss();
        }
        else
        {
            ScoreSongManager.NoteHit();
            Destroy(nota);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        nota = collision.gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        nota = null;
    }
}
