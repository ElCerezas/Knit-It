using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeclaLogic : MonoBehaviour
{
    ScoreSongManager ScoreSongManager;
    GameObject nota;
    [SerializeField] Collider2D perfectZone;
    bool perfectNote = false;
    private void Start()
    {
        ScoreSongManager = ScoreSongManager.Instance;
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
                nota.GetComponent<NoteLogic>().OnNoteHit(perfectNote);
                Destroy(nota);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        perfectNote = perfectZone.IsTouching(collision);
        nota = collision.gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        perfectNote = perfectZone.IsTouching(collision);
        nota = null;
    }
}
