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
    }
}