using System.Collections.Generic;

[System.Serializable]
public class SerializedFunctionItem
{

    public string name = "functionItem";
    public string ClassName = "FunctionItem";
    public List<string> attributeName;
    public List<string> attributeValue;
    public List<int> getnodeConnected;
    public List<int> getnodeItems;
    public List<int> givenodeConnected;
    public List<int> givenodeItems;

    public SerializedFunctionItem()
    {
        name = "functionItem";
        ClassName = "FunctionItem";
        attributeName = new List<string>();
        attributeValue = new List<string>();
        getnodeConnected = new List<int>();
        getnodeItems = new List<int>();
        givenodeConnected = new List<int>();
        givenodeItems = new List<int>();
    }
}
