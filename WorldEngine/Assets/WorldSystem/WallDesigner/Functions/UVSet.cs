using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class UVSet : FunctionItem, IFunctionItem
{
    float UVX = 1;
    float UVY = 1;
    bool world = false;
    bool Square = false;
    public UVSet(int gets,int gives) 
    {
        Init();
        Name = "Set UV";
        ClassName = typeof(UVSet).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);

        GiveNode givenode1 = new GiveNode();//forward-street
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);

        CalculateRect();
        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);

        ToggleAttribute ta1 = new ToggleAttribute(at1Rect);
        ta1.mToggle = world;
        ta1.SetName("World Scale");
        attrebutes.Add(ta1);

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y + 20, rect.width, rect.height);

        FloatAttrebute fl1 = new FloatAttrebute(at2Rect);
        fl1.mFloat = UVX;
        fl1.SetMinMax(0.01f, 5);
        fl1.SetName("UVX");
        attrebutes.Add(fl1);

        Rect at3Rect = new Rect(position.x, rect.height / 2 + position.y + 40, rect.width, rect.height);

        FloatAttrebute fl2 = new FloatAttrebute(at3Rect);
        fl2.mFloat = UVY;
        fl2.SetMinMax(0.01f, 5);
        fl2.SetName("UVY");
        attrebutes.Add(fl2);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.getnodeConnectedFI.Count > 0)
            GetNodes[0].ConnectedNode = functionItems[item.getnodeConnectedFI[0]].GiveNodes[item.getnodeItems[0]];

        if (item.givenodeConnectedFI.Count > 0)
            GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GetNodes[item.givenodeItems[0]];

        //if (item.givenodeConnectedFI.Count > 1)
        //    GiveNodes[1].ConnectedNode = functionItems[item.givenodeConnectedFI[1]].GetNodes[item.givenodeItems[1]];
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

        FloatAttrebute att2 = (FloatAttrebute)attrebutes[2];
        att2.mFloat = float.Parse(item.attributeValue[2]);
        attrebutes[2] = att2;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("UVSet attrebutes");

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        string stringtoggle = ta1.mToggle.ToString();
        item.attributeValue.Add(stringtoggle);

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[1];
        string stringFloat = att1.mFloat.ToString();
        item.attributeValue.Add(stringFloat);

        FloatAttrebute att2 = (FloatAttrebute)attrebutes[2];
        string stringFloat2 = att2.mFloat.ToString();
        item.attributeValue.Add(stringFloat2);

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
        WallItem WI = mMesh as WallItem;
        if (GetNodes[0].ConnectedNode != null)
            WI = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(WI, GetNodes[0].ConnectedNode.id);

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        world = (bool)ta1.GetValue();

        FloatAttrebute fl1 = (FloatAttrebute)attrebutes[1];
        UVX = (float)fl1.GetValue();

        FloatAttrebute fl2 = (FloatAttrebute)attrebutes[2];
        UVY = (float)fl2.GetValue();

        for (int j = 0; j < WI.wallPartItems.Count; j++)
        {
            if (WI.wallPartItems[j] == null)
                continue;

            Mesh originalMesh = WI.wallPartItems[j].mesh;
            Vector3[] vertices = originalMesh.vertices;
            Vector3[] normals = originalMesh.normals;
            int[] triangles = originalMesh.triangles;
            Vector2[] UVs = originalMesh.uv;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                //Debug.Log(i);
                if (world)
                {
                    /*float distX = Vector3.Magnitude(vertices[triangles[i]] - vertices[triangles[i + 1]]);
                    float distY = Vector3.Magnitude(vertices[triangles[i]] - vertices[triangles[i + 2]]);
                    float distX2 = 0;
                    float distY2 = 0;
                    if (i % 6 > 0)//Second triangle of polygon
                    {
                        distX2 = Vector3.Magnitude(vertices[triangles[i + 2]] - vertices[triangles[i - 3]]);
                        distY2 = Vector3.Magnitude(vertices[triangles[i + 1]] - vertices[triangles[i - 3]]);
                    }
                    else
                    {
                        distX2 = Vector3.Magnitude(vertices[triangles[i + 2]] - vertices[triangles[i + 3]]);
                        distY2 = Vector3.Magnitude(vertices[triangles[i + 1]] - vertices[triangles[i + 3]]);
                    }
                    UVs[triangles[i]] = new Vector2((UVs[triangles[i]].x / UVX)* distX, (UVs[triangles[i]].y / UVY)* distY);
                    UVs[triangles[i + 1]] = new Vector2((UVs[triangles[i + 1]].x / UVX)* distX, ( UVs[triangles[i + 1]].y / UVY)* distY2);
                    UVs[triangles[i + 2]] = new Vector2((UVs[triangles[i + 2]].x / UVX)* distX2, (UVs[triangles[i + 2]].y / UVY)* distY);*/

                    float distX = Vector3.Magnitude(vertices[triangles[i]] - vertices[triangles[i + 1]]);
                    float distY = Vector3.Magnitude(vertices[triangles[i]] - vertices[triangles[i + 2]]);
                    float distX2 = 0;
                    float distY2 = 0;
                    /*if (i % 6 > 0)//Second triangle of polygon
                    {
                        distX2 = Vector3.Magnitude(vertices[triangles[i + 2]] - vertices[triangles[i - 3]]);
                        distY2 = Vector3.Magnitude(vertices[triangles[i + 1]] - vertices[triangles[i - 3]]);
                    }
                    else
                    {
                        distX2 = Vector3.Magnitude(vertices[triangles[i + 2]] - vertices[triangles[i + 3]]);
                        distY2 = Vector3.Magnitude(vertices[triangles[i + 1]] - vertices[triangles[i + 3]]);
                    }*/

                    distX2 = Vector3.Magnitude(vertices[triangles[i + 2]] - vertices[triangles[i + 1]]);
                    distY2 = Vector3.Magnitude(vertices[triangles[i + 1]] - vertices[triangles[i + 2]]);

                    if (i % 6 > 0)//Second triangle of polygon
                    {
                        UVs[triangles[i]] = new Vector2((UVs[triangles[i]].x / UVX) * distX, (UVs[triangles[i]].y / UVY) * distY);
                    }
                    else
                    {

                        UVs[triangles[i]] = new Vector2((UVs[triangles[i]].x / UVX) * distX, (UVs[triangles[i]].y / UVY) * distY);
                        UVs[triangles[i + 1]] = new Vector2((UVs[triangles[i + 1]].x / UVX) * distX, (UVs[triangles[i + 1]].y / UVY) * distY2);
                        UVs[triangles[i + 2]] = new Vector2((UVs[triangles[i + 2]].x / UVX) * distX2, (UVs[triangles[i + 2]].y / UVY) * distY);
                    }
                }
                else
                {
                    if (i % 6 > 0)//Second triangle of polygon
                    {
                        //Debug.Log(UVs[triangles[i]] + " --- ");
                        UVs[triangles[i]] = new Vector2(UVs[triangles[i]].x / UVX, UVs[triangles[i]].y / UVY);
                    }
                    else
                    {
                        //Debug.Log(UVs[triangles[i]] + " --- " + UVs[triangles[i + 1]] + " --- " + UVs[triangles[i + 2]]);
                        UVs[triangles[i    ]] = new Vector2(UVs[triangles[i    ]].x / UVX, UVs[triangles[i]].y / UVY);
                        UVs[triangles[i + 1]] = new Vector2(UVs[triangles[i + 1]].x / UVX, UVs[triangles[i + 1]].y / UVY);
                        UVs[triangles[i + 2]] = new Vector2(UVs[triangles[i + 2]].x / UVX, UVs[triangles[i + 2]].y / UVY);
                        
                    }
                }
            }
            //UVs[triangles[i]] = new Vector2( UVs[triangles[i]].x / UVX , UVs[triangles[i]].y/ UVY);

            WI.wallPartItems[j].mesh.uv = UVs;

        }









        return WI;
    }
}
