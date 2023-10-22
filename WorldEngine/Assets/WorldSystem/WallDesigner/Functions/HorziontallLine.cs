using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using WallDesigner;

public class HorziontallLine : FunctionItem, IFunctionItem
{
    private WallPartItem UpperPart;
    private WallPartItem LowerPart;
    private float distance = 1;
    public HorziontallLine()
    {
        Init();
        Name = "Horrizontal Slicer";
        UpperPart = new WallPartItem();
        LowerPart = new WallPartItem();
        ClassName = typeof(HorziontallLine).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);

        GiveNode givenode1 = new GiveNode();
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);

        GiveNode givenode2 = new GiveNode();
        givenode2.AttachedFunctionItem = this;
        givenode2.id = 1;
        GiveNodes.Add(givenode2);

        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        FloatAttrebute fl1 = new FloatAttrebute(at1Rect);
        fl1.mFloat = distance;
        fl1.SetName("Distance");
        attrebutes.Add(fl1);
    }

    public object Execute(object mMesh,object id)
    {
        FloatAttrebute fa1 = (FloatAttrebute)attrebutes[0];
        distance = fa1.mFloat;
        if (mMesh == null)
            return null;

        WallPartItem wpi = (WallPartItem)mMesh;
        
        if (GetNodes[0].ConnectedNode != null)
            wpi = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);

        Mesh mesh = wpi.mesh;
        //List<Material material = wpi.material;
        mesh.RecalculateNormals();
        Mesh upperMesh = new Mesh();
        Mesh lowerMesh = new Mesh();

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector2[] uv = mesh.uv;
        Vector3[] normals = mesh.normals;

        List<int> upperTriangles = new List<int>();
        List<Vector3> upperVertices = new List<Vector3>();
        List<Vector2> UpperUV = new List<Vector2>();
        List<Vector3> UpperNormals = new List<Vector3>();

        List<int> lowerTriangles = new List<int>();
        List<Vector3> lowerVertices = new List<Vector3>();
        List<Vector2> lowerUV = new List<Vector2>();
        List<Vector3> lowerNormals = new List<Vector3>();

        float highestZ = float.MinValue;
        foreach (Vector3 vertex in vertices)
        {
            if (vertex.z > highestZ)
            {
                highestZ = vertex.z;
            }
        }
        float cutZ = highestZ - distance;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            if (vertices[triangles[i]].z > cutZ && vertices[triangles[i]+1].z > cutZ && vertices[triangles[i]+2].z > cutZ && (int)id==0 )
            {
                upperVertices.Add(vertices[triangles[i]]);
                upperVertices.Add(vertices[triangles[i+1]]);
                upperVertices.Add(vertices[triangles[i+2]]);

                UpperUV.Add(uv[triangles[i]]);
                UpperUV.Add(uv[triangles[i+1]]);
                UpperUV.Add(uv[triangles[i + 2]]);

                UpperNormals.Add(normals[triangles[i]]);
                UpperNormals.Add(normals[triangles[i + 1]]);
                UpperNormals.Add(normals[triangles[i + 2]]);

                upperTriangles.Add(i);
                upperTriangles.Add(i+1);
                upperTriangles.Add(i+2);
            }
            else if (vertices[triangles[i]].z < cutZ && vertices[triangles[i] + 1].z < cutZ && vertices[triangles[i] + 2].z < cutZ)
            {
                lowerVertices.Add(vertices[triangles[i]]);
                lowerVertices.Add(vertices[triangles[i + 1]]);
                lowerVertices.Add(vertices[triangles[i + 2]]);
                
                lowerUV.Add(uv[triangles[i]]);
                lowerUV.Add(uv[triangles[i + 1]]);
                lowerUV.Add(uv[triangles[i + 2]]);

                lowerNormals.Add(normals[triangles[i]]);
                lowerNormals.Add(normals[triangles[i + 1]]);
                lowerNormals.Add(normals[triangles[i + 2]]);

                lowerTriangles.Add(i);
                lowerTriangles.Add(i +1);
                lowerTriangles.Add(i +2);
            }
            else
            {
                if(vertices[triangles[i]].z < cutZ)
                {
                    lowerVertices.Add(vertices[triangles[i]]);  
                    vertices[triangles[i]].z = cutZ;
                    upperVertices.Add(vertices[triangles[i]]);
                }
                else
                {
                    upperVertices.Add(vertices[triangles[i]]);
                    vertices[triangles[i]].z = cutZ;
                    lowerVertices.Add(vertices[triangles[i]]);

                }

                if (vertices[triangles[i+1]].z < cutZ)
                {
                    lowerVertices.Add(vertices[triangles[i+1]]);
                    vertices[triangles[i+1]].z = cutZ;
                    upperVertices.Add(vertices[triangles[i+1]]);

                }
                else
                {
                    upperVertices.Add(vertices[triangles[i + 1]]);
                    vertices[triangles[i + 1]].z = cutZ;
                    lowerVertices.Add(vertices[triangles[i + 1]]);
                }

                if (vertices[triangles[i + 2]].z < cutZ)
                {
                    lowerVertices.Add(vertices[triangles[i + 2]]);
                    vertices[triangles[i + 2]].z = cutZ;
                    upperVertices.Add(vertices[triangles[i + 2]]);
                }
                else
                {
                    upperVertices.Add(vertices[triangles[i + 2]]);
                    vertices[triangles[i + 2]].z = cutZ;
                    lowerVertices.Add(vertices[triangles[i + 2]]);
                }

                UpperUV.Add(uv[triangles[i]]);
                UpperUV.Add(uv[triangles[i + 1]]);
                UpperUV.Add(uv[triangles[i + 2]]);

                UpperNormals.Add(normals[triangles[i]]);
                UpperNormals.Add(normals[triangles[i + 1]]);
                UpperNormals.Add(normals[triangles[i + 2]]);

                upperTriangles.Add(i);
                upperTriangles.Add(i + 1);
                upperTriangles.Add(i + 2);

                lowerUV.Add(uv[triangles[i]]);
                lowerUV.Add(uv[triangles[i + 1]]);
                lowerUV.Add(uv[triangles[i + 2]]);

                lowerNormals.Add(normals[triangles[i]]);
                lowerNormals.Add(normals[triangles[i + 1]]);
                lowerNormals.Add(normals[triangles[i + 2]]);

                lowerTriangles.Add(i);
                lowerTriangles.Add(i + 1);
                lowerTriangles.Add(i + 2);

            }
        }

        upperMesh.vertices = upperVertices.ToArray();
        upperMesh.triangles = upperTriangles.ToArray();
        upperMesh.uv = UpperUV.ToArray();
        upperMesh.normals = UpperNormals.ToArray();
        upperMesh.RecalculateNormals();
        upperMesh.RecalculateBounds();

        lowerMesh.vertices = lowerVertices.ToArray();
        lowerMesh.triangles = lowerTriangles.ToArray();
        lowerMesh.uv = lowerUV.ToArray();
        lowerMesh.normals = lowerNormals.ToArray();
        lowerMesh.RecalculateNormals();
        lowerMesh.RecalculateBounds();


        Debug.Log("id = "+id +" --u v c:"+ upperMesh.vertices.Length + " --L v c:" + lowerMesh.vertices.Length+ "cutZ = "+cutZ);

        if ((int)id == 0)
        {
            UpperPart.mesh = upperMesh;
            UpperPart.material = wpi.material;
            return UpperPart;
        }
        else
        {
            LowerPart.mesh = lowerMesh;
            LowerPart.material = wpi.material;
            return LowerPart;
        }
    }
}
