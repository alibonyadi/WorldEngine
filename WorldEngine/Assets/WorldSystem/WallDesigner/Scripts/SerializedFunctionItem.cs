using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SerializedFunctionItem
{

    public string name = "functionItem";
    public string ClassName = "FunctionItem";
    public Vector2 Position = new Vector2();
    public List<string> attributeName;
    public List<string> attributeValue;
    public List<int> getnodeConnectedFI;
    public List<int> getnodeItems;
    public List<int> givenodeConnectedFI;
    public List<int> givenodeItems;
    public List<SerializedProperty> properties;

    public SerializedFunctionItem()
    {
        name = "functionItem";
        ClassName = "FunctionItem";
        Position = new Vector2();
        attributeName = new List<string>();
        attributeValue = new List<string>();
        getnodeConnectedFI = new List<int>();
        getnodeItems = new List<int>();
        givenodeConnectedFI = new List<int>();
        givenodeItems = new List<int>();
        properties = new List<SerializedProperty>();
    }
}


[System.Serializable]
public class SerializedProperty
{
    public List<int> getnodeConnectedFI;
    public List<int> getnodeConnectedAttrebute;
    public List<int> getnodeItems;
    public int getnodeCount;
    public List<bool> hasGetConnected;
    public List<int> givenodeConnectedFI;
    public List<int> givenodeConnectedAttrebute;
    public List<int> givenodeItems;
    public int givennodeCount;
    public List<bool> hasGiveConnected;
    public SerializedProperty()
    {
        getnodeConnectedFI = new List<int>();
        getnodeConnectedAttrebute = new List<int>();
        getnodeItems = new List<int>();
        getnodeCount = 0;
        hasGetConnected = new List<bool>();
        givenodeConnectedFI = new List<int>();
        givenodeConnectedAttrebute = new List<int>();
        givenodeItems = new List<int>();
        givennodeCount = 0;
        hasGiveConnected = new List<bool>();
    }
}