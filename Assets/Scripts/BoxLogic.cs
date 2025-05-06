using Melanchall.DryWetMidi.MusicTheory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLogic : MonoBehaviour
{
    [SerializeField] int col, row;
    public int GetBoxCol()
    {
        return col;
    }
    public int GetBoxRow()
    {
        return row;
    }
    public void SpawnNote(NoteType type, int bpId, int colId)
    {
        string prefabName = $"Note_{type}";
        GameObject notePrefab = Resources.Load<GameObject>($"{prefabName}");
        notePrefab.name = ("B:"+ bpId + "C:"+colId + "T" + type.ToString());

        if (notePrefab != null)
        {
            GameObject spawned = Instantiate(notePrefab, transform.position, Quaternion.identity, transform);
        }
        else
        {
            Debug.LogWarning($"No se encontró el prefab para {prefabName} en Resources/");
        }
    }

}
