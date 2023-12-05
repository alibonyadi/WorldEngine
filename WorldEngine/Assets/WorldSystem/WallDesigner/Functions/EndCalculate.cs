using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;
using static UnityEditor.Progress;

[System.Serializable]
public class EndCalculate : FunctionItem, IFunctionItem
{
    WallItem wallItem;

    public EndCalculate(int gets, int gives)
    {
        Name = "End Calculate";
        //WallEditorController.Instance.EndItem = this;
        ClassName = typeof(EndCalculate).FullName;
        basecolor = Color.red;
        myFunction = Execute;
        wallItem = new WallItem();
        GiveNodes = new List<Node>();
        GetNodes = new List<Node>();
        GetNode node = new GetNode();
        node.AttachedFunctionItem = this;
        GetNodes.Add((Node)node);
        node = new GetNode();
        node.color = Color.yellow;
        node.AttachedFunctionItem = this;
        GetNodes.Add((Node)node);
        //position = new Vector2(200, 200);
        CalculateRect();
        rect = new Rect(position.x, position.y, rect.width, rect.height);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.getnodeConnectedFI.Count > 0)
        {
            GetNodes[0].ConnectedNode = functionItems[item.getnodeConnectedFI[0]].GiveNodes[item.getnodeItems[0]];
        }
        //GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GiveNodes[item.givenodeItems[0]];
        //GiveNodes[1].ConnectedNode = functionItems[item.givenodeConnectedFI[1]].GiveNodes[item.givenodeItems[1]];
    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {

    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        if (GetNodes[0].ConnectedNode != null)
        {
            int connectedGetNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GetNodes[0].ConnectedNode.AttachedFunctionItem);
            item.getnodeConnectedFI.Add(connectedGetNodeNumber);
            item.getnodeItems.Add(GetNodes[0].ConnectedNode.id);
        }
        return item;
    }

    public object Execute(object mesh,object id)
    {
        if (GetNodes[0].ConnectedNode == null)
            return wallItem;

        //wallItem.material.Clear();

        wallItem = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wallItem, GetNodes[0].ConnectedNode.id);
        for (int i = 0; i < wallItem.wallPartItems.Count; i++)
        {
            if (i > 0)
            {
                wallItem.wallPartItems[i] = CombineItems.CombineTwoItem(wallItem.wallPartItems[i - 1].mesh, wallItem.wallPartItems[i].mesh, wallItem.wallPartItems[i - 1].material, wallItem.wallPartItems[i].material);
            }
        }

        WallPartItem EndItem = wallItem.wallPartItems[wallItem.wallPartItems.Count - 1];
        WallItem output = new WallItem();
        //wallItem.Clear();
        output.wallPartItems.Add(EndItem);
        output.buildingDirection = wallItem.buildingDirection;
        //return wallItem;
        return output;
    }
}