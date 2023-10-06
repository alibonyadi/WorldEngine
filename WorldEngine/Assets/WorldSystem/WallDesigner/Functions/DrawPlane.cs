using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;


public class DrawPlane : FunctionItem, IFunctionItem
{
    private Mesh outputMesh;

    private float width = 1;
    private float height = 1;

    public DrawPlane()
    {
        Init();
        Name = "Draw Plane";
        outputMesh = new Mesh();
        ClassName = typeof(DrawPlane).FullName;
        basecolor = Color.white;
        myFunction = Execute;
        GiveNode node = new GiveNode();
        node.AttachedFunctionItem = this;
        GiveNodes.Add((Node)node);
        //position = new Vector2(200, 200);
        CalculateRect();
        rect = new Rect(position.x, position.y, rect.width, rect.height);
        Rect at1Rect = new Rect(position.x+15,position.y+5,60,15);
        FloatAttrebute fl1 = new FloatAttrebute(at1Rect);
        attrebutes.Add(fl1);
        Rect at2Rect = new Rect(position.x + 15, position.y + 25, 60, 15);
        FloatAttrebute fl2 = new FloatAttrebute(at2Rect);
        attrebutes.Add(fl2);
    }

    public Mesh Execute(Mesh mesh)
    {
        mesh = new Mesh();

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

        return mesh;
    }
} 