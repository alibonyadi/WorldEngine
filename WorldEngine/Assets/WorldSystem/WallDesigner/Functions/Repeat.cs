using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;
using WallDesigner;

public class Repeat : FunctionItem, IFunctionItem
{
    int count = 1;

    public Repeat()
    {
        Init();
        Name = "Repeat";
        //outputTexture = new Texture();
        ClassName = typeof(Repeat).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GiveNode node = new GiveNode();
        node.AttachedFunctionItem = this;
        GiveNodes.Add((Node)node);

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        gnode.id = 0;
        GetNodes.Add(gnode);

        GetNode gnode1 = new GetNode();
        gnode1.AttachedFunctionItem = this;
        gnode1.id = 1;
        GetNodes.Add(gnode1);

        CalculateRect();
        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        IntAttrebute fl1 = new IntAttrebute(at1Rect);
        fl1.mInt = count;
        fl1.SetName("Counter");
        attrebutes.Add(fl1);
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

        IntAttrebute att = (IntAttrebute)attrebutes[0];
        att.mInt = int.Parse(item.attributeValue[0]);
        attrebutes[0] = att;

    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("IntAttrebute");

        IntAttrebute att1 = (IntAttrebute)attrebutes[0];
        string stringfloat1 = att1.mInt.ToString();
        item.attributeValue.Add(stringfloat1);

        if (GetNodes[0].ConnectedNode != null)
        {
            int connectedGetNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GetNodes[0].ConnectedNode.AttachedFunctionItem);
            item.getnodeConnectedFI.Add(connectedGetNodeNumber);
            item.getnodeItems.Add(GetNodes[0].ConnectedNode.id);
        }

        if (GetNodes[1].ConnectedNode != null)
        {
            int connectedGetNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GetNodes[1].ConnectedNode.AttachedFunctionItem);
            item.getnodeConnectedFI.Add(connectedGetNodeNumber);
            item.getnodeItems.Add(GetNodes[1].ConnectedNode.id);
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
        WallPartItem wpi = new WallPartItem();
        WallPartItem Item2 = new WallPartItem();
        WallPartItem Temp = new WallPartItem();
        IntAttrebute att1 = attrebutes[0] as IntAttrebute;

        if (GetNodes[1].ConnectedNode == null)
            return mMesh;


        wpi = (WallPartItem)GetNodes[1].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[1].ConnectedNode.id);

        if (GetNodes[0].ConnectedNode != null)
        {
            for(int i = 0;i< att1.mInt;i++)
            {
                Temp = new WallPartItem();
                //Debug.Log(i);
                
                if (i > 0)
                {
                    Temp = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(Item2, GetNodes[0].ConnectedNode.id);
                    Item2 = CombineItems.CombineTwoItem(Item2.mesh,Temp.mesh,Item2.material,Temp.material);
                }
                else
                {
                    //Temp = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);
                    Item2 = wpi;
                }
            }
        }
        WallPartItem item = new WallPartItem();
        item.material.Clear();
        item.mesh = Item2.mesh;
        Debug.Log(Item2.mesh.vertexCount);
        item.material = AddMaterial.CopyMaterials(Item2);

        return item;
    }
}
