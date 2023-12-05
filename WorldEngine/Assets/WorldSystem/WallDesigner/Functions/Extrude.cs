using System.Collections.Generic;
using UnityEngine;
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
        fl1.SetMinMax(-5, 10);
        fl1.SetName("Extrude");
        attrebutes.Add(fl1);

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y + 15, rect.width, rect.height);

        FloatAttrebute fl2 = new FloatAttrebute(at2Rect);
        fl2.mFloat = insetDistance;
        fl2.SetMinMax(0, 4);
        fl2.SetName("Inset");
        attrebutes.Add(fl2);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.getnodeConnectedFI.Count > 0)
            GetNodes[0].ConnectedNode = functionItems[item.getnodeConnectedFI[0]].GiveNodes[item.getnodeItems[0]];

        if (item.givenodeConnectedFI.Count > 0)
            GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GetNodes[item.givenodeItems[0]];

        if (item.givenodeConnectedFI.Count > 1)
            GiveNodes[1].ConnectedNode = functionItems[item.givenodeConnectedFI[1]].GetNodes[item.givenodeItems[1]];
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

        if (GiveNodes[1].ConnectedNode != null)
        {
            int connectedGiveNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GiveNodes[1].ConnectedNode.AttachedFunctionItem);
            item.givenodeConnectedFI.Add(connectedGiveNodeNumber);
            item.givenodeItems.Add(GiveNodes[1].ConnectedNode.id);
        }
        return item;
    }

    public object Execute(object mMesh, object id)
    {
        WallItem item = mMesh as WallItem;

        if (GetNodes[0].ConnectedNode != null)
            item = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(item, GetNodes[0].ConnectedNode.id);


        FloatAttrebute fl1 = (FloatAttrebute)attrebutes[0];
        extrudeDistance = (float)fl1.GetValue();

        FloatAttrebute fl2 = (FloatAttrebute)attrebutes[1];
        insetDistance = (float)fl2.GetValue();

        List<WallPartItem> output = new List<WallPartItem>();

        for (int j = 0; j < item.wallPartItems.Count; j++)
        {

            if (item.wallPartItems[j] == null)
                continue;

            Mesh originalMesh = item.wallPartItems[j].mesh;
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

            for (int i = 0; i < vertices.Length; i++)
            {
                extrudedVertices[i] = vertices[i];
                extrudedVertices[i] += normals[i] * extrudeDistance;
            }

            extrudedMesh.vertices = extrudedVertices;
            extrudedMesh.normals = normals;
            extrudedMesh.triangles = triangles;
            extrudedMesh.uv = originalMesh.uv;

            extrudedMesh = InsetMesh(extrudedMesh, -insetDistance);

            extrudedVertices = extrudedMesh.vertices;

            int OrginaltriangleCount = triangles.Length / 3;


            List<Vector3> sideVertices = new List<Vector3>();
            List<int> sideTriangles = new List<int>();
            List<Vector2> sideUVs = new List<Vector2>();

            int index = vertices.Length / OrginaltriangleCount;


            for (int i = 0; i < triangles.Length; i += 3)
            {
                if (IsEdgeOnBorder(vertices[triangles[i]], vertices[triangles[i + 1]], originalMesh))
                {
                    Vector3 vertex1 = vertices[triangles[i]];
                    Vector3 vertex2 = vertices[triangles[i + 1]];
                    Vector3 exvertex1 = extrudedVertices[triangles[i]];
                    Vector3 exvertex2 = extrudedVertices[triangles[i + 1]];

                    sideVertices.Add(vertex1);//0 w = 0 -- h = 0
                    sideVertices.Add(vertex2);//1 w = 1 -- h = 0
                    sideVertices.Add(exvertex1);//2 w = 0 -- h = 1
                    sideVertices.Add(exvertex2);//3 w = 1 -- h =1

                    int v1 = sideVertices.Count - 4;
                    int v2 = sideVertices.Count - 3;
                    int v3 = sideVertices.Count - 2;
                    int v4 = sideVertices.Count - 1;

                    sideUVs.Add(new Vector2(0, 0));
                    sideUVs.Add(new Vector2(1, 0));
                    sideUVs.Add(new Vector2(0, 1));
                    sideUVs.Add(new Vector2(1, 1));

                    sideTriangles.Add(v2);
                    sideTriangles.Add(v3);
                    sideTriangles.Add(v1);

                    sideTriangles.Add(v3);
                    sideTriangles.Add(v2);
                    sideTriangles.Add(v4);
                }

                if (IsEdgeOnBorder(vertices[triangles[i]], vertices[triangles[i + 2]], originalMesh))
                {
                    Vector3 vertex1 = vertices[triangles[i]];
                    Vector3 vertex2 = vertices[triangles[i + 2]];
                    Vector3 exvertex1 = extrudedVertices[triangles[i]];
                    Vector3 exvertex2 = extrudedVertices[triangles[i + 2]];

                    sideVertices.Add(vertex1);//0 w = 0 -- h = 0
                    sideVertices.Add(vertex2);//1 w = 1 -- h = 0
                    sideVertices.Add(exvertex1);//2 w = 0 -- h = 1
                    sideVertices.Add(exvertex2);//3 w = 1 -- h =1

                    int v1 = sideVertices.Count - 4;
                    int v2 = sideVertices.Count - 3;
                    int v3 = sideVertices.Count - 2;
                    int v4 = sideVertices.Count - 1;

                    sideUVs.Add(new Vector2(0, 0));
                    sideUVs.Add(new Vector2(1, 0));
                    sideUVs.Add(new Vector2(0, 1));
                    sideUVs.Add(new Vector2(1, 1));

                    sideTriangles.Add(v1);
                    sideTriangles.Add(v3);
                    sideTriangles.Add(v2);

                    sideTriangles.Add(v4);
                    sideTriangles.Add(v2);
                    sideTriangles.Add(v3);
                }

                if (IsEdgeOnBorder(vertices[triangles[i + 1]], vertices[triangles[i + 2]], originalMesh))
                {
                    Vector3 vertex1 = vertices[triangles[i + 1]];
                    Vector3 vertex2 = vertices[triangles[i + 2]];
                    Vector3 exvertex1 = extrudedVertices[triangles[i + 1]];
                    Vector3 exvertex2 = extrudedVertices[triangles[i + 2]];

                    sideVertices.Add(vertex1);//0 w = 0 -- h = 0
                    sideVertices.Add(vertex2);//1 w = 1 -- h = 0
                    sideVertices.Add(exvertex1);//2 w = 0 -- h = 1
                    sideVertices.Add(exvertex2);//3 w = 1 -- h =1

                    int v1 = sideVertices.Count - 4;
                    int v2 = sideVertices.Count - 3;
                    int v3 = sideVertices.Count - 2;
                    int v4 = sideVertices.Count - 1;

                    sideUVs.Add(new Vector2(0, 0));
                    sideUVs.Add(new Vector2(1, 0));
                    sideUVs.Add(new Vector2(0, 1));
                    sideUVs.Add(new Vector2(1, 1));

                    sideTriangles.Add(v2);
                    sideTriangles.Add(v3);
                    sideTriangles.Add(v1);

                    sideTriangles.Add(v3);
                    sideTriangles.Add(v2);
                    sideTriangles.Add(v4);
                }
            }

            /*for (int i=0;i< OrginaltriangleCount;i++)
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
            }*/

            Mesh sideMesh = new Mesh();
            sideMesh.vertices = sideVertices.ToArray();
            sideMesh.triangles = sideTriangles.ToArray();
            sideMesh.uv = sideUVs.ToArray();
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


            WallPartItem itemTemp = new WallPartItem();
            if ((int)id == 0)
            {
                itemTemp.mesh = extrudedMesh;
            }
            else
            {
                itemTemp.mesh = sideMesh;
            }

            itemTemp.material = AddMaterial.CopyMaterials(item.wallPartItems[j]);
            output.Add(itemTemp);
        }
        
        return output;
    }



    static bool IsTriangleOnEdgeOfMesh(Mesh mesh, int triangleIndex)
    {
        int[] triangles = mesh.triangles;
        int[] edges = new int[] { triangles[triangleIndex], triangles[triangleIndex + 1], triangles[triangleIndex + 2] };

        for (int i = 0; i < edges.Length; i += 2)
        {
            int edge0 = edges[i];
            int edge1 = edges[i + 1];
            bool isEdge0OnBoundary = false;
            bool isEdge1OnBoundary = false;

            for (int j = 0; j < triangles.Length; j += 3)
            {
                if (j / 3 != triangleIndex)
                {
                    if ((triangles[j] == edge0 && triangles[j + 1] == edge1) ||
                        (triangles[j] == edge1 && triangles[j + 1] == edge0) ||
                        (triangles[j + 1] == edge0 && triangles[j + 2] == edge1) ||
                        (triangles[j + 1] == edge1 && triangles[j + 2] == edge0) ||
                        (triangles[j + 2] == edge0 && triangles[j] == edge1) ||
                        (triangles[j + 2] == edge1 && triangles[j] == edge0))
                    {
                        isEdge0OnBoundary = true;
                        break;
                    }
                }
            }

            for (int j = 0; j < triangles.Length; j += 3)
            {
                if (j / 3 != triangleIndex)
                {
                    if ((triangles[j] == edge0 && triangles[j + 1] == edge1) ||
                        (triangles[j] == edge1 && triangles[j + 1] == edge0) ||
                        (triangles[j + 1] == edge0 && triangles[j + 2] == edge1) ||
                        (triangles[j + 1] == edge1 && triangles[j + 2] == edge0) ||
                        (triangles[j + 2] == edge0 && triangles[j] == edge1) ||
                        (triangles[j + 2] == edge1 && triangles[j] == edge0))
                    {
                        isEdge1OnBoundary = true;
                        break;
                    }
                }
            }

            if (!isEdge0OnBoundary || !isEdge1OnBoundary)
            {
                return true;
            }
        }

        return false;
    }

    bool IsEdgeOnBorder(Vector3 v1, Vector3 v2, Mesh mesh)
    {
        int counter = 0;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            int triangleIndex1 = mesh.triangles[i];
            int triangleIndex2 = mesh.triangles[i + 1];
            int triangleIndex3 = mesh.triangles[i + 2];

            Vector3 vertex1 = mesh.vertices[triangleIndex1];
            Vector3 vertex2 = mesh.vertices[triangleIndex2];
            Vector3 vertex3 = mesh.vertices[triangleIndex3];

            if ((vertex1 == v1 && vertex2 == v2) || (vertex1 == v2 && vertex2 == v1))
            {
                // Found edge shared by two triangles
                //return false;
                counter++;
            }
            else if ((vertex2 == v1 && vertex3 == v2) || (vertex2 == v2 && vertex3 == v1))
            {
                // Found edge shared by two triangles
                counter++;
            }
            else if ((vertex3 == v1 && vertex1 == v2) || (vertex3 == v2 && vertex1 == v1))
            {
                // Found edge shared by two triangles
                counter++;
                //return false;
            }

            if (counter > 1)
                return false;

        }
        // Edge not shared by two triangles, is on border
        return true;
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