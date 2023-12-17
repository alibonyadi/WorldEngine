using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WallDesigner;

public class Split : FunctionItem, IFunctionItem
{
    private float distance = 0f;
    private bool autoAdjust=false;
    private bool useX = false;
    private bool useZ = false;
    private bool is6VertexPolygon = false;
    public Split(int gets, int gives) 
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

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y+20, rect.width, rect.height);
        ToggleAttribute ta2 = new ToggleAttribute(at2Rect);
        ta2.mToggle = autoAdjust;
        ta2.SetName("Use X axis");
        attrebutes.Add(ta2);

        Rect at3Rect = new Rect(position.x, rect.height / 2 + position.y+40, rect.width, rect.height);
        ToggleAttribute ta3 = new ToggleAttribute(at3Rect);
        ta3.mToggle = autoAdjust;
        ta3.SetName("Use Z axis");
        attrebutes.Add(ta3);

        Rect at4Rect = new Rect(position.x, rect.height / 2 + position.y + 60, rect.width, rect.height);
        FloatAttrebute fl1 = new FloatAttrebute(at4Rect);
        fl1.mFloat = distance;
        fl1.SetMinMax(0.01f, 10);
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

        if (item.attributeValue.Count > 0)
        {
            ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
            ta1.mToggle = item.attributeValue[0] == "True";
            attrebutes[0] = ta1;
        }

        if (item.attributeValue.Count > 1)
        {
            ToggleAttribute ta2 = (ToggleAttribute)attrebutes[1];
            ta2.mToggle = item.attributeValue[1] == "True";
            attrebutes[1] = ta2;
        }

        if (item.attributeValue.Count > 2)
        {
            ToggleAttribute ta3 = (ToggleAttribute)attrebutes[2];
            ta3.mToggle = item.attributeValue[2] == "True";
            attrebutes[2] = ta3;
        }

        if (item.attributeValue.Count > 3)
        {
            FloatAttrebute att = (FloatAttrebute)attrebutes[3];
            att.mFloat = float.Parse(item.attributeValue[3]);
            attrebutes[3] = att;
        }
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

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[3];
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
        FloatAttrebute fa1 = (FloatAttrebute)attrebutes[3];
        distance = (float)fa1.GetValue();

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        autoAdjust = (bool)ta1.GetValue();

        ToggleAttribute ta2 = (ToggleAttribute)attrebutes[1];
        if(useX != (bool)ta2.GetValue())
        {
            Debug.Log("useX");
            useX = (bool)ta2.GetValue();
            if (useX)
            {
                useZ = false;
                ToggleAttribute taaa = (ToggleAttribute)attrebutes[2];
                taaa.mToggle = useZ;
                attrebutes[2] = taaa;
            }
        }

        ToggleAttribute ta3 = (ToggleAttribute)attrebutes[2];
        if (useZ != (bool)ta3.GetValue())
        {
            Debug.Log("useZ");
            useZ = (bool)ta3.GetValue();
            if (useZ)
            {
                useX = false;
                ToggleAttribute taaaa = (ToggleAttribute)attrebutes[1];
                taaaa.mToggle = useX;
                attrebutes[1] = taaaa;
            }
        }

        WallItem wpi = (WallItem)mMesh;

        if (GetNodes[0].ConnectedNode != null)
            wpi = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);

        for (int i = 0; i < wpi.wallPartItems.Count; i++)
        {
            if (i > 0)
            {
                wpi.wallPartItems[i] = CombineItems.CombineTwoItem(wpi.wallPartItems[i - 1].mesh, wpi.wallPartItems[i].mesh, wpi.wallPartItems[i - 1].material, wpi.wallPartItems[i].material);
            }
        }

        WallPartItem wallPartItem = wpi.wallPartItems[wpi.wallPartItems.Count - 1];

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
        //Debug.Log(numberOfSlices);
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
            int temp4Index = 0;
            int temp4IndexLowwer = 0;
            for (int i = 0; i < triangles.Length; i += 3)
            {
                if (is6VertexPolygon)
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
                else//4 vertex polygon
                {
                    if (i % 6 > 0)//Second triangle of polygon
                    {
                        /*if (vertices[triangles[temp4IndexUpper]].y > cutY && vertices[triangles[temp4IndexUpper + 1]].y > cutY && vertices[triangles[temp4IndexUpper + 2]].y > cutY)
                        {
                            upperVertices.Add(vertices[triangles[temp4Index]]);
                            UpperUV.Add(uv[triangles[temp4Index]]);
                            UpperNormals.Add(normals[triangles[temp4Index]]);

                            upperTriangles.Add(temp4Index);
                            upperTriangles.Add(temp4Index-1);
                            upperTriangles.Add(temp4Index-2);
                            temp4IndexUpper++;
                            Debug.Log("Full Up");

                        }
                        else if (vertices[triangles[temp4Index]].y < cutY && vertices[triangles[temp4Index + 1]].y < cutY && vertices[triangles[temp4Index + 2]].y < cutY)
                        {
                            lowerVertices.Add(vertices[triangles[temp4Index]]);
                            lowerUV.Add(uv[triangles[temp4Index]]);
                            lowerNormals.Add(normals[triangles[temp4Index]]);

                            Debug.Log("Full Low");
                            lowerTriangles.Add(temp4Index);
                            lowerTriangles.Add(temp4Index-1);
                            lowerTriangles.Add(temp4Index-2);
                            temp4IndexLowwer++;
                        }
                        else
                        {*/

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

                            UpperUV.Add(uv[triangles[i]]);
                            UpperNormals.Add(normals[triangles[i]]);
                            upperTriangles.Add(temp4Index);
                            upperTriangles.Add(temp4Index-1);
                            upperTriangles.Add(temp4Index-2);
                            lowerUV.Add(uv[triangles[i]]);
                            lowerNormals.Add(normals[triangles[i]]);
                            lowerTriangles.Add(temp4Index);
                            lowerTriangles.Add(temp4Index-1);
                            lowerTriangles.Add(temp4Index-2);
                            temp4Index += 1;
                        //}
                    }
                    else
                    {
                        /*if (vertices[triangles[temp4Index]].y > cutY && vertices[triangles[temp4Index + 1]].y > cutY && vertices[triangles[temp4Index + 2]].y > cutY)
                        {

                            upperVertices.Add(vertices[triangles[temp4Index]]);
                            upperVertices.Add(vertices[triangles[temp4Index + 1]]);
                            upperVertices.Add(vertices[triangles[temp4Index + 2]]);

                            UpperUV.Add(uv[triangles[temp4Index]]);
                            UpperUV.Add(uv[triangles[temp4Index + 1]]);
                            UpperUV.Add(uv[triangles[temp4Index + 2]]);

                            UpperNormals.Add(normals[triangles[temp4Index]]);
                            UpperNormals.Add(normals[triangles[temp4Index + 1]]);
                            UpperNormals.Add(normals[triangles[temp4Index + 2]]);

                            upperTriangles.Add(temp4Index);
                            upperTriangles.Add(temp4Index+1);
                            upperTriangles.Add(temp4Index+2);
                            temp4IndexUpper += 3;
                            Debug.Log("Full Up");
                        }
                        else if (vertices[triangles[temp4Index]].y < cutY && vertices[triangles[temp4Index + 1]].y < cutY && vertices[triangles[temp4Index + 2]].y < cutY)
                        {
                            lowerVertices.Add(vertices[triangles[temp4Index]]);
                            lowerVertices.Add(vertices[triangles[temp4Index + 1]]);
                            lowerVertices.Add(vertices[triangles[temp4Index + 2]]);

                            lowerUV.Add(uv[triangles[temp4Index]]);
                            lowerUV.Add(uv[triangles[temp4Index + 1]]);
                            lowerUV.Add(uv[triangles[temp4Index + 2]]);

                            lowerNormals.Add(normals[triangles[temp4Index]]);
                            lowerNormals.Add(normals[triangles[temp4Index + 1]]);
                            lowerNormals.Add(normals[triangles[temp4Index + 2]]);

                            lowerTriangles.Add(temp4Index);
                            lowerTriangles.Add(temp4Index+1);
                            lowerTriangles.Add(temp4Index+2);
                            Debug.Log("Full Low");
                            temp4IndexLowwer += 3;
                        }
                        else
                        {*/
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

                            upperTriangles.Add(temp4Index);
                            upperTriangles.Add(temp4Index+1);
                            upperTriangles.Add(temp4Index+2);

                            lowerUV.Add(uv[triangles[i]]);
                            lowerUV.Add(uv[triangles[i + 1]]);
                            lowerUV.Add(uv[triangles[i + 2]]);

                            lowerNormals.Add(normals[triangles[i]]);
                            lowerNormals.Add(normals[triangles[i + 1]]);
                            lowerNormals.Add(normals[triangles[i + 2]]);

                            lowerTriangles.Add(temp4Index);
                            lowerTriangles.Add(temp4Index+1);
                            lowerTriangles.Add(temp4Index+2);

                            temp4Index += 3;
                        //}

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
        WallItem output = new WallItem();
        output.wallPartItems = SlicedItems;
        output.buildingDirection =wpi.buildingDirection;
        return output;
    }
}
