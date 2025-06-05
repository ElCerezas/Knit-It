using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NotePrefabEntry
{
    public NoteType type;
    public GameObject prefab;
}
public static class NotePrefabLibrary
{
    public static Dictionary<NoteType, GameObject> PrefabDict = new Dictionary<NoteType, GameObject>();
}
public class NotePrefabInitializer : MonoBehaviour
{
    public List<NotePrefabEntry> notePrefabs;

    void Awake()
    {
        NotePrefabLibrary.PrefabDict.Clear();
        foreach (var entry in notePrefabs)
        {
            if (!NotePrefabLibrary.PrefabDict.ContainsKey(entry.type))
                NotePrefabLibrary.PrefabDict.Add(entry.type, entry.prefab);
        }
    }
}
