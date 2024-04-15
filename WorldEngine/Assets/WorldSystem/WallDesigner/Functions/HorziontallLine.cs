using System;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

[System.Serializable]
public class HorziontallLine : FunctionItem, IFunctionItem
{
    private WallItem UpperPart;
    private WallItem LowerPart;
    private float distance = 1;
    public HorziontallLine(int gets, int gives)
    {
        Init();
        Name = "Horrizontal Slicer";
        UpperPart = new WallItem();
        LowerPart = new WallItem();
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
        
        ToggleAttribute ta1 = new ToggleAttribute(at1Rect, this);
        ta1.mToggle = true;
        ta1.SetName("fromTop");
        attrebutes.Add(ta1);

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y +20, rect.width, rect.height);

        FloatAttrebute fl1 = new FloatAttrebute(at2Rect, this);
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

        if (item.givenodeConnectedFI.Count > 1)
            GiveNodes[1].ConnectedNode = functionItems[item.givenodeConnectedFI[1]].GetNodes[item.givenodeItems[1]];
    }


    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        ta1.mToggle = item.attributeValue[0]=="True";
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

        if (GiveNodes[1].ConnectedNode != null)
        {
            int connectedGiveNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GiveNodes[1].ConnectedNode.AttachedFunctionItem);
            item.givenodeConnectedFI.Add(connectedGiveNodeNumber);
            item.givenodeItems.Add(GiveNodes[1].ConnectedNode.id);
        }
        return item;
    }

    public object Execute(object mMesh,object id)
    {
        FloatAttrebute fa1 = (FloatAttrebute)attrebutes[1];
        distance = (float)fa1.GetValue();

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        bool fromTop = ta1.mToggle;

        List<WallPartItem> wpi = (List<WallPartItem>)mMesh;

        //WallPartItem wpi = new WallPartItem();

        if (GetNodes[0].ConnectedNode != null)
            wpi = (List<WallPartItem>)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);

        UpperPart = new WallItem();
        LowerPart = new WallItem();

        for (int j = 0; j < wpi.Count; j++)
        {
            Mesh mesh = wpi[j].mesh;
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

            float cutZ = 0;

            if (fromTop)
            {
                float highestZ = float.MinValue;
                foreach (Vector3 vertex in vertices)
                {
                    if (vertex.z > highestZ)
                    {
                        highestZ = vertex.z;
                    }
                }

                cutZ = highestZ - distance;
            }
            else
            {
                float minZ = float.MaxValue;
                foreach (Vector3 vertex in vertices)
                {
                    if (vertex.z < minZ)
                    {
                        minZ = vertex.z;
                    }
                }

                cutZ = minZ + distance;
            }



            for (int i = 0; i < triangles.Length; i += 3)
            {
                if (vertices[triangles[i]].z > cutZ && vertices[triangles[i + 1]].z > cutZ && vertices[triangles[i + 2]].z > cutZ)
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
                else if (vertices[triangles[i]].z < cutZ && vertices[triangles[i + 1]].z < cutZ && vertices[triangles[i + 2]].z < cutZ)
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
                    if (vertices[triangles[i]].z < cutZ)
                    {
                        lowerVertices.Add(vertices[triangles[i]]);
                        //vertices[triangles[i]].z = cutZ;
                        Vector3 v = vertices[triangles[i]];
                        v.z = cutZ;
                        upperVertices.Add(v);
                    }
                    else
                    {
                        upperVertices.Add(vertices[triangles[i]]);
                        //vertices[triangles[i]].z = cutZ;
                        Vector3 v = vertices[triangles[i]];
                        v.z = cutZ;
                        lowerVertices.Add(v);

                    }

                    if (vertices[triangles[i + 1]].z < cutZ)
                    {
                        lowerVertices.Add(vertices[triangles[i + 1]]);
                        //vertices[triangles[i+1]].z = cutZ;
                        Vector3 v = vertices[triangles[i + 1]];
                        v.z = cutZ;
                        upperVertices.Add(v);

                    }
                    else
                    {
                        upperVertices.Add(vertices[triangles[i + 1]]);
                        //vertices[triangles[i + 1]].z = cutZ;
                        Vector3 v = vertices[triangles[i + 1]];
                        v.z = cutZ;
                        lowerVertices.Add(v);
                    }

                    if (vertices[triangles[i + 2]].z < cutZ)
                    {
                        lowerVertices.Add(vertices[triangles[i + 2]]);
                        //vertices[triangles[i + 2]].z = cutZ;
                        Vector3 v = vertices[triangles[i + 2]];
                        v.z = cutZ;
                        upperVertices.Add(v);
                    }
                    else
                    {
                        upperVertices.Add(vertices[triangles[i + 2]]);
                        //vertices[triangles[i + 2]].z = cutZ;
                        Vector3 v = vertices[triangles[i + 2]];
                        v.z = cutZ;
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

            List<Material> mats = new List<Material>();
            if (wpi[j].material.Count > 0)
            {
                //int count = wallitem.material.Count;
                //wallitem.material.Clear();

                for (int i = 0; i < wpi[j].material.Count; i++)
                {
                    Material mat1 = new Material(Shader.Find("Standard"));
                    mat1.color = wpi[j].material[i].color;
                    if (wpi[j].material[i].mainTexture != null)
                        mat1.mainTexture = wpi[j].material[i].mainTexture;
                    mats.Add(mat1);
                }
                //wpi.material.Clear();
                //wpi.material = mats;
            }

            WallPartItem item = new WallPartItem();
            item.material = mats;
            if ((int)id == 0)
            {
                item.mesh = upperMesh;
                UpperPart.wallPartItems.Add(item);
            }
            else
            {
                item.mesh = lowerMesh;
                LowerPart.wallPartItems.Add(item);
                //return LowerPart;
            }
        }

        if ((int)id == 0)
        {
            //UpperPart.mesh = upperMesh;
            //UpperPart.material = mats;
            return UpperPart;
        }
        else
        {
            //LowerPart.mesh = lowerMesh;
            //LowerPart.material = mats;
            return LowerPart;
        }
    }
}
