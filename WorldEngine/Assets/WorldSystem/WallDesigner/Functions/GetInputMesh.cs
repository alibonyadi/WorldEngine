using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class GetInputMesh : FunctionItem, IFunctionItem
{
    public WallPartItem inputMesh;
    public bool havemesh = false;


    public GetInputMesh()
    {
        Init();
        Name = "Get Input Mesh";
        ClassName = typeof(GetInputMesh).FullName;
        basecolor = Color.blue;
        inputMesh = new WallPartItem();
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
        WallPartItem item = new WallPartItem();
        if (havemesh)
        {
            Debug.Log("input found!!!");
            item.mesh = inputMesh.mesh;
            item.material = AddMaterial.CopyMaterials(inputMesh);
        }
        else
        {
            Debug.Log("no input!!!");
            //item = (WallPartItem)mMesh;
        }
        item.material.Clear();
        Material material = new Material(Shader.Find("Standard"));
        item.material.Add(material);
        return item;
    }
}
