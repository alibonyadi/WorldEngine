using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class VerticalLineNew : FunctionItem, IFunctionItem
{
    private WallItem LeftPart;
    private WallItem RightPart;
    private float distance = 1;
    private bool is6VertexPolygon = false;

    public VerticalLineNew(int gets, int gives)
    {
        Init();
        Name = "Vertical Edge Slicer";
        LeftPart = new WallItem();
        RightPart = new WallItem();
        ClassName = typeof(VerticalLineNew).FullName;
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
        ta1.SetName("fromLeft");
        attrebutes.Add(ta1);

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y + 20, rect.width, rect.height);

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
        FloatAttrebute fa1 = (FloatAttrebute)attrebutes[1];
        distance = (float)fa1.GetValue();

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        bool fromLeft = (bool)ta1.GetValue();



        WallItem wpi = (WallItem)mMesh;

        //WallPartItem wpi = new WallPartItem();

        if (GetNodes[0].ConnectedNode != null)
            wpi = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);

        LeftPart = new WallItem();
        RightPart =new WallItem();

        for (int l = 0; l < wpi.wallPartItems.Count; l++)
        {
            Mesh mesh = wpi.wallPartItems[l].mesh;
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

            float cutx = 0;

            if (fromLeft)
            {
                float highestx = float.MinValue;
                foreach (Vector3 vertex in vertices)
                {
                    if (vertex.x > highestx)
                    {
                        highestx = vertex.x;
                    }
                }

                cutx = highestx - distance;
            }
            else
            {
                float minx = float.MaxValue;
                foreach (Vector3 vertex in vertices)
                {
                    if (vertex.x < minx)
                    {
                        minx = vertex.x;
                    }
                }

                cutx = minx + distance;
            }


            int temp4Index = 0;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                Vector3 v = new Vector3();
                if (!is6VertexPolygon)
                {
                    if (i % 6 > 0)//Second triangle of polygon
                    {
                        if (fromLeft)
                        {
                            upperVertices.Add(vertices[triangles[i]]);
                            v = vertices[triangles[i + 1]];
                            v += (vertices[triangles[i]] - vertices[triangles[i + 1]]).normalized * distance;
                            lowerVertices.Add(v);

                            /*lowerVertices.Add(vertices[triangles[i + 1]]);
                            //v = vertices[triangles[i+1]];
                            //v += (vertices[triangles[i+1]]- vertices[triangles[i]]).normalized * distance;
                            upperVertices.Add(v);

                            upperVertices.Add(vertices[triangles[i + 2]]);
                            v = vertices[triangles[i - 3]];
                            v += (vertices[triangles[i +2]] - vertices[triangles[i -3]]).normalized * distance;
                            lowerVertices.Add(v);*/
                        }
                        else
                        {
                            upperVertices.Add(vertices[triangles[i]]);
                            v = vertices[triangles[i]];
                            v += (vertices[triangles[i + 1]] - vertices[triangles[i]]).normalized * distance;
                            lowerVertices.Add(v);

                            /*lowerVertices.Add(vertices[triangles[i + 1]]);
                            //v = vertices[triangles[i + 1]];
                            //v += (vertices[triangles[i]] - vertices[triangles[i + 1]]).normalized * distance;
                            upperVertices.Add(v);

                            upperVertices.Add(vertices[triangles[i + 2]]);
                            v = vertices[triangles[i + 2]];
                            v += (vertices[triangles[i -3]] - vertices[triangles[i +2]]).normalized * distance;
                            lowerVertices.Add(v);*/
                        }

                        UpperUV.Add(uv[triangles[i]]);
                        lowerUV.Add(uv[triangles[i]]);

                        UpperNormals.Add(normals[triangles[i]]);

                        upperTriangles.Add(temp4Index);
                        upperTriangles.Add(temp4Index - 1);
                        upperTriangles.Add(temp4Index - 2);

                        lowerNormals.Add(normals[triangles[i]]);

                        lowerTriangles.Add(temp4Index);
                        lowerTriangles.Add(temp4Index - 1);
                        lowerTriangles.Add(temp4Index - 2);

                        temp4Index += 1;
                    }
                    else
                    {
                        if (fromLeft)
                        {
                            lowerVertices.Add(vertices[triangles[i]]);
                            v = vertices[triangles[i]];
                            v += (vertices[triangles[i + 1]] - vertices[triangles[i]]).normalized * distance;
                            upperVertices.Add(v);

                            upperVertices.Add(vertices[triangles[i + 1]]);
                            //v = vertices[triangles[i+1]];
                            //v += (vertices[triangles[i+1]]- vertices[triangles[i]]).normalized * distance;
                            lowerVertices.Add(v);

                            lowerVertices.Add(vertices[triangles[i + 2]]);
                            v = vertices[triangles[i + 2]];
                            v += (vertices[triangles[i + 3]] - vertices[triangles[i + 2]]).normalized * distance;
                            upperVertices.Add(v);
                        }
                        else
                        {
                            lowerVertices.Add(vertices[triangles[i]]);
                            v = vertices[triangles[i + 1]];
                            v += (vertices[triangles[i]] - vertices[triangles[i + 1]]).normalized * distance;
                            upperVertices.Add(v);

                            upperVertices.Add(vertices[triangles[i + 1]]);
                            //v = vertices[triangles[i + 1]];
                            //v += (vertices[triangles[i]] - vertices[triangles[i + 1]]).normalized * distance;
                            lowerVertices.Add(v);

                            lowerVertices.Add(vertices[triangles[i + 2]]);
                            v = vertices[triangles[i + 3]];
                            v += (vertices[triangles[i + 2]] - vertices[triangles[i + 3]]).normalized * distance;
                            upperVertices.Add(v);
                        }

                        UpperUV.Add(uv[triangles[i]]);
                        UpperUV.Add(uv[triangles[i + 1]]);
                        UpperUV.Add(uv[triangles[i + 2]]);

                        lowerUV.Add(uv[triangles[i]]);
                        lowerUV.Add(uv[triangles[i + 1]]);
                        lowerUV.Add(uv[triangles[i + 2]]);

                        UpperNormals.Add(normals[triangles[i]]);
                        UpperNormals.Add(normals[triangles[i + 1]]);
                        UpperNormals.Add(normals[triangles[i + 2]]);

                        upperTriangles.Add(temp4Index);
                        upperTriangles.Add(temp4Index+1);
                        upperTriangles.Add(temp4Index+2);

                        lowerNormals.Add(normals[triangles[i]]);
                        lowerNormals.Add(normals[triangles[i + 1]]);
                        lowerNormals.Add(normals[triangles[i + 2]]);

                        lowerTriangles.Add(temp4Index);
                        lowerTriangles.Add(temp4Index+1);
                        lowerTriangles.Add(temp4Index+2);

                        temp4Index += 3;

                    }



                    //}
                }
                else
                {
                    if (i % 6 > 0)//Second triangle of polygon
                    {
                        if (fromLeft)
                        {
                            upperVertices.Add(vertices[triangles[i]]);
                            v = vertices[triangles[i + 1]];
                            v += (vertices[triangles[i]] - vertices[triangles[i + 1]]).normalized * distance;
                            lowerVertices.Add(v);

                            lowerVertices.Add(vertices[triangles[i + 1]]);
                            //v = vertices[triangles[i+1]];
                            //v += (vertices[triangles[i+1]]- vertices[triangles[i]]).normalized * distance;
                            upperVertices.Add(v);

                            upperVertices.Add(vertices[triangles[i + 2]]);
                            v = vertices[triangles[i - 3]];
                            v += (vertices[triangles[i +2]] - vertices[triangles[i -3]]).normalized * distance;
                            lowerVertices.Add(v);
                        }
                        else
                        {
                            upperVertices.Add(vertices[triangles[i]]);
                            v = vertices[triangles[i]];
                            v += (vertices[triangles[i + 1]] - vertices[triangles[i]]).normalized * distance;
                            lowerVertices.Add(v);

                            lowerVertices.Add(vertices[triangles[i + 1]]);
                            //v = vertices[triangles[i + 1]];
                            //v += (vertices[triangles[i]] - vertices[triangles[i + 1]]).normalized * distance;
                            upperVertices.Add(v);

                            upperVertices.Add(vertices[triangles[i + 2]]);
                            v = vertices[triangles[i + 2]];
                            v += (vertices[triangles[i -3]] - vertices[triangles[i +2]]).normalized * distance;
                            lowerVertices.Add(v);
                        }

                        UpperUV.Add(uv[triangles[i]]);
                        lowerUV.Add(uv[triangles[i]]);

                        UpperNormals.Add(normals[triangles[i]]);

                        upperTriangles.Add(upperTriangles.Count);
                        upperTriangles.Add(upperTriangles.Count - 2);
                        upperTriangles.Add(upperTriangles.Count - 4);

                        lowerNormals.Add(normals[triangles[i]]);

                        lowerTriangles.Add(lowerTriangles.Count);
                        lowerTriangles.Add(lowerTriangles.Count - 2);
                        lowerTriangles.Add(lowerTriangles.Count - 4);
                    }
                    else
                    {
                        if (fromLeft)
                        {
                            lowerVertices.Add(vertices[triangles[i]]);
                            v = vertices[triangles[i]];
                            v += (vertices[triangles[i + 1]] - vertices[triangles[i]]).normalized * distance;
                            upperVertices.Add(v);

                            upperVertices.Add(vertices[triangles[i + 1]]);
                            //v = vertices[triangles[i+1]];
                            //v += (vertices[triangles[i+1]]- vertices[triangles[i]]).normalized * distance;
                            lowerVertices.Add(v);

                            lowerVertices.Add(vertices[triangles[i + 2]]);
                            v = vertices[triangles[i + 2]];
                            v += (vertices[triangles[i + 3]] - vertices[triangles[i + 2]]).normalized * distance;
                            upperVertices.Add(v);
                        }
                        else
                        {
                            lowerVertices.Add(vertices[triangles[i]]);
                            v = vertices[triangles[i + 1]];
                            v += (vertices[triangles[i]] - vertices[triangles[i + 1]]).normalized * distance;
                            upperVertices.Add(v);

                            upperVertices.Add(vertices[triangles[i + 1]]);
                            //v = vertices[triangles[i + 1]];
                            //v += (vertices[triangles[i]] - vertices[triangles[i + 1]]).normalized * distance;
                            lowerVertices.Add(v);

                            lowerVertices.Add(vertices[triangles[i + 2]]);
                            v = vertices[triangles[i + 3]];
                            v += (vertices[triangles[i + 2]] - vertices[triangles[i + 3]]).normalized * distance;
                            upperVertices.Add(v);
                        }

                        UpperUV.Add(uv[triangles[i]]);
                        UpperUV.Add(uv[triangles[i + 1]]);
                        UpperUV.Add(uv[triangles[i + 2]]);

                        lowerUV.Add(uv[triangles[i]]);
                        lowerUV.Add(uv[triangles[i + 1]]);
                        lowerUV.Add(uv[triangles[i + 2]]);

                        UpperNormals.Add(normals[triangles[i]]);
                        UpperNormals.Add(normals[triangles[i + 1]]);
                        UpperNormals.Add(normals[triangles[i + 2]]);

                        upperTriangles.Add(upperTriangles.Count);
                        upperTriangles.Add(upperTriangles.Count);
                        upperTriangles.Add(upperTriangles.Count);

                        lowerNormals.Add(normals[triangles[i]]);
                        lowerNormals.Add(normals[triangles[i + 1]]);
                        lowerNormals.Add(normals[triangles[i + 2]]);

                        lowerTriangles.Add(lowerTriangles.Count);
                        lowerTriangles.Add(lowerTriangles.Count);
                        lowerTriangles.Add(lowerTriangles.Count);
                    }
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

            //mats = AddMaterial.CopyMaterials(wpi);

            if (wpi.wallPartItems[l].material.Count > 0)
            {
                //int count = wallitem.material.Count;
                //wallitem.material.Clear();

                for (int i = 0; i < wpi.wallPartItems[l].material.Count; i++)
                {
                    Material mat1 = new Material(Shader.Find("Standard"));
                    mat1.color = wpi.wallPartItems[l].material[i].color;
                    if (wpi.wallPartItems[l].material[i].mainTexture != null)
                        mat1.mainTexture = wpi.wallPartItems[l].material[i].mainTexture;
                    mats.Add(mat1);
                }
                //wpi.material.Clear();
                //wpi.material = mats;
            }

            WallPartItem wallPartItem = new WallPartItem();

            if ((int)id == 0)
            {
                wallPartItem.mesh = upperMesh;
                wallPartItem.material = mats;
                LeftPart.wallPartItems.Add(wallPartItem);
            }
            else
            {
                wallPartItem.mesh = lowerMesh;
                wallPartItem.material = mats;
                RightPart.wallPartItems.Add(wallPartItem);
                //return RightPart;
            }
        }



        if ((int)id == 0)
        {
            //LeftPart[l].mesh = upperMesh;
            //LeftPart[l].material = mats;
            return  LeftPart;
        }
        else
        {
            //RightPart[l].mesh = lowerMesh;
            //RightPart[l].material = mats;
            return RightPart;
        }
    }

    private float getCutXPos(Vector3 v1,Vector3 v2,bool fromLeft)
    {
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(v1);
        vertices.Add(v2);
        float cutx = 0;
        if (fromLeft)
        {
            float highestx = float.MinValue;
            foreach (Vector3 vertex in vertices)
            {
                if (vertex.x > highestx)
                {
                    highestx = vertex.x;
                }
            }

            cutx = highestx - distance;
        }
        else
        {
            float minx = float.MaxValue;
            foreach (Vector3 vertex in vertices)
            {
                if (vertex.x < minx)
                {
                    minx = vertex.x;
                }
            }

            cutx = minx + distance;
        }

        return cutx;
    }
}
