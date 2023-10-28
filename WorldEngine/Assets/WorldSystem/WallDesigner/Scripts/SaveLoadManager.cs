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


public class SaveLoadManager
{
    public static void SaveAllItems()
    {
        List<SerializedFunctionItem> functionItems = new List<SerializedFunctionItem>();

        foreach(FunctionItem item in WallEditorController.Instance.GetAllCreatedItems())
        {
            functionItems.Add(item.SaveSerialize());
        }
        string path = Application.dataPath + "/WorldSystem/WallDesigner/CreatedFunctions";
        SaveToXml(path+"/TestSave1.Wall", functionItems);
    }

    public static void LoadAllItems()
    {
        string path = Application.dataPath + "/WorldSystem/WallDesigner/CreatedFunctions";
        List<SerializedFunctionItem> functionItems = LoadSerializedFunctionItemList(path+ "/TestSave1.Wall");

        List<FunctionItem> functions = new List<FunctionItem>();

        foreach (SerializedFunctionItem item in functionItems)
        {
            Type type = Type.GetType(item.ClassName);
            if (type != null && type.IsSubclassOf(typeof(FunctionItem)))
            {
                FunctionItem fitem = (FunctionItem)Activator.CreateInstance(type);
                fitem.LoadSerializedAttributes(item);
                functions.Add(fitem);
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