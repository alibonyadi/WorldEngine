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


    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.attributeName.Add("FloatAttrebute");
        item.attributeName.Add("FloatAttrebute");

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[0];
        string stringtexturePath = att1.mFloat.ToString();
        item.attributeValue.Add(stringtexturePath);

        FloatAttrebute att2 = (FloatAttrebute)attrebutes[1];
        string stringtexturePath2 = att2.mFloat.ToString();
        item.attributeValue.Add(stringtexturePath2);

        if (GetNodes[0].ConnectedNode != null)
        {
            int connectedGetNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GetNodes[0].ConnectedNode.AttachedFunctionItem);
            item.getnodeConnected.Add(connectedGetNodeNumber);
        }

        if (GiveNodes[0].ConnectedNode != null)
        {
            int connectedGiveNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GiveNodes[0].ConnectedNode.AttachedFunctionItem);
            item.getnodeConnected.Add(connectedGiveNodeNumber);
        }

        return item;
    }


    public object Execute(object mMesh,object id)
    {

        if (GetNodes[0].ConnectedNode != null)
            output = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(output, GetNodes[0].ConnectedNode.id);
        else
        {
            output.material.Clear();
            Material material = new Material(Shader.Find("Standard"));
            output.material.Add(material);
        }

        Mesh mesh = new Mesh();
        FloatAttrebute fa1 = (FloatAttrebute)attrebutes[0];
        FloatAttrebute fa2 = (FloatAttrebute)attrebutes[1];
        width = fa1.mFloat;
        height = fa2.mFloat;

        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(-width / 2, 0, height / 2);
        vertices[1] = new Vector3(width / 2, 0, height / 2);
        vertices[2] = new Vector3(width / 2, 0, -height / 2);
        vertices[3] = new Vector3(-width / 2, 0, -height / 2);
        vertices[4] = new Vector3(-width / 2, 0, -height / 2);
        vertices[5] = new Vector3(width / 2, 0, height / 2);

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(1, 0);
        uv[3] = new Vector2(0, 0);
        uv[4] = new Vector2(0, 0);
        uv[5] = new Vector2(1, 1);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 2;
        triangles[4] = 4;
        triangles[5] = 5;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        output.mesh = mesh;

        return output;
    }
} 