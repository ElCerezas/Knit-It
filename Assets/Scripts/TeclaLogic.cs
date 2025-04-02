using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeclaLogic : MonoBehaviour
{
    ScoreSongManager ScoreSongManager;
    GameObject nota;
    private void Start()
    {
        ScoreSongManager = ScoreSongManager.instance;
    }
    public void TeclaPulsada(InputAction.CallbackContext Context)
    {
        if (Context.performed)
        {
            if (nota == null)
            {
                ScoreSongManager.NoteMiss();
            }
            else
            {
                ScoreSongManager.NoteHit(false);
                Destroy(nota);
            }
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
