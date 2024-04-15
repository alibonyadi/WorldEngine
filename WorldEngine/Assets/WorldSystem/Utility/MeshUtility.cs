using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MeshUtility
{
    public static int[] TriangulateVertices(Vector3[] vertices)
    {
        int[] triangles = new int[(vertices.Length - 2) * 3];

        // find center of vertices
        Vector3 center = Vector3.zero;
        foreach (Vector3 vertex in vertices)
        {
            center += vertex;
        }
        center /= vertices.Length;

        // sort vertices clockwise based on angle to center
        System.Array.Sort(vertices, (a, b) =>
        {
            float angleA = Mathf.Atan2(a.z - center.z, a.x - center.x);
            float angleB = Mathf.Atan2(b.z - center.z, b.x - center.x);
            if (angleA < angleB)
            {
                return 1;
            }
            if (angleA > angleB)
            {
                return -1;
            }
            return 0;
        });

        // triangulate vertices

        

        int index = 0;
        /*for (int i = 0; i < vertices.Length - 2; i++)
        {
            //if(i%2>0)
            //{
                triangles[index++] = 0;
                triangles[index++] = i + 1;
                triangles[index++] = i + 2;
            }
            else
            {
                triangles[index++] = 0;
                triangles[index++] = i + 1;
                triangles[index++] = i + 2;
            }
            
        }*/

        //Debug.Log("Vertices = " + vertices.Length + " -- triangles = " + triangles.Length);
        /*for(int i=0;i<vertices.Length;i++)
        {
            Debug.Log("Vertex "+i+" position = "+vertices[i]);
        }*/

        int tempIndex = 0;
        for (int i = 0;i < triangles.Length/3;i+=1)
        {
            //if (i > 1)
             //   break;

            /*if (i % 6 > 0)//Second triangle of polygon
            {
                triangles[index++] = tempIndex-1;
                triangles[index++] = tempIndex;
                triangles[index++] = 0;

                tempIndex += 1;
                //Debug.Log("tri " + (index - 3) + " = " + triangles[index - 3]);
                //Debug.Log("tri " + (index - 2) + " = " + triangles[index - 2]);
                //Debug.Log("tri " + (index - 1) + " = " + triangles[index - 1]);
                //Debug.Log("tempIndex +1 = " + tempIndex);
            }
            else
            {*/
                triangles[index++] = 0;
                triangles[index++] = i+1;
                triangles[index++] = i+2;

                tempIndex += 3;
                //Debug.Log("tri " + (index - 3) + " = " + triangles[index - 3]);
                //Debug.Log("tri " + (index - 2) + " = " + triangles[index - 2]);
                //Debug.Log("tri " + (index - 1) + " = " + triangles[index - 1]);
                //Debug.Log("tempIndex +3 = " + tempIndex);

            //}
            //Debug.Log("Max value indexed =" + (tempIndex + 2));
        }

        //Debug.Log("Max value indexed =" + (tempIndex + 2));
        return triangles;
    }

    public static Vector2[] GenerateUVList(List<Vector3> vertices)
    {
        Vector2[] list = new Vector2[vertices.Count];
        float MinX = vertices[0].x, MaxX = vertices[0].x;
        float MinZ = vertices[0].z, MaxZ = vertices[0].z;
        int index = 0;
        foreach (Vector3 v in vertices)
        {
            MinX = MinX < v.x ? MinX : v.x;
            MaxX = MaxX > v.x ? MaxX : v.x;
            MinZ = MinZ < v.z ? MinZ : v.z;
            MaxZ = MaxZ > v.z ? MaxZ : v.z;
        }

        foreach (Vector3 v in vertices)
        {
            float DifferenceX = MaxX - MinX;
            float DifferenceZ = MaxZ - MinZ;
            float mU = (v.x - MinX) / DifferenceX;
            float mV = (v.z - MinZ) / DifferenceZ;

            list[index] = new Vector2(mU, mV);
            index++;
        }

        return list.ToArray() ;
    }

    public static Mesh CreateMesh(Vector3[] newVertices, Vector2[] newUV, int[] newTriangles)
    {
        Mesh mesh = new Mesh();
        //GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;

        /*mesh.normals = new Vector3[newVertices.Length];

        for(int i = 0; i < mesh.normals.Length; i++)
        {
            mesh.normals[i] = new Vector3(0,1,0);
        }*/
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        mesh.Optimize();

        return mesh;
    }

    public static Vector3 CalculateNormal(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3 side1 = v2 - v1;
        Vector3 side2 = v3 - v1;

        return Vector3.Cross(side1, side2).normalized;
    }

    public static Mesh GenerateFlatMeshOnVertices(List<Vector3> vertices)
    {
        Vector3[] newVertices = vertices.ToArray();
        Vector2[] newUVs = GenerateUVList(vertices);
        int[] newTriangles = TriangulateVertices(newVertices);
        Mesh mesh = CreateMesh(newVertices, newUVs, newTriangles);
        return mesh;
    }

    public static Mesh GenerateBaseConfluenceMesh(List<Vector3> backs,List<Vector3> lefts,List<Vector3> fronts,List<Vector3> rights)
    {
        Mesh resultMesh = new Mesh();
        //r = result
        List<Vector3> rVertices = new List<Vector3>();
        List<int> rTriangles = new List<int>();
        List<Vector2> rUVs = new List<Vector2>();

        rVertices.Add(backs[0]);
        rVertices.Add(backs[1]);

        for(int i=0;i<lefts.Count;i++)
        {
            
        }

        Debug.Log("back v = " + backs.Count + " -- left v = " + lefts.Count + " -- front v = " + fronts.Count + " -- right v =" + rights.Count);

        return resultMesh;
    }

    public static List<Vector3> WeldVertices(List<Vector3> vertices, float treshold)
    {
        List<Vector3> newVerticesList = new List<Vector3>();
        //Debug.Log("Weld Start Vertices = "+vertices.Count);
        //newVerticesList.Add(vertices[0]);
        for (int i = 0; i < vertices.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < i; j++)
            {
                if (Vector3.Distance(vertices[i], vertices[j]) <= treshold)
                {
                    //Debug.Log("must weld "+vertices[i]+" !!!");
                    found = true;
                    break;
                }
            }

            if (!found)
                newVerticesList.Add(vertices[i]);
        }

        return newVerticesList;
    }

    public static Mesh RePositionMesh(Mesh mesh,Vector3 position)
    {
        Vector3[] verices = mesh.vertices;

        for(int i = 0;i<verices.Length;i++)
        {
            verices[i] += position;
        }

        mesh.vertices = verices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}
