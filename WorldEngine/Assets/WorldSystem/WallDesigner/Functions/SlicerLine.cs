using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class SlicerLine : FunctionItem, IFunctionItem
{
    private WallItem LeftPart;
    private WallItem RightPart;
    private float distance = 1;
    private bool is6VertexPolygon = false;
    private bool Xdirection = true;
    private bool XdirectionTemp = true;
    private bool Zdirection;
    private bool ZdirectionTemp;
    private bool Ydirection;
    private bool YdirectionTemp;

    public SlicerLine(int gets, int gives)
    {
        Init();
        Name = "Slicer Line";
        LeftPart = new WallItem();
        RightPart = new WallItem();
        ClassName = typeof(SlicerLine).FullName;
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
        ta1.SetName("Other Side");
        attrebutes.Add(ta1);

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y + 20, rect.width, rect.height);
        ToggleAttribute ta2 = new ToggleAttribute(at2Rect, this);
        ta2.mToggle = Xdirection;
        ta2.SetName("Use X axis");
        attrebutes.Add(ta2);

        Rect at3Rect = new Rect(position.x, rect.height / 2 + position.y + 40, rect.width, rect.height);
        ToggleAttribute ta3 = new ToggleAttribute(at3Rect, this);
        ta3.mToggle = Ydirection;
        ta3.SetName("Use Y axis");
        attrebutes.Add(ta3);

        Rect at4Rect = new Rect(position.x, rect.height / 2 + position.y + 60, rect.width, rect.height);
        ToggleAttribute ta4 = new ToggleAttribute(at4Rect, this);
        ta4.mToggle = Zdirection;
        ta4.SetName("Use Z axis");
        attrebutes.Add(ta4);

        Rect at5Rect = new Rect(position.x, rect.height / 2 + position.y + 80, rect.width, rect.height);

        FloatAttrebute fl1 = new FloatAttrebute(at5Rect, this);
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

        ToggleAttribute ta2 = (ToggleAttribute)attrebutes[1];
        ta2.mToggle = item.attributeValue[1] == "True";
        attrebutes[1] = ta2;
        XdirectionTemp = ta2.mToggle;

        ToggleAttribute ta3 = (ToggleAttribute)attrebutes[2];
        ta3.mToggle = item.attributeValue[2] == "True";
        attrebutes[2] = ta3;
        YdirectionTemp = ta3.mToggle;

        ToggleAttribute ta4 = (ToggleAttribute)attrebutes[3];
        ta4.mToggle = item.attributeValue[3] == "True";
        attrebutes[3] = ta4;
        ZdirectionTemp = ta4.mToggle;

        FloatAttrebute att = (FloatAttrebute)attrebutes[4];
        att.mFloat = float.Parse(item.attributeValue[4]);
        attrebutes[4] = att;
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

        ToggleAttribute ta2 = (ToggleAttribute)attrebutes[1];
        string stringtoggle2 = ta2.mToggle.ToString();
        item.attributeValue.Add(stringtoggle2);

        ToggleAttribute ta3 = (ToggleAttribute)attrebutes[2];
        string stringtoggle3 = ta3.mToggle.ToString();
        item.attributeValue.Add(stringtoggle3);

        ToggleAttribute ta4 = (ToggleAttribute)attrebutes[3];
        string stringtoggle4 = ta4.mToggle.ToString();
        item.attributeValue.Add(stringtoggle4);

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[4];
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

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        bool fromLeft = (bool)ta1.GetValue();

        ToggleAttribute ta2 = (ToggleAttribute)attrebutes[1];
        Xdirection = (bool)ta2.GetValue();
        if (Xdirection == true && Xdirection != XdirectionTemp)
        {
            Ydirection = false;
            ToggleAttribute ydir = (ToggleAttribute)attrebutes[2];
            ydir.mToggle = Ydirection;
            attrebutes[2] = ydir;

            Zdirection = false;
            ToggleAttribute zdir = (ToggleAttribute)attrebutes[3];
            zdir.mToggle = Zdirection;
            attrebutes[3] = zdir;
        }


        ToggleAttribute ta3 = (ToggleAttribute)attrebutes[2];
        Ydirection = (bool)ta3.GetValue();
        if (Ydirection == true && Ydirection != YdirectionTemp)
        {
            Xdirection = false;
            ToggleAttribute xdir = (ToggleAttribute)attrebutes[1];
            xdir.mToggle = Xdirection;
            attrebutes[1] = xdir;

            Zdirection = false;
            ToggleAttribute zdir = (ToggleAttribute)attrebutes[3];
            zdir.mToggle = Zdirection;
            attrebutes[3] = zdir;
        }

        ToggleAttribute ta4 = (ToggleAttribute)attrebutes[3];
        Zdirection = (bool)ta4.GetValue();
        if (Zdirection == true && Zdirection != ZdirectionTemp)
        {
            Xdirection = false;
            ToggleAttribute xdir = (ToggleAttribute)attrebutes[1];
            xdir.mToggle = Xdirection;
            attrebutes[1] = xdir;

            Ydirection = false;
            ToggleAttribute ydir = (ToggleAttribute)attrebutes[2];
            ydir.mToggle = Ydirection;
            attrebutes[2] = ydir;
        }

        XdirectionTemp = Xdirection;
        YdirectionTemp = Ydirection;
        ZdirectionTemp = Zdirection;

        

        WallItem wpi = (WallItem)mMesh;

        //WallPartItem wpi = new WallPartItem();

        if (GetNodes[0].ConnectedNode != null)
            wpi = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);

        LeftPart = new WallItem();
        RightPart =new WallItem();

        for (int l = 0; l < wpi.wallPartItems.Count; l++)
        {
            FloatAttrebute fa1 = (FloatAttrebute)attrebutes[4];
            distance = (float)fa1.GetValue();

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

            float cutline = 0;

            if (Zdirection)
            {
                if (fromLeft)
                {
                    float highestz = float.MinValue;
                    foreach (Vector3 vertex in vertices)
                    {
                        if (vertex.z > highestz)
                        {
                            highestz = vertex.z;
                        }
                    }

                    cutline = highestz - distance;
                }
                else
                {
                    float minz = float.MaxValue;
                    foreach (Vector3 vertex in vertices)
                    {
                        if (vertex.z < minz)
                        {
                            minz = vertex.z;
                        }
                    }

                    cutline = minz + distance;
                }
            }
            else if (Ydirection)
            {
                if (fromLeft)
                {
                    float highesty = float.MinValue;
                    foreach (Vector3 vertex in vertices)
                    {
                        if (vertex.y > highesty)
                        {
                            highesty = vertex.y;
                        }
                    }

                    cutline = highesty - distance;
                }
                else
                {
                    float miny = float.MaxValue;
                    foreach (Vector3 vertex in vertices)
                    {
                        if (vertex.y < miny)
                        {
                            miny = vertex.y;
                        }
                    }

                    cutline = miny + distance;
                }
            }
            else//Xdirection
            {
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

                    cutline = highestx - distance;
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

                    cutline = minx + distance;
                }
            }

            int temp4Index = 0;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                if (Zdirection)
                {
                    /*if (i % 6 > 0)//Second triangle of polygon
                    {
                        if (vertices[triangles[i]].z < cutline)
                        {
                            lowerVertices.Add(vertices[triangles[i]]);
                            //vertices[triangles[i]].z = cutZ;
                            Vector3 v = vertices[triangles[i]];
                            v.z = cutline;
                            upperVertices.Add(v);
                        }
                        else
                        {
                            upperVertices.Add(vertices[triangles[i]]);
                            //vertices[triangles[i]].z = cutZ;
                            Vector3 v = vertices[triangles[i]];
                            v.z = cutline;
                            lowerVertices.Add(v);
                        }

                        UpperUV.Add(uv[triangles[i]]);
                        UpperNormals.Add(normals[triangles[i]]);
                        upperTriangles.Add(temp4Index);
                        upperTriangles.Add(temp4Index - 1);
                        upperTriangles.Add(temp4Index - 2);
                        lowerUV.Add(uv[triangles[i]]);
                        lowerNormals.Add(normals[triangles[i]]);
                        lowerTriangles.Add(temp4Index);
                        lowerTriangles.Add(temp4Index - 1);
                        lowerTriangles.Add(temp4Index - 2);
                        temp4Index += 1;
                    }
                    else//first triangle of polygon
                    {*/
                        if (vertices[triangles[i]].z < cutline)
                        {
                            lowerVertices.Add(vertices[triangles[i]]);
                            //vertices[triangles[i]].z = cutZ;
                            Vector3 v = vertices[triangles[i]];
                            v.z = cutline;
                            upperVertices.Add(v);
                        }
                        else
                        {
                            upperVertices.Add(vertices[triangles[i]]);
                            //vertices[triangles[i]].z = cutZ;
                            Vector3 v = vertices[triangles[i]];
                            v.z = cutline;
                            lowerVertices.Add(v);

                        }

                        if (vertices[triangles[i + 1]].z < cutline)
                        {
                            lowerVertices.Add(vertices[triangles[i + 1]]);
                            //vertices[triangles[i+1]].z = cutZ;
                            Vector3 v = vertices[triangles[i + 1]];
                            v.z = cutline;
                            upperVertices.Add(v);

                        }
                        else
                        {
                            upperVertices.Add(vertices[triangles[i + 1]]);
                            //vertices[triangles[i + 1]].z = cutZ;
                            Vector3 v = vertices[triangles[i + 1]];
                            v.z = cutline;
                            lowerVertices.Add(v);
                        }

                        if (vertices[triangles[i + 2]].z < cutline)
                        {
                            lowerVertices.Add(vertices[triangles[i + 2]]);
                            //vertices[triangles[i + 2]].z = cutZ;
                            Vector3 v = vertices[triangles[i + 2]];
                            v.z = cutline;
                            upperVertices.Add(v);
                        }
                        else
                        {
                            upperVertices.Add(vertices[triangles[i + 2]]);
                            //vertices[triangles[i + 2]].z = cutZ;
                            Vector3 v = vertices[triangles[i + 2]];
                            v.z = cutline;
                            lowerVertices.Add(v);
                        }

                        UpperUV.Add(uv[triangles[i]]);
                        UpperUV.Add(uv[triangles[i + 1]]);
                        UpperUV.Add(uv[triangles[i + 2]]);

                        UpperNormals.Add(normals[triangles[i]]);
                        UpperNormals.Add(normals[triangles[i + 1]]);
                        UpperNormals.Add(normals[triangles[i + 2]]);

                        upperTriangles.Add(temp4Index);
                        upperTriangles.Add(temp4Index + 1);
                        upperTriangles.Add(temp4Index + 2);

                        lowerUV.Add(uv[triangles[i]]);
                        lowerUV.Add(uv[triangles[i + 1]]);
                        lowerUV.Add(uv[triangles[i + 2]]);

                        lowerNormals.Add(normals[triangles[i]]);
                        lowerNormals.Add(normals[triangles[i + 1]]);
                        lowerNormals.Add(normals[triangles[i + 2]]);

                        lowerTriangles.Add(temp4Index);
                        lowerTriangles.Add(temp4Index + 1);
                        lowerTriangles.Add(temp4Index + 2);

                        temp4Index += 3;
                   // }
                }
                else if (Ydirection)
                {
                    /*if (i % 6 > 0)//Second triangle of polygon
                    {
                        if (vertices[triangles[i]].y < cutline)
                        {
                            lowerVertices.Add(vertices[triangles[i]]);
                            //vertices[triangles[i]].z = cutZ;
                            Vector3 v = vertices[triangles[i]];
                            v.y = cutline;
                            upperVertices.Add(v);
                        }
                        else
                        {
                            upperVertices.Add(vertices[triangles[i]]);
                            //vertices[triangles[i]].z = cutZ;
                            Vector3 v = vertices[triangles[i]];
                            v.y = cutline;
                            lowerVertices.Add(v);
                        }

                        UpperUV.Add(uv[triangles[i]]);
                        UpperNormals.Add(normals[triangles[i]]);
                        upperTriangles.Add(temp4Index);
                        upperTriangles.Add(temp4Index - 1);
                        upperTriangles.Add(temp4Index - 2);
                        lowerUV.Add(uv[triangles[i]]);
                        lowerNormals.Add(normals[triangles[i]]);
                        lowerTriangles.Add(temp4Index);
                        lowerTriangles.Add(temp4Index - 1);
                        lowerTriangles.Add(temp4Index - 2);
                        temp4Index += 1;
                    }
                    else//first triangle of polygon
                    {*/
                        if (vertices[triangles[i]].y < cutline)
                        {
                            lowerVertices.Add(vertices[triangles[i]]);
                            //vertices[triangles[i]].z = cutZ;
                            Vector3 v = vertices[triangles[i]];
                            v.y = cutline;
                            upperVertices.Add(v);
                        }
                        else
                        {
                            upperVertices.Add(vertices[triangles[i]]);
                            //vertices[triangles[i]].z = cutZ;
                            Vector3 v = vertices[triangles[i]];
                            v.y = cutline;
                            lowerVertices.Add(v);

                        }

                        if (vertices[triangles[i + 1]].y < cutline)
                        {
                            lowerVertices.Add(vertices[triangles[i + 1]]);
                            //vertices[triangles[i+1]].z = cutZ;
                            Vector3 v = vertices[triangles[i + 1]];
                            v.y = cutline;
                            upperVertices.Add(v);

                        }
                        else
                        {
                            upperVertices.Add(vertices[triangles[i + 1]]);
                            //vertices[triangles[i + 1]].z = cutZ;
                            Vector3 v = vertices[triangles[i + 1]];
                            v.y = cutline;
                            lowerVertices.Add(v);
                        }

                        if (vertices[triangles[i + 2]].y < cutline)
                        {
                            lowerVertices.Add(vertices[triangles[i + 2]]);
                            //vertices[triangles[i + 2]].z = cutZ;
                            Vector3 v = vertices[triangles[i + 2]];
                            v.y = cutline;
                            upperVertices.Add(v);
                        }
                        else
                        {
                            upperVertices.Add(vertices[triangles[i + 2]]);
                            //vertices[triangles[i + 2]].z = cutZ;
                            Vector3 v = vertices[triangles[i + 2]];
                            v.y = cutline;
                            lowerVertices.Add(v);
                        }

                        UpperUV.Add(uv[triangles[i]]);
                        UpperUV.Add(uv[triangles[i + 1]]);
                        UpperUV.Add(uv[triangles[i + 2]]);

                        UpperNormals.Add(normals[triangles[i]]);
                        UpperNormals.Add(normals[triangles[i + 1]]);
                        UpperNormals.Add(normals[triangles[i + 2]]);

                        upperTriangles.Add(temp4Index);
                        upperTriangles.Add(temp4Index + 1);
                        upperTriangles.Add(temp4Index + 2);

                        lowerUV.Add(uv[triangles[i]]);
                        lowerUV.Add(uv[triangles[i + 1]]);
                        lowerUV.Add(uv[triangles[i + 2]]);

                        lowerNormals.Add(normals[triangles[i]]);
                        lowerNormals.Add(normals[triangles[i + 1]]);
                        lowerNormals.Add(normals[triangles[i + 2]]);

                        lowerTriangles.Add(temp4Index);
                        lowerTriangles.Add(temp4Index + 1);
                        lowerTriangles.Add(temp4Index + 2);

                        temp4Index += 3;
                    //}
                }
                else
                {
                    if (is6VertexPolygon)
                    {
                        if (vertices[triangles[i]].x > cutline && vertices[triangles[i + 1]].x > cutline && vertices[triangles[i + 2]].x > cutline)
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
                        else if (vertices[triangles[i]].x < cutline && vertices[triangles[i + 1]].x < cutline && vertices[triangles[i + 2]].x < cutline)
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
                            float tempXpos = getCutXPos(vertices[triangles[i]], vertices[triangles[i + 1]], fromLeft);

                            float distZ = vertices[triangles[i]].z - vertices[triangles[i + 1]].z;
                            //Debug.Log("Dist Z = "+distZ+" -- i = "+i+ "-- vertices[i]= " + vertices[triangles[i]] + " -- vertices[i+1]= " + vertices[triangles[i+1]] + " -- vertices[i+2]= " + vertices[triangles[i + 2]]);
                            //Debug.Log("cutX = "+cutx+"-- tempcutX= "+ tempXpos);
                            float distX = Mathf.Abs(vertices[triangles[i]].x - vertices[triangles[i + 1]].x);
                            float xdistI = Mathf.Abs(vertices[triangles[i]].x - (tempXpos));// + MathF.Abs(cutx - tempXpos)));
                            float xdistII = Mathf.Abs(vertices[triangles[i + 1]].x - cutline);
                            float xdistIII = 0;// = Mathf.Abs(vertices[triangles[i+2]].x - cutx);
                            float Zpos = vertices[triangles[i]].z - (distZ * Mathf.Abs(xdistI / distX));
                            float Zpos2 = 0;
                            float distX2 = 0;
                            float tempXpos2 = 0;
                            if (i % 6 > 0)//Second triangle of polygon
                            {
                                tempXpos2 = getCutXPos(vertices[triangles[i + 2]], vertices[triangles[i - 3]], fromLeft);
                                xdistIII = Mathf.Abs(vertices[triangles[i + 2]].x - tempXpos2);
                                float distZ2 = vertices[triangles[i + 2]].z - vertices[triangles[i - 3]].z;
                                distX2 = Mathf.Abs(vertices[triangles[i + 2]].x - vertices[triangles[i - 3]].x);
                                Zpos2 = vertices[triangles[i + 2]].z - (distZ2 * Mathf.Abs(xdistIII / distX2));
                            }
                            else//First triangle of polygon
                            {
                                tempXpos2 = getCutXPos(vertices[triangles[i + 2]], vertices[triangles[i + 3]], fromLeft);
                                xdistIII = Mathf.Abs(vertices[triangles[i + 2]].x - tempXpos2);
                                float distZ2 = vertices[triangles[i + 2]].z - vertices[triangles[i + 3]].z;
                                distX2 = Mathf.Abs(vertices[triangles[i + 2]].x - vertices[triangles[i + 3]].x);
                                Zpos2 = vertices[triangles[i + 2]].z - (distZ2 * Mathf.Abs(xdistIII / distX2));
                            }

                            if (vertices[triangles[i]].x < tempXpos)
                            {
                                lowerVertices.Add(vertices[triangles[i]]);
                                //vertices[triangles[i]].x = cutx;
                                Vector3 v = vertices[triangles[i]];
                                v.x = tempXpos;
                                v.z = Zpos;
                                upperVertices.Add(v);
                            }
                            else
                            {
                                upperVertices.Add(vertices[triangles[i]]);
                                //vertices[triangles[i]].x = cutx;
                                Vector3 v = vertices[triangles[i]];
                                v.x = tempXpos;
                                v.z = Zpos;
                                lowerVertices.Add(v);
                            }

                            if (vertices[triangles[i + 1]].x < tempXpos)
                            {
                                lowerVertices.Add(vertices[triangles[i + 1]]);
                                //vertices[triangles[i + 1]].x = cutx;
                                Vector3 v = vertices[triangles[i + 1]];
                                v.x = tempXpos;
                                v.z = Zpos;
                                upperVertices.Add(v);

                            }
                            else
                            {
                                upperVertices.Add(vertices[triangles[i + 1]]);
                                //vertices[triangles[i + 1]].x = cutx;
                                Vector3 v = vertices[triangles[i + 1]];
                                v.x = tempXpos;
                                v.z = Zpos;
                                lowerVertices.Add(v);
                            }

                            if (vertices[triangles[i + 2]].x < cutline)
                            {
                                lowerVertices.Add(vertices[triangles[i + 2]]);
                                //vertices[triangles[i + 2]].x = cutx;
                                Vector3 v = vertices[triangles[i + 2]];
                                v.x = tempXpos2;
                                v.z = Zpos2;
                                upperVertices.Add(v);
                            }
                            else
                            {
                                upperVertices.Add(vertices[triangles[i + 2]]);
                                //vertices[triangles[i + 2]].x = cutx;
                                Vector3 v = vertices[triangles[i + 2]];
                                v.x = tempXpos2;
                                v.z = Zpos2;
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
                    else
                    {
                        /*if (i % 6 > 0)//Second triangle of polygon
                        {
                            if (vertices[triangles[i]].x < cutline)
                            {
                                lowerVertices.Add(vertices[triangles[i]]);
                                //vertices[triangles[i]].z = cutZ;
                                Vector3 v = vertices[triangles[i]];
                                v.x = cutline;
                                upperVertices.Add(v);
                            }
                            else
                            {
                                upperVertices.Add(vertices[triangles[i]]);
                                //vertices[triangles[i]].z = cutZ;
                                Vector3 v = vertices[triangles[i]];
                                v.x = cutline;
                                lowerVertices.Add(v);
                            }

                            UpperUV.Add(uv[triangles[i]]);
                            UpperNormals.Add(normals[triangles[i]]);
                            upperTriangles.Add(temp4Index);
                            upperTriangles.Add(temp4Index - 1);
                            upperTriangles.Add(temp4Index - 2);
                            lowerUV.Add(uv[triangles[i]]);
                            lowerNormals.Add(normals[triangles[i]]);
                            lowerTriangles.Add(temp4Index);
                            lowerTriangles.Add(temp4Index - 1);
                            lowerTriangles.Add(temp4Index - 2);
                            temp4Index += 1;
                        }
                        else//first triangle of polygon
                        {*/
                            if (vertices[triangles[i]].x < cutline)
                            {
                                lowerVertices.Add(vertices[triangles[i]]);
                                //vertices[triangles[i]].z = cutZ;
                                Vector3 v = vertices[triangles[i]];
                                v.x = cutline;
                                upperVertices.Add(v);
                            }
                            else
                            {
                                upperVertices.Add(vertices[triangles[i]]);
                                //vertices[triangles[i]].z = cutZ;
                                Vector3 v = vertices[triangles[i]];
                                v.x = cutline;
                                lowerVertices.Add(v);

                            }

                            if (vertices[triangles[i + 1]].x < cutline)
                            {
                                lowerVertices.Add(vertices[triangles[i + 1]]);
                                //vertices[triangles[i+1]].z = cutZ;
                                Vector3 v = vertices[triangles[i + 1]];
                                v.x = cutline;
                                upperVertices.Add(v);

                            }
                            else
                            {
                                upperVertices.Add(vertices[triangles[i + 1]]);
                                //vertices[triangles[i + 1]].z = cutZ;
                                Vector3 v = vertices[triangles[i + 1]];
                                v.x = cutline;
                                lowerVertices.Add(v);
                            }

                            if (vertices[triangles[i + 2]].x < cutline)
                            {
                                lowerVertices.Add(vertices[triangles[i + 2]]);
                                //vertices[triangles[i + 2]].z = cutZ;
                                Vector3 v = vertices[triangles[i + 2]];
                                v.x = cutline;
                                upperVertices.Add(v);
                            }
                            else
                            {
                                upperVertices.Add(vertices[triangles[i + 2]]);
                                //vertices[triangles[i + 2]].z = cutZ;
                                Vector3 v = vertices[triangles[i + 2]];
                                v.x = cutline;
                                lowerVertices.Add(v);
                            }

                            UpperUV.Add(uv[triangles[i]]);
                            UpperUV.Add(uv[triangles[i + 1]]);
                            UpperUV.Add(uv[triangles[i + 2]]);

                            UpperNormals.Add(normals[triangles[i]]);
                            UpperNormals.Add(normals[triangles[i + 1]]);
                            UpperNormals.Add(normals[triangles[i + 2]]);

                            upperTriangles.Add(temp4Index);
                            upperTriangles.Add(temp4Index + 1);
                            upperTriangles.Add(temp4Index + 2);

                            lowerUV.Add(uv[triangles[i]]);
                            lowerUV.Add(uv[triangles[i + 1]]);
                            lowerUV.Add(uv[triangles[i + 2]]);

                            lowerNormals.Add(normals[triangles[i]]);
                            lowerNormals.Add(normals[triangles[i + 1]]);
                            lowerNormals.Add(normals[triangles[i + 2]]);

                            lowerTriangles.Add(temp4Index);
                            lowerTriangles.Add(temp4Index + 1);
                            lowerTriangles.Add(temp4Index + 2);

                            temp4Index += 3;
                       // }
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
