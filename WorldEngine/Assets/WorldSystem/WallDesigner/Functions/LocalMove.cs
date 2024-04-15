using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class LocalMove : FunctionItem, IFunctionItem
{
    float X, Y, Z;

    public LocalMove(int gets, int gives)
    {
        Init();
        Name = "Local Move";
        ClassName = typeof(LocalMove).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);

        GiveNode givenode1 = new GiveNode();
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);


        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);

        FloatAttrebute fl1 = new FloatAttrebute(at1Rect, this);
        fl1.mFloat = X;
        fl1.SetMinMax(-5, 5);
        fl1.SetName("X");
        attrebutes.Add(fl1);

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y + 15, rect.width, rect.height);

        FloatAttrebute fl2 = new FloatAttrebute(at2Rect, this);
        fl2.mFloat = Y;
        fl2.SetMinMax(-5, 5);
        fl2.SetName("Y");
        attrebutes.Add(fl2);

        Rect at3Rect = new Rect(position.x, rect.height / 2 + position.y + 30, rect.width, rect.height);

        FloatAttrebute fl3 = new FloatAttrebute(at3Rect, this);
        fl3.mFloat = Z;
        fl3.SetMinMax(-5, 5);
        fl3.SetName("Z");
        attrebutes.Add(fl3);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.getnodeConnectedFI.Count > 0)
            GetNodes[0].ConnectedNode = functionItems[item.getnodeConnectedFI[0]].GiveNodes[item.getnodeItems[0]];

        if (item.givenodeConnectedFI.Count > 0)
            GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GetNodes[item.givenodeItems[0]];

    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        FloatAttrebute ta1 = (FloatAttrebute)attrebutes[0];
        ta1.mFloat = float.Parse(item.attributeValue[0]);
        attrebutes[0] = ta1;

        FloatAttrebute att = (FloatAttrebute)attrebutes[1];
        att.mFloat = float.Parse(item.attributeValue[1]);
        attrebutes[1] = att;

        FloatAttrebute att3 = (FloatAttrebute)attrebutes[2];
        att3.mFloat = float.Parse(item.attributeValue[2]);
        attrebutes[2] = att3;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("FloatAttrebute");

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[0];
        string stringtexturePath = att1.mFloat.ToString();
        item.attributeValue.Add(stringtexturePath);

        FloatAttrebute att2 = (FloatAttrebute)attrebutes[1];
        string stringtexturePath2 = att2.mFloat.ToString();
        item.attributeValue.Add(stringtexturePath2);

        FloatAttrebute att3 = (FloatAttrebute)attrebutes[2];
        string stringtexturePath3 = att3.mFloat.ToString();
        item.attributeValue.Add(stringtexturePath3);

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
        WallItem item = mMesh as WallItem;

        if (GetNodes[0].ConnectedNode != null)
            item = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(item, GetNodes[0].ConnectedNode.id);

        FloatAttrebute fl1 = (FloatAttrebute)attrebutes[0];
        X = (float)fl1.GetValue();

        FloatAttrebute fl2 = (FloatAttrebute)attrebutes[1];
        Y = (float)fl2.GetValue();

        FloatAttrebute fl3 = (FloatAttrebute)attrebutes[2];
        Z = (float)fl3.GetValue();

        WallItem outitem = new WallItem();

        for (int j = 0; j < item.wallPartItems.Count; j++)
        {
            Mesh originalMesh = item.wallPartItems[j].mesh;
            Mesh MovedMesh = new Mesh();

            Vector3[] vertices = originalMesh.vertices;

            int numSubMeshes = originalMesh.subMeshCount;



            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] += new Vector3(X, Y, Z);
            }


            MovedMesh.vertices = vertices;
            MovedMesh.normals = originalMesh.normals;
            MovedMesh.uv = originalMesh.uv;
            //MovedMesh.triangles = originalMesh.triangles;
            MovedMesh.subMeshCount = numSubMeshes;
            for (int i = 0; i < numSubMeshes; i++)
            {
                int[] originalTriangles = originalMesh.GetTriangles(i);
                MovedMesh.SetTriangles(originalTriangles, i);
            }

            WallPartItem output = new WallPartItem();
            output.mesh = MovedMesh;
            output.material = AddMaterial.CopyMaterials(item.wallPartItems[j]);
            outitem.wallPartItems.Add(output);

        }
        return outitem;
    }

}
