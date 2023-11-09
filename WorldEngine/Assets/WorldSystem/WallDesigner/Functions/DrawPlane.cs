using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

[System.Serializable]
public class DrawPlane : FunctionItem, IFunctionItem
{
    private WallPartItem output;
    private float width = 1;
    private float height = 1;

    public DrawPlane()
    {
        Init();
        Name = "Draw Plane";
        output = new WallPartItem();
        ClassName = typeof(DrawPlane).FullName;
        basecolor = Color.white;
        myFunction = Execute;
        GiveNode node = new GiveNode();
        node.AttachedFunctionItem = this;
        GiveNodes.Add((Node)node);

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);
        //position = new Vector2(200, 200);
        CalculateRect();
        //rect = new Rect(position.x, position.y, rect.width, rect.height);
        Rect at1Rect = new Rect(position.x, rect.height/2+position.y,rect.width,rect.height);
        FloatAttrebute fl1 = new FloatAttrebute(at1Rect);
        fl1.mFloat = width;
        fl1.SetName("width");
        attrebutes.Add(fl1);
        Rect at2Rect = new Rect(position.x, rect.height/2+position.y + 15, rect.width, rect.height);
        FloatAttrebute fl2 = new FloatAttrebute(at2Rect);
        fl2.mFloat = height;
        fl2.SetName("heigh");
        attrebutes.Add(fl2);
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

        FloatAttrebute att = (FloatAttrebute)attrebutes[0];
        att.mFloat = float.Parse(item.attributeValue[0]);
        attrebutes[0] = att;

        FloatAttrebute att2 = (FloatAttrebute)attrebutes[1];
        att2.mFloat = float.Parse(item.attributeValue[1]);
        attrebutes[1] = att2;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("FloatAttrebute");
        item.attributeName.Add("FloatAttrebute");

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[0];
        string stringfloat1 = att1.GetValue().ToString();
        item.attributeValue.Add(stringfloat1);

        FloatAttrebute att2 = (FloatAttrebute)attrebutes[1];
        string stringfloat2 = att2.GetValue().ToString();
        item.attributeValue.Add(stringfloat2);

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
        if (GetNodes[0].ConnectedNode != null)
        {
            output = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(output, GetNodes[0].ConnectedNode.id);
            //Debug.Log(output.mesh.vertexCount);
            if (output.mesh.vertexCount > 0)
            {
                return output;
            }
        }
        else
        {
            output.material.Clear();
            Material material = new Material(Shader.Find("Standard"));
            output.material.Add(material);
        }


        Mesh mesh = new Mesh();
        FloatAttrebute fa1 = (FloatAttrebute)attrebutes[0];
        FloatAttrebute fa2 = (FloatAttrebute)attrebutes[1];
        width = (float)fa1.GetValue();
        height = (float)fa2.GetValue();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(-width / 2, 0, height / 2);
        vertices[1] = new Vector3(width / 2, 0, height / 2);
        vertices[2] = new Vector3(-width / 2, 0, -height / 2);
        vertices[3] = new Vector3(width / 2, 0, -height / 2);


        normals[0] = new Vector3(0, 1, 0);
        normals[1] = new Vector3(0, 1, 0);
        normals[2] = new Vector3(0, 1, 0);
        normals[3] = new Vector3(0, 1, 0);


        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 0);


        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 3;
        triangles[4] = 2;
        triangles[5] = 1;



        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.normals = normals;
        mesh.triangles = triangles;

        output.mesh = mesh;
        return output;
    }
} 