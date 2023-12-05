using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WallDesigner;

public class Split : FunctionItem, IFunctionItem
{
    private float distance = 0f;
    private bool autoAdjust=false;
    public Split() 
    {
        Init();
        Name = "Split";
        ClassName = typeof(Split).FullName;
        basecolor = Color.white;
        GiveNodes = new List<Node>();
        GetNodes = new List<Node>();

        GiveNode node = new GiveNode();
        node.AttachedFunctionItem = this;
        GiveNodes.Add((Node)node);

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        gnode.id = 0;
        GetNodes.Add(gnode);

        myFunction = Execute;
        CalculateRect();
        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        ToggleAttribute ta1 = new ToggleAttribute(at1Rect);
        ta1.mToggle = autoAdjust;
        ta1.SetName("Auto adjust");
        attrebutes.Add(ta1);


        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y + 20, rect.width, rect.height);
        FloatAttrebute fl1 = new FloatAttrebute(at2Rect);
        fl1.mFloat = distance;
        fl1.SetName("Distance");
        attrebutes.Add(fl1);
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

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        ta1.mToggle = item.attributeValue[0] == "True";
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

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        string stringtoggle = ta1.mToggle.ToString();
        item.attributeValue.Add(stringtoggle);

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[1];
        string stringtexturePath = att1.mFloat.ToString();
        item.attributeValue.Add(stringtexturePath);

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
        FloatAttrebute fa1 = (FloatAttrebute)attrebutes[1];
        distance = (float)fa1.GetValue();

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        autoAdjust = (bool)ta1.GetValue();

        List<WallPartItem> wpi = (List<WallPartItem>)mMesh;

        if (GetNodes[0].ConnectedNode != null)
            wpi = (List<WallPartItem>)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);

        for (int i = 0; i < wpi.Count; i++)
        {
            if (i > 0)
            {
                wpi[i] = CombineItems.CombineTwoItem(wpi[i - 1].mesh, wpi[i].mesh, wpi[i - 1].material, wpi[i].material);
            }
        }

        WallPartItem wallPartItem = wpi[wpi.Count - 1];

        List<WallPartItem> SlicedItems = new List<WallPartItem>();

        Mesh mesh = wallPartItem.mesh;
        mesh.RecalculateNormals();
        int numberOfSlices=0;
        float newDistance = 0;
        if (autoAdjust)
        {
            numberOfSlices = (int)(mesh.bounds.size.y / distance);
            float remain = mesh.bounds.size.y % distance;
            if(remain <= distance/2)
            {
                newDistance = distance + (remain/numberOfSlices);
            }
            else
            {
                float menus = (distance - remain) / (numberOfSlices + 1);
                newDistance = distance - menus;
                numberOfSlices++;
            }
        }
        else
        {
            numberOfSlices = (int)(mesh.bounds.size.y / distance);
            newDistance = distance;
        }

        //Debug.Log(distance+" <=Dist -- N=> "+numberOfSlices);
        Mesh upperMesh = new Mesh();
        Mesh lowerMesh = new Mesh();

        for (int j=0;j<numberOfSlices;j++)
        {

            upperMesh = new Mesh();
            lowerMesh = new Mesh();

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

            float cutY = 0;

            float minY = float.MaxValue;
            foreach (Vector3 vertex in vertices)
            {
                if (vertex.y < minY)
                {
                    minY = vertex.y;
                }
            }

            cutY = minY + newDistance;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                if (vertices[triangles[i]].y > cutY && vertices[triangles[i + 1]].y > cutY && vertices[triangles[i + 2]].y > cutY)
                {
                    upperVertices.Add(vertices[triangles[i]]);
                    upperVertices.Add(vertices[triangles[i + 1]]);
                    upperVertices.Add(vertices[triangles[i + 2]]);

                    UpperUV.Add(uv[triangles[i]]);
                    UpperUV.Add(uv[triangles[i + 1]]);
                    UpperUV.Add(uv[triangles[i + 2]]);

                    UpperNormals.Add(normals[triangles[i]]);
                    UpperNormals.Add(normals[triangles[i + 1]]);
                    UpperNormals.Add(normals[triangles[i + 2]]);

                    upperTriangles.Add(upperTriangles.Count);
                    upperTriangles.Add(upperTriangles.Count);
                    upperTriangles.Add(upperTriangles.Count);

                }
                else if (vertices[triangles[i]].y < cutY && vertices[triangles[i + 1]].y < cutY && vertices[triangles[i + 2]].y < cutY)
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

                    lowerTriangles.Add(lowerTriangles.Count);
                    lowerTriangles.Add(lowerTriangles.Count);
                    lowerTriangles.Add(lowerTriangles.Count);
                }
                else
                {
                    if (vertices[triangles[i]].y < cutY)
                    {
                        lowerVertices.Add(vertices[triangles[i]]);
                        //vertices[triangles[i]].z = cutZ;
                        Vector3 v = vertices[triangles[i]];
                        v.y = cutY;
                        upperVertices.Add(v);
                    }
                    else
                    {
                        upperVertices.Add(vertices[triangles[i]]);
                        //vertices[triangles[i]].z = cutZ;
                        Vector3 v = vertices[triangles[i]];
                        v.y = cutY;
                        lowerVertices.Add(v);

                    }

                    if (vertices[triangles[i + 1]].y < cutY)
                    {
                        lowerVertices.Add(vertices[triangles[i + 1]]);
                        //vertices[triangles[i+1]].z = cutZ;
                        Vector3 v = vertices[triangles[i + 1]];
                        v.y = cutY;
                        upperVertices.Add(v);

                    }
                    else
                    {
                        upperVertices.Add(vertices[triangles[i + 1]]);
                        //vertices[triangles[i + 1]].z = cutZ;
                        Vector3 v = vertices[triangles[i + 1]];
                        v.y = cutY;
                        lowerVertices.Add(v);
                    }

                    if (vertices[triangles[i + 2]].y < cutY)
                    {
                        lowerVertices.Add(vertices[triangles[i + 2]]);
                        //vertices[triangles[i + 2]].z = cutZ;
                        Vector3 v = vertices[triangles[i + 2]];
                        v.y = cutY;
                        upperVertices.Add(v);
                    }
                    else
                    {
                        upperVertices.Add(vertices[triangles[i + 2]]);
                        //vertices[triangles[i + 2]].z = cutZ;
                        Vector3 v = vertices[triangles[i + 2]];
                        v.y = cutY;
                        lowerVertices.Add(v);
                    }

                    UpperUV.Add(uv[triangles[i]]);
                    UpperUV.Add(uv[triangles[i + 1]]);
                    UpperUV.Add(uv[triangles[i + 2]]);

                    UpperNormals.Add(normals[triangles[i]]);
                    UpperNormals.Add(normals[triangles[i + 1]]);
                    UpperNormals.Add(normals[triangles[i + 2]]);

                    upperTriangles.Add(upperTriangles.Count);
                    upperTriangles.Add(upperTriangles.Count);
                    upperTriangles.Add(upperTriangles.Count);

                    lowerUV.Add(uv[triangles[i]]);
                    lowerUV.Add(uv[triangles[i + 1]]);
                    lowerUV.Add(uv[triangles[i + 2]]);

                    lowerNormals.Add(normals[triangles[i]]);
                    lowerNormals.Add(normals[triangles[i + 1]]);
                    lowerNormals.Add(normals[triangles[i + 2]]);

                    lowerTriangles.Add(lowerTriangles.Count);
                    lowerTriangles.Add(lowerTriangles.Count);
                    lowerTriangles.Add(lowerTriangles.Count);


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

            WallPartItem tempitem = new WallPartItem();
            tempitem.mesh = lowerMesh;
            Material mat1 = new Material(Shader.Find("Standard"));
            tempitem.material.Add(mat1);
            mesh = upperMesh;
            SlicedItems.Add(tempitem);
        }
        WallPartItem titem = new WallPartItem();
        titem.mesh = upperMesh;
        Material mat2 = new Material(Shader.Find("Standard"));
        titem.material.Add(mat2);
        SlicedItems.Add(titem);

        return SlicedItems;
    }
}
