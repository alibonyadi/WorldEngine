using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using WallDesigner;
using System.Text;


public class SaveLoadManager
{
    public static void SaveData(string path, SaveData saveData)
    {
        string jsonData = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, jsonData);
    }

    public static SaveData LoadData(string path)
    {
        SaveData saveData = new SaveData();
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(jsonData);
        }
        return saveData;
    }
}

[System.Serializable]
public class SaveData
{
    public List<FunctionItem> functionItemList;
}
