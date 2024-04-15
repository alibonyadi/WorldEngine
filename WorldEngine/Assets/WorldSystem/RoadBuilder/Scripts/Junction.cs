using System.Collections.Generic;
using UnityEngine;

public class Junction : MonoBehaviour
{
    public void CreateBase(List<Vector3> startVertices,List<Vector3> endVertices)
    {
        GenerateBaseMesh(startVertices, endVertices);
    }

    public void GenerateBaseMesh(List<Vector3> startList, List<Vector3> endlist)
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();

        if (gameObject.GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = CreateBasePlane(startList, endlist);
        mesh.RecalculateBounds();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    public Mesh CreateBasePlane(List<Vector3> startList, List<Vector3> endList)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[3];
        Vector3[] normals = new Vector3[3];
        Vector2[] uv = new Vector2[3];
        int[] triangles = new int[3];

        vertices[0] = transform.InverseTransformPoint(startList[0]);
        vertices[1] = transform.InverseTransformPoint(startList[1]);
        vertices[2] = transform.InverseTransformPoint(endList[0]);
        //vertices[3] = transform.InverseTransformPoint(endList[1]);

        normals[0] = new Vector3(0, 1, 0);
        normals[1] = new Vector3(0, 1, 0);
        normals[2] = new Vector3(0, 1, 0);
        //normals[3] = new Vector3(0, 1, 0);

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        //uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        //triangles[3] = 3;
        //triangles[4] = 2;
        //triangles[5] = 1;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.normals = normals;
        mesh.triangles = triangles;

        return mesh;

    }
}