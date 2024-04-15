using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WallDesigner;

public class CreateObject : FunctionItem, IFunctionItem
{
    private List<GameObject> m_obj = new List<GameObject>();
    private string path;
    public CreateObject(int gets, int gives)
    {
        Init();
        Name = "Create Base Object";
        ClassName = typeof(CreateObject).FullName;
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
        GetFileAttrebute att1 = new GetFileAttrebute(at1Rect, this);
        att1.folderlocation = Application.dataPath + "\\WorldSystem\\BuildingEditor\\Modules";
        att1.SetName(Name);
        att1.extension = "wall,Building,mudule";
        attrebutes.Add(att1);


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

        if (item.attributeValue.Count > 0)//auto adjust
        {
            GetFileAttrebute att = (GetFileAttrebute)attrebutes[0];
            att.adress = item.attributeValue[0];
            //att.texture = GenerateTextureFromPath(att.adress);
            attrebutes[0] = att;
        }
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("Get File Att");

        GetFileAttrebute att1 = (GetFileAttrebute)attrebutes[0];
        if (att1.adress != null)
        {
            string stringFilePath = att1.adress;
            item.attributeValue.Add(stringFilePath);
        }

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
        WallItem wpi = (WallItem)mMesh;
        if (GetNodes[0].ConnectedNode != null)
            wpi = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);

        GetFileAttrebute att1 = attrebutes[0] as GetFileAttrebute;
        path = att1.adress;

        if (path == null)
        {
            return wpi;
        }

        foreach (GameObject go in m_obj)
        {
            GameObject.DestroyImmediate(go);
        }

        m_obj.Clear();
        GameObject inEdit = GameObject.Find("InEdit");
        for (int i = 0; i < wpi.wallPartItems.Count; i++)
        {
            GameObject go = new GameObject("LotPart");
            Debug.Log(wpi.isInEditMode + "<- edit mode --- caller = " + wpi.Caller);
            if (wpi.isInEditMode)
                go.transform.parent = inEdit.transform;
            else
            {
                go.transform.parent = wpi.Caller.transform;
            }
            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.mesh = wpi.wallPartItems[i].mesh;
            go.AddComponent<MeshRenderer>();
            PruceduralRoad pr = go.AddComponent<PruceduralRoad>();
            pr.SetPath(path);
            pr.GenerateMeshes();
            m_obj.Add(go);
        }

        return wpi;
    }
}
