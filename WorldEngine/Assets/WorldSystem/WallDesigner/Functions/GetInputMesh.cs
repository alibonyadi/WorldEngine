using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class GetInputMesh : FunctionItem, IFunctionItem
{
    public WallItem inputMesh;
    public bool havemesh = false;


    public GetInputMesh(int gets, int gives)
    {
        Init();
        Name = "Get Input Mesh";
        ClassName = typeof(GetInputMesh).FullName;
        basecolor = Color.blue;
        inputMesh = new WallItem();
        myFunction = Execute;

        GiveNode givenode1 = new GiveNode();
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);

        CalculateRect();
    }
    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;
    }


    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.givenodeConnectedFI.Count > 0)
            GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GetNodes[item.givenodeItems[0]];

    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;


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
        WallItem item = new WallItem();

        if (havemesh)
        {
            for(int i=0;i< inputMesh.wallPartItems.Count;i++)
            {
                WallPartItem item1 = new WallPartItem();
                item1.mesh = inputMesh.wallPartItems[i].mesh;
                item1.material = AddMaterial.CopyMaterials(inputMesh.wallPartItems[i]);
                item.buildingDirection = inputMesh.buildingDirection;
                item.isInEditMode = inputMesh.isInEditMode;
                item.Caller = inputMesh.Caller;
                item.wallPartItems.Add(item1);
            }
        }
        else
        {
            WallPartItem item1 = new WallPartItem();
            item1.material.Clear();
            Material material = new Material(Shader.Find("Standard"));
            item1.material.Add(material);
            item.wallPartItems.Add(item1);
        }

        return item;
    }
}
