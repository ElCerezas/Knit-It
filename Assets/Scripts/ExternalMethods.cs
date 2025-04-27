using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteType
{
    Basic, Golden, BubleGum
}
public struct BeatData
{
    public double time;//Cuando aparece la nota (s)
    public int column;//Columna donde spawnea (0-3)
    public NoteType type;//Tipo de nota a spawnear
}
public class BeatOverrideData
{
    public int beatIndex;//En que beat se ha de cambiar la nota
    public int? column;//Si se hubiese de ajustar la columna
    public string type;//Tipo de nota, default = Basic
}
public static class JsonUtilityWrapper //Para que unity pueda leer el json
{
    private class Wrapper<T>
    {
        public List<T> list;
    }
    public static List<T> FromJsonList<T>(string json)
    {
        return JsonUtility.FromJson<Wrapper<T>>("{\"list\":" + json + "}").list;
    }
}

