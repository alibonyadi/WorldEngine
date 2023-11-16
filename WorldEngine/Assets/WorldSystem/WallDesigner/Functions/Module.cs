using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WallDesigner;

public class Module : FunctionItem, IFunctionItem
{
    private List<FunctionItem> functions;
    private string path;
    private FunctionItem endItem;
    private FunctionItem InputItem;
    private int repeatCount=1;
    public Module()
    {
        Init();
        Name = "Module";
        ClassName = typeof(Module).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        functions = new List<FunctionItem>();

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);

        GiveNode givenode1 = new GiveNode();
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);

        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        GetFileAttrebute att1 = new GetFileAttrebute(at1Rect);
        att1.folderlocation = Application.dataPath + "\\WorldSystem\\BuildingEditor\\Modules";
        att1.SetName(Name);
        attrebutes.Add(att1);

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y+20, rect.width, rect.height);
        IntAttrebute att2 = new IntAttrebute(at2Rect);
        att2.mInt = repeatCount;
        att2.SetMinMax(1, 10);
        att2.SetName(Name);
        attrebutes.Add(att2);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.getnodeConnectedFI.Count > 0)
        {
            GetNodes[0].ConnectedNode = functionItems[item.getnodeConnectedFI[0]].GiveNodes[item.getnodeItems[0]];
        }

        if (item.givenodeConnectedFI.Count > 0)
        {
            GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GetNodes[item.givenodeItems[0]];
        }
    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        GetFileAttrebute att = (GetFileAttrebute)attrebutes[0];
        att.adress = item.attributeValue[0];
        //att.texture = GenerateTextureFromPath(att.adress);
        attrebutes[0] = att;

        IntAttrebute att2 = (IntAttrebute)attrebutes[1];
        att2.mInt =  int.Parse(item.attributeValue[1]);
        att2.SetMinMax(1,10);
        attrebutes[1] = att2;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("GetFileAtt");

        GetFileAttrebute att1 = (GetFileAttrebute)attrebutes[0];
        if (att1.adress != null)
        {
            string stringFilePath = att1.adress;
            item.attributeValue.Add(stringFilePath);
        }

        IntAttrebute att2 = (IntAttrebute)attrebutes[1];
        string stringIntAtt = att2.mInt.ToString();
        item.attributeValue.Add(stringIntAtt);


        if (GetNodes[0].ConnectedNode != null)
        {
            int connectedGetNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GetNodes[0].ConnectedNode.AttachedFunctionItem);
            item.getnodeConnectedFI.Add(connectedGetNodeNumber);
            item.getnodeItems.Add(GetNodes[0].ConnectedNode.id);
        }

        if (GiveNodes[0].ConnectedNode != null)
        {
            int connectedGiveNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GiveNodes[0].ConnectedNode.AttachedFunctionItem);
            item.givenodeConnectedFI.Add(connectedGiveNodeNumber);
            item.givenodeItems.Add(GiveNodes[0].ConnectedNode.id);
        }
        return item;
    }

    public object Execute(object mMesh, object id)
    {
        List<WallPartItem> wpi = new List<WallPartItem>();

        if (GetNodes[0].ConnectedNode != null)
            wpi = (List<WallPartItem>)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);

        GetFileAttrebute att1 = attrebutes[0] as GetFileAttrebute;
        path = att1.adress;

        if (path == null)
        {
            return wpi;
        }

        IntAttrebute att2 = attrebutes[1] as IntAttrebute;
        repeatCount = (int)att2.mInt;

        Name = Path.GetFileNameWithoutExtension(path);
        //Debug.Log(Name);
        functions.Clear();

        List<SerializedFunctionItem> functionItems = SaveLoadManager.LoadSerializedFunctionItemList(path);
        foreach (SerializedFunctionItem item2 in functionItems)
        {
            Type type = Type.GetType(item2.ClassName);
            if (type != null && type.IsSubclassOf(typeof(FunctionItem)))
            {
                FunctionItem fitem = (FunctionItem)Activator.CreateInstance(type);
                fitem.LoadSerializedAttributes(item2);
                fitem.position = item2.Position;
                functions.Add(fitem);
            }
            if (functions[functions.Count - 1].GetType() == typeof(EndCalculate))
            {
                //EndItemIndex = functions.Count - 1;
                endItem = functions[functions.Count - 1];
                //CreateAction(EndItemIndex);
                //Debug.Log("EndItem founded!! " + endItem.Name);
            }
            if (functions[functions.Count - 1].GetType() == typeof(GetInputMesh))
            {
                InputItem = functions[functions.Count - 1];
                GetInputMesh gIM = InputItem as GetInputMesh;
                gIM.inputMesh = wpi;
                gIM.havemesh = true;
                functions[functions.Count - 1] = gIM;
                //CreateAction(EndItemIndex);
                //Debug.Log("GetInput founded!! " + InputItem.Name);
            }
        }

        for (int i = 0; i < functionItems.Count; i++)
        {
            functions[i].LoadNodeConnections(functionItems[i], functions);
        }

        if (endItem == null)
            return wpi;

        if (endItem.GetNodes[0].ConnectedNode == null)
            return wpi;

        WallPartItem item = new WallPartItem();
        item = (WallPartItem)endItem.myFunction(mMesh, 0);
        List<WallPartItem> output = new List<WallPartItem>();
        output.Add(item);
        return output;
    }
}
