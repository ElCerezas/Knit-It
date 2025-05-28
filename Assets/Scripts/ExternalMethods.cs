using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteType
{
    Basic, Golden, Gum, Healing, ZigZagR, ZigZagL, Quick, Feint, Instant, Double
}
public struct BeatData
{
    public double time;//Cuando aparece la nota (s)
    public int column;//Columna donde spawnea (0-3)
    public NoteType type;//Tipo de nota a spawnear
}
[Serializable]
public class BeatOverrideData
{
    public int beatIndex;
    public int? column;
    public string type;
}

public static class JsonUtilityWrapper
{
    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }

    public static List<T> FromJsonList<T>(string json)
    {
        if (string.IsNullOrEmpty(json)) return new List<T>();

        try
        {
            var wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"items\":" + json + "}");
            return wrapper.items ?? new List<T>();
        }
        catch
        {
            Debug.LogError("Error parsing JSON overrides.");
            return new List<T>();
        }
    }
}

[Serializable]
public class BeatOverrideList
{
    public List<BeatOverrideData> items;
}
