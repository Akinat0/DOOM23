using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveSystem
{
    public void Save<TStruct>(TStruct saveStruct, int slotId = 0)
    {
        string json = JsonUtility.ToJson(saveStruct);
        PlayerPrefs.SetString($"save_{slotId}", json);
        PlayerPrefs.Save();
    }

    public TStruct Load<TStruct>(int slotId = 0)
    {
        string json = PlayerPrefs.GetString($"save_{slotId}", string.Empty);
        return JsonUtility.FromJson<TStruct>(json);
    }
}

[Serializable]
public class SaveStruct
{
    public Vector3 position;
    public Quaternion rotation;
}
