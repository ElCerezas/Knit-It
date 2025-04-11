using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLogic : MonoBehaviour
{
    [SerializeField] int col, row;
    private void OnEnable()
    {
        SongManager.OnBeat += HandleBeat;
    }

    private void OnDisable()
    {
        SongManager.OnBeat -= HandleBeat;
    }

    void HandleBeat()
    {
        GameObject[] Notes = transform.GetComponentsInChildren<GameObject>();
        for (int i = 0; i < Notes.Length; i++)
        {
            Notes[i].SendMessage("OnBeatMove");
        }
    }
    public int GetBoxCol()
    {
        return col;
    }
    public int GetBoxRow()
    {
        return row;
    }
    public void SpawnNote(NoteType type)
    {
        string prefabName = $"Note_{type}";
        GameObject notePrefab = Resources.Load<GameObject>($"Prefabs/Notes/{prefabName}");

        if (notePrefab != null)
        {
            GameObject spawned = Instantiate(notePrefab, transform.position, Quaternion.identity, transform);
        }
        else
        {
            Debug.LogWarning($"No se encontró el prefab para {type}");
        }
    }

}
