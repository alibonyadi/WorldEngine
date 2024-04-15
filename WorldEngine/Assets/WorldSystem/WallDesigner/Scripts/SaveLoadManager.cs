using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using WallDesigner;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System;
using UnityEditor;

public class SaveLoadManager
{
    public static void SaveAllItems()
    {
        List<SerializedFunctionItem> functionItems = new List<SerializedFunctionItem>();
        foreach(FunctionItem item in WallEditorController.Instance.GetAllCreatedItems())
        {
            SerializedFunctionItem SFI = item.SaveSerialize();
            functionItems.Add(item.SaveProperty(SFI));
        }
        string path = EditorUtility.SaveFilePanel("Save Wall Item", Application.dataPath + "/WorldSystem/WallDesigner/CreatedFunctions", "Wall", "wall");
        if (path != "" ) 
        {
            SaveToXml(path, functionItems);
            Debug.Log("Saved!!!");
        } 
    }
    public static void LoadAllItems()
    {
        string path = EditorUtility.OpenFilePanel("Open Wall Item", Application.dataPath + "/WorldSystem/WallDesigner/CreatedFunctions", "wall");
        if (path != "")
        {
            //string path = Application.dataPath + "/WorldSystem/WallDesigner/CreatedFunctions";
            List<SerializedFunctionItem> functionItems = LoadSerializedFunctionItemList(path);
            List<FunctionItem> functions = new List<FunctionItem>();
            foreach (SerializedFunctionItem item in functionItems)
            {
                Type type = Type.GetType(item.ClassName);
                if (type != null && type.IsSubclassOf(typeof(FunctionItem)))
                {
                    FunctionItem fitem = (FunctionItem)Activator.CreateInstance(type, item.getnodeItems.Count,item.givenodeItems.Count);
                    fitem.LoadSerializedAttributes(item);
                    fitem.position = item.Position;
                    functions.Add(fitem);
                }
                if (functions[functions.Count - 1].GetType() == typeof(EndCalculate))
                {
                    WallEditorController.Instance.EndItemIndex = functions.Count - 1;
                    WallEditorController.Instance.EndItem = functions[functions.Count - 1];
                    //CreateAction(EndItemIndex);
                }
            }
            WallEditorController.Instance.SetAllCreatedItems(functions);
            for (int i = 0; i < functionItems.Count; i++)
            {
                functions[i].LoadNodeConnections(functionItems[i], functions);
                functions[i].LoadProperty(functionItems[i],functions);
            }
        }
    }

    public static void SaveToXml(string path, List<SerializedFunctionItem> functionItems)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<SerializedFunctionItem>));
        using (XmlWriter writer = XmlWriter.Create(path))
        {
            serializer.Serialize(writer, functionItems);
        }
    }

    public static List<SerializedFunctionItem> LoadSerializedFunctionItemList(string filePath)
    {
        List<SerializedFunctionItem> itemList = new List<SerializedFunctionItem>();

        XmlSerializer serializer = new XmlSerializer(typeof(List<SerializedFunctionItem>));

        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
            itemList = (List<SerializedFunctionItem>)serializer.Deserialize(stream);
        }

        return itemList;
    }
    public static void SaveFunctionItemList(List<FunctionItem> functionItems)
    {
        foreach(FunctionItem item in functionItems)
        {

        }
    }
}