using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using WallDesigner;

public class Extrude : FunctionItem, IFunctionItem
{
    float extrudeDistance = 1.0f;
    float insetDistance = 0.0f;

    public Extrude()
    {
        Init();
        Name = "Extrude";
        ClassName = typeof(Extrude).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);

        GiveNode givenode1 = new GiveNode();
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);

        GiveNode givenode2 = new GiveNode();
        givenode2.id = 1;
        givenode2.AttachedFunctionItem = this;
        GiveNodes.Add(givenode2);

        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);

        FloatAttrebute fl1 = new FloatAttrebute(at1Rect);
        fl1.mFloat = extrudeDistance;
        fl1.SetMinMax(-5,10);
        fl1.SetName("Extrude");
        attrebutes.Add(fl1);

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y +15, rect.width, rect.height);

        FloatAttrebute fl2 = new FloatAttrebute(at2Rect);
        fl2.mFloat = insetDistance;
        fl2.SetMinMax(0, 4);
        fl2.SetName("Inset");
        attrebutes.Add(fl2);
    }


    public object Execute(object mMesh, object id)
    {
        WallPartItem item = mMesh as WallPartItem;

        if (GetNodes[0].ConnectedNode != null)
            item = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(item, GetNodes[0].ConnectedNode.id);


        FloatAttrebute fl1 = (FloatAttrebute)attrebutes[0];
        extrudeDistance = fl1.mFloat;

        FloatAttrebute fl2 = (FloatAttrebute)attrebutes[1];
        insetDistance = fl2.mFloat;

        WallPartItem output = new WallPartItem();
        Mesh originalMesh = item.mesh;
        Mesh extrudedMesh = new Mesh();


        // Get the normals and vertices of the original mesh
        Vector3[] vertices = originalMesh.vertices;
        Vector3[] normals = originalMesh.normals;
        int[] triangles = originalMesh.triangles;
        Vector2[] UVs = originalMesh.uv;

        // Create new arrays to hold the extruded vertices and triangles
        Vector3[] extrudedVertices = new Vector3[vertices.Length];
        Vector3[] extrudedNormals = new Vector3[normals.Length];
        int[] extrudedTriangles = new int[triangles.Length];
        Vector2[] extrudedUvs = new Vector2[UVs.Length];

        for(int i=0;i<vertices.Length;i++)
        {
            extrudedVertices[i] = vertices[i];
            extrudedVertices[i] += normals[i]*extrudeDistance;
        }

        extrudedMesh.vertices = extrudedVertices;
        extrudedMesh.normals = normals;
        extrudedMesh.triangles = triangles;
        extrudedMesh.uv = originalMesh.uv;

        extrudedMesh = InsetMesh(extrudedMesh,-insetDistance);

        extrudedVertices = extrudedMesh.vertices;

        int OrginaltriangleCount = triangles.Length / 3;


        Vector3[] sideVertices = new Vector3[OrginaltriangleCount*12];
        int[] sideTriangles = new int[OrginaltriangleCount*12];
        Vector2[] sideUVs = new Vector2[OrginaltriangleCount*12];

        int index = vertices.Length / OrginaltriangleCount;
        for (int i=0;i< OrginaltriangleCount;i++)
        {
            int mul = i*12;
            
            sideVertices[mul+2] = vertices[ index * i + 0];
            sideVertices[mul + 1] = extrudedVertices[index * i + 0];
            sideVertices[mul] = vertices[index * i + 1];

            sideVertices[mul +5] = extrudedVertices[index * i + 1];
            sideVertices[mul +4] = vertices[index * i + 1];
            sideVertices[mul +3] = extrudedVertices[index * i + 0];


            sideVertices[mul + 6] = vertices[index * i + 0];
            sideVertices[mul + 7] = extrudedVertices[index * i + 0];
            sideVertices[mul + 8] = vertices[index * i + 2];

            sideVertices[mul + 9] = extrudedVertices[index * i + 2];
            sideVertices[mul + 10] = vertices[index * i + 2];
            sideVertices[mul + 11] = extrudedVertices[index * i + 0];

            sideTriangles[mul] = mul;
            sideTriangles[mul+1] = mul+1;
            sideTriangles[mul+2] = mul + 2;
            sideTriangles[mul+3] = mul + 3;
            sideTriangles[mul+4] = mul + 4;
            sideTriangles[mul+5] = mul + 5;
            sideTriangles[mul+6] = mul + 6;
            sideTriangles[mul+7] = mul + 7;
            sideTriangles[mul+8] = mul + 8;
            sideTriangles[mul+9] = mul + 9;
            sideTriangles[mul+10] = mul + 10;
            sideTriangles[mul+11] = mul + 11;

            sideUVs[mul+2] = new Vector2(0, 1);
            sideUVs[mul+1] = new Vector2(1, 1);
            sideUVs[mul ] = new Vector2(0, 0);

            sideUVs[mul + 5] = new Vector2(1, 0);
            sideUVs[mul + 4] = new Vector2(0, 0);
            sideUVs[mul + 3] = new Vector2(1, 1);

            sideUVs[mul + 6] = new Vector2(0, 1);
            sideUVs[mul + 7] = new Vector2(1, 1);
            sideUVs[mul + 8] = new Vector2(0, 0);

            sideUVs[mul + 9] = new Vector2(1, 0);
            sideUVs[mul + 10] = new Vector2(0, 0);
            sideUVs[mul + 11] = new Vector2(1, 1);
        }

        Mesh sideMesh = new Mesh();
        sideMesh.vertices = sideVertices;
        sideMesh.triangles = sideTriangles;
        sideMesh.uv = sideUVs;
        sideMesh.RecalculateBounds();
        sideMesh.RecalculateNormals();

        Mesh combinedMesh = new Mesh();

        CombineInstance[] combineInstances = new CombineInstance[2];
        combineInstances[0].mesh = extrudedMesh;
        combineInstances[0].transform = Matrix4x4.identity;
        combineInstances[1].mesh = sideMesh;
        combineInstances[1].transform = Matrix4x4.identity;
        combinedMesh.CombineMeshes(combineInstances);
        //sideMesh.RecalculateBounds();
        //sideMesh.RecalculateNormals();
        if(id == 0)
        {

        }
        output.mesh = combinedMesh;
        output.material = item.material;
        return output;
    }


    public static Mesh InsetMesh(Mesh mesh, float insetAmount)
    {
        // get center point of mesh
        Vector3 center = mesh.bounds.center;
        // get all vertices of mesh
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            // calculate vector from center to vertex
            Vector3 offset = vertices[i] - center;
            // normalize the vector
            offset.Normalize();
            // move vertex along the normalized vector by the inset amount
            vertices[i] += offset * insetAmount;
        }
        // create new mesh and assign modified vertices
        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        // copy over any other necessary data such as normals and uv coordinates
        newMesh.normals = mesh.normals;
        newMesh.uv = mesh.uv;
        newMesh.triangles = mesh.triangles;
        return newMesh;
    }

}