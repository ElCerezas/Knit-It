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
            Debug.LogWarning("No note found");
            ScoreSongManager.NoteMiss();
        }
        else
        {
            Debug.LogWarning("Note Hit");
            ScoreSongManager.NoteHit();
            Destroy(nota);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        nota = collision.gameObject;
        Debug.Log("NoteEnter");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        nota = null;
        Debug.Log("NoteExit");
    }
}
