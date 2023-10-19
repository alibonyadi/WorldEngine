using UnityEngine;
using WallDesigner;


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

    public object Execute(object mMesh)
    {
        if (GetNodes[0].ConnectedNode != null)
            output = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(output);


        Mesh mesh = new Mesh();
        FloatAttrebute fa1 = (FloatAttrebute)attrebutes[0];
        FloatAttrebute fa2 = (FloatAttrebute)attrebutes[1];
        width = fa1.mFloat;
        height = fa2.mFloat;

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(-width / 2, 0, height / 2);
        vertices[1] = new Vector3(width / 2, 0, height / 2);
        vertices[2] = new Vector3(width / 2, 0, -height / 2);
        vertices[3] = new Vector3(-width / 2, 0, -height / 2);

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(1, 0);
        uv[3] = new Vector2(0, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        output.mesh = mesh;

        return output;
    }
} 