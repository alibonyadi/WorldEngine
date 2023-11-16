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
        List<WallPartItem> wpi = new List<WallPartItem>();
        List < WallPartItem> Item2 = new List<WallPartItem>();
        List < WallPartItem> item = new List<WallPartItem>();
        WallPartItem Temp = new WallPartItem();
        IntAttrebute att1 = attrebutes[0] as IntAttrebute;
        count = (int)att1.GetValue();
        if (GetNodes[1].ConnectedNode == null)
            return mMesh;


        wpi = (List<WallPartItem>)GetNodes[1].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[1].ConnectedNode.id);

        if (GetNodes[0].ConnectedNode != null)
        {
            for (int j = 0; j < wpi.Count; j++)
            {
                for (int i = 0; i < (int)att1.GetValue(); i++)
                {
                    Temp = new WallPartItem();
                    //Debug.Log(i);

                    if (i > 0)
                    {
                        Temp = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(Item2, GetNodes[0].ConnectedNode.id);
                        Item2[j] = CombineItems.CombineTwoItem(Item2[j].mesh, Temp.mesh, Item2[j].material, Temp.material);
                    }
                    else
                    {
                        //Temp = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);
                        Item2 = wpi;
                    }
                }

                item[j].material.Clear();
                item[j].mesh = Item2[j].mesh;
                item[j].material = AddMaterial.CopyMaterials(Item2[j]);
            }
        }
        else
        {
            item = wpi;
        }

        return item;
    }
}
