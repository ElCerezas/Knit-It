using UnityEngine;

public class BoxLogic : MonoBehaviour
{
    [SerializeField] int col, row;

    public int GetBoxCol() => col;
    public int GetBoxRow() => row;

    public void SpawnNote(NoteType type, int beatIndex, int column)
    {
        if (!NotePrefabLibrary.PrefabDict.TryGetValue(type, out GameObject prefab))
        {
            Debug.LogError($"[BoxLogic] No prefab found for note type: {type}");
            return;
        }

        Instantiate(prefab, transform.position, Quaternion.identity, transform);
    }
}
