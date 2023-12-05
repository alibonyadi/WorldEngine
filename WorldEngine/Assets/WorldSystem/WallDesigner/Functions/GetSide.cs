using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using WallDesigner;


public class GetSide : FunctionItem, IFunctionItem
{
    public GetSide()
    {
        Init();
        Name = "Get Mesh Sides";
        ClassName = typeof(GetSide).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);

        GiveNode givenode1 = new GiveNode();//forward-street
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);

        GiveNode givenode2 = new GiveNode();//side
        givenode2.AttachedFunctionItem = this;
        givenode2.id = 1;
        GiveNodes.Add(givenode2);

        GiveNode givenode3 = new GiveNode();//the alley
        givenode3.AttachedFunctionItem = this;
        givenode3.id = 1;
        GiveNodes.Add(givenode3);

        CalculateRect();
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.getnodeConnectedFI.Count > 0)
            GetNodes[0].ConnectedNode = functionItems[item.getnodeConnectedFI[0]].GiveNodes[item.getnodeItems[0]];

        if (item.givenodeConnectedFI.Count > 0)
            GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GetNodes[item.givenodeItems[0]];

        if (item.givenodeConnectedFI.Count > 1)
            GiveNodes[1].ConnectedNode = functionItems[item.givenodeConnectedFI[1]].GetNodes[item.givenodeItems[1]];

        if (item.givenodeConnectedFI.Count > 1)
            GiveNodes[2].ConnectedNode = functionItems[item.givenodeConnectedFI[2]].GetNodes[item.givenodeItems[2]];

    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        /*ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        ta1.mToggle = item.attributeValue[0] == "True";
        attrebutes[0] = ta1;

        FloatAttrebute att = (FloatAttrebute)attrebutes[1];
        att.mFloat = float.Parse(item.attributeValue[1]);
        attrebutes[1] = att;*/
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        /*item.attributeName.Add("FloatAttrebute");

        ToggleAttribute ta1 = (ToggleAttribute)attrebutes[0];
        string stringtoggle = ta1.mToggle.ToString();
        item.attributeValue.Add(stringtoggle);

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[1];
        string stringtexturePath = att1.mFloat.ToString();
        item.attributeValue.Add(stringtexturePath);*/

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

        if (GiveNodes[2].ConnectedNode != null)
        {
            int connectedGiveNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GiveNodes[2].ConnectedNode.AttachedFunctionItem);
            item.givenodeConnectedFI.Add(connectedGiveNodeNumber);
            item.givenodeItems.Add(GiveNodes[2].ConnectedNode.id);
        }
        return item;
    }

    public object Execute(object mMesh, object id)
    {
        WallItem wallItem = new WallItem();//(List<WallPartItem>)mMesh;

        float angleTreshold = 20f;

        if (GetNodes[0].ConnectedNode != null)
            wallItem = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wallItem, GetNodes[0].ConnectedNode.id);

        List<Vector3> forwardDirection = wallItem.buildingDirection.CheckStreet() ? wallItem.buildingDirection.streetDirections : new List<Vector3>() { new Vector3(0, 0, 1) };
        List<Vector3> alleyDirection = wallItem.buildingDirection.CheckAlley() ? wallItem.buildingDirection.alleyDirections : null;

        List<WallPartItem> forwardSide = new List<WallPartItem>();
        List<WallPartItem> side = new List<WallPartItem>();
        List<WallPartItem> alleySide = new List<WallPartItem>();

        List<Vector3> forwardsideVertices = new List<Vector3>();
        List<int> forwardsideTriangles = new List<int>();
        List<Vector3> forwardsideNormals = new List<Vector3>();
        List<Vector2> forwardsideUVs = new List<Vector2>();

        List<Vector3> alleysideVertices = new List<Vector3>();
        List<int> alleysideTriangles = new List<int>();
        List<Vector3> alleysideNormals = new List<Vector3>();
        List<Vector2> alleysideUVs = new List<Vector2>();

        List<Vector3> sideVertices = new List<Vector3>();
        List<int> sideTriangles = new List<int>();
        List<Vector3> sideNormals = new List<Vector3>();
        List<Vector2> sideUVs = new List<Vector2>();

        for (int j=0;j< wallItem.wallPartItems.Count;j++)
        {
            int[] tri = wallItem.wallPartItems[j].mesh.triangles;
            Vector3[] vertices = wallItem.wallPartItems[j].mesh.vertices;
            Vector3[] normals = wallItem.wallPartItems[j].mesh.normals;
            Vector2[] uvs = wallItem.wallPartItems[j].mesh.uv;
            
            for(int i=0;i< tri.Length;i+=3)
            {
                bool isForward = false;
                bool isAlley = false;
                Vector3 normal = GetTriangleNormal(vertices[tri[i]], vertices[tri[i + 1]], vertices[tri[i + 2]]);
                for(int k=0;k< forwardDirection.Count;k++)
                {
                    float angle = Vector3.Angle(normal, forwardDirection[k]);
                    if(angle < angleTreshold)
                    {
                        isForward = true;

                        forwardsideVertices.Add(vertices[tri[i]]);
                        forwardsideVertices.Add(vertices[tri[i + 1]]);
                        forwardsideVertices.Add(vertices[tri[i + 2]]);

                        forwardsideTriangles.Add(forwardsideTriangles.Count);
                        forwardsideTriangles.Add(forwardsideTriangles.Count);
                        forwardsideTriangles.Add(forwardsideTriangles.Count);

                        forwardsideNormals.Add(normals[tri[i]]);
                        forwardsideNormals.Add(normals[tri[i + 1]]);
                        forwardsideNormals.Add(normals[tri[i + 2]]);

                        forwardsideUVs.Add(uvs[tri[i]]);
                        forwardsideUVs.Add(uvs[tri[i + 1]]);
                        forwardsideUVs.Add(uvs[tri[i + 2]]);
                    }
                }

                if (isForward)
                    continue;


                if(alleyDirection != null)
                for (int k = 0; k < alleyDirection.Count; k++)
                {
                    float angle = Vector3.Angle(normal, alleyDirection[k]);
                    if (angle < angleTreshold)
                    {
                        isAlley = true;

                        alleysideVertices.Add(vertices[tri[i]]);
                        alleysideVertices.Add(vertices[tri[i + 1]]);
                        alleysideVertices.Add(vertices[tri[i + 2]]);

                        alleysideTriangles.Add(alleysideTriangles.Count);
                        alleysideTriangles.Add(alleysideTriangles.Count);
                        alleysideTriangles.Add(alleysideTriangles.Count);

                        alleysideNormals.Add(normals[tri[i]]);
                        alleysideNormals.Add(normals[tri[i + 1]]);
                        alleysideNormals.Add(normals[tri[i + 2]]);

                        alleysideUVs.Add(uvs[tri[i]]);
                        alleysideUVs.Add(uvs[tri[i + 1]]);
                        alleysideUVs.Add(uvs[tri[i + 2]]);
                    }
                }

                if (isAlley)
                    continue;

                for (int k = 0; k < alleyDirection.Count; k++)
                {
                    float angle = Vector3.Angle(normal, alleyDirection[k]);
                    if (angle < angleTreshold)
                    {
                        isAlley = true;

                        sideVertices.Add(vertices[tri[i]]);
                        sideVertices.Add(vertices[tri[i + 1]]);
                        sideVertices.Add(vertices[tri[i + 2]]);

                        sideTriangles.Add(sideTriangles.Count);
                        sideTriangles.Add(sideTriangles.Count);
                        sideTriangles.Add(sideTriangles.Count);

                        sideNormals.Add(normals[tri[i]]);
                        sideNormals.Add(normals[tri[i + 1]]);
                        sideNormals.Add(normals[tri[i + 2]]);

                        sideUVs.Add(uvs[tri[i]]);
                        sideUVs.Add(uvs[tri[i + 1]]);
                        sideUVs.Add(uvs[tri[i + 2]]);
                    }
                }
            }
            
            if((int)id==0)
            {
                Mesh forwardMesh = new Mesh();
                forwardMesh.vertices = forwardsideVertices.ToArray();
                forwardMesh.triangles = forwardsideTriangles.ToArray();
                forwardMesh.normals = forwardsideNormals.ToArray();
                forwardMesh.uv = forwardsideUVs.ToArray();
                forwardMesh.RecalculateBounds();

                WallPartItem wallPartItem = new WallPartItem();
                wallPartItem.mesh = forwardMesh;
                wallPartItem.material = wallItem.wallPartItems[j].material;

                forwardSide.Add(wallPartItem);

            }
            else if((int)id == 1)
            {
                Mesh sideMesh = new Mesh();
                sideMesh.vertices = sideVertices.ToArray();
                sideMesh.triangles = sideTriangles.ToArray();
                sideMesh.normals = sideNormals.ToArray();
                sideMesh.uv = sideUVs.ToArray();
                sideMesh.RecalculateBounds();

                WallPartItem wallPartItem = new WallPartItem();
                wallPartItem.mesh = sideMesh;
                wallPartItem.material = wallItem.wallPartItems[j].material;

                side.Add(wallPartItem);
            }
            else if((int)id == 2)
            {
                Mesh alleyMesh = new Mesh();
                alleyMesh.vertices = alleysideVertices.ToArray();
                alleyMesh.triangles = alleysideTriangles.ToArray();
                alleyMesh.normals = alleysideNormals.ToArray();
                alleyMesh.uv = alleysideUVs.ToArray();
                alleyMesh.RecalculateBounds();

                WallPartItem wallPartItem = new WallPartItem();
                wallPartItem.mesh = alleyMesh;
                wallPartItem.material = wallItem.wallPartItems[j].material;

                side.Add(wallPartItem);
            }
             

        }

        WallItem output = new WallItem();
        output.buildingDirection = wallItem.buildingDirection;
        if ((int)id == 0)
            output.wallPartItems = forwardSide;
        else if ((int)id == 1)
            output.wallPartItems = side;
        else if((int)id == 2)
            output.wallPartItems = alleySide;

        return output;
    }

    public static Vector3 GetTriangleNormal(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        Vector3 vertexA = vertex1;
        Vector3 vertexB = vertex2;
        Vector3 vertexC = vertex3;

        Vector3 side1 = vertexB - vertexA;
        Vector3 side2 = vertexC - vertexA;

        return Vector3.Cross(side1, side2).normalized;
    }
}
