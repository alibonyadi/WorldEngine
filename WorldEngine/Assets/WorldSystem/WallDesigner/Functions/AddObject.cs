using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.UIElements;
using WallDesigner;

public class AddObject : FunctionItem, IFunctionItem
{
    private string path;
    public Vector3 localposition;
    public Vector3 localrotation;
    public GameObject myObject;
    public AddObject(int gets,int gives)
    {
        Init();
        Name = "Add Object";
        ClassName = typeof(AddObject).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);

        GiveNode givenode1 = new GiveNode();
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);

        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        GetFileAttrebute att1 = new GetFileAttrebute(at1Rect);
        att1.folderlocation = Application.dataPath + "\\Resources";
        att1.extension = "";
        att1.SetName(Name);
        attrebutes.Add(att1);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.getnodeConnectedFI.Count > 0)
        {
            GetNodes[0].ConnectedNode = functionItems[item.getnodeConnectedFI[0]].GiveNodes[item.getnodeItems[0]];
        }

        if (item.givenodeConnectedFI.Count > 0)
        {
            GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GetNodes[item.givenodeItems[0]];
        }
    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        GetFileAttrebute att = (GetFileAttrebute)attrebutes[0];
        att.adress = item.attributeValue[0];
        //att.texture = GenerateTextureFromPath(att.adress);
        attrebutes[0] = att;

        /*IntAttrebute att2 = (IntAttrebute)attrebutes[1];
        att2.mInt = int.Parse(item.attributeValue[1]);
        att2.SetMinMax(1, 10);
        attrebutes[1] = att2;*/
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("GetFileAtt");

        GetFileAttrebute att1 = (GetFileAttrebute)attrebutes[0];
        if (att1.adress != null)
        {
            string stringFilePath = att1.adress;
            item.attributeValue.Add(stringFilePath);
        }

        /*IntAttrebute att2 = (IntAttrebute)attrebutes[1];
        string stringIntAtt = att2.mInt.ToString();
        item.attributeValue.Add(stringIntAtt);*/


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
        WallItem wpi = new WallItem();

        if (GetNodes[0].ConnectedNode != null)
            wpi = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wpi, GetNodes[0].ConnectedNode.id);

        localposition = wpi.wallPartItems[0].mesh.vertices[0];
        //Debug.Log(localposition);

        GetFileAttrebute att1 = attrebutes[0] as GetFileAttrebute;
        path = att1.adress;

        if (path == null)
        {
            return wpi;
        }
        GameObject inEdit = GameObject.Find("InEdit");
        Name = Path.GetFileNameWithoutExtension(path);
        string seprator = "Resources/";
        string[] str = path.Split(seprator,StringSplitOptions.RemoveEmptyEntries);

        //Object obj = EditorUtility.FindAsset(path);
        //var obj = AssetDatabase.LoadAssetAtPath(path,Object);
        path = str[str.Length - 1];
        str = path.Split(".", StringSplitOptions.RemoveEmptyEntries);
        path = str[0];
        if (myObject == null)
        {
            GameObject Prefab = Resources.Load(path) as GameObject;
            myObject = GameObject.Instantiate(Prefab, inEdit.transform.position, Quaternion.identity);
        }

        if (wpi.isInEditMode)
            myObject.transform.parent = inEdit.transform;
        else
        {
            myObject.transform.parent = wpi.Caller.transform;
            myObject.transform.position = wpi.Caller.transform.position;
        }
        
        myObject.transform.localPosition = localposition;
        myObject.transform.localRotation = Quaternion.Euler(localrotation);

        WallItem output = new WallItem();

        return output;
    }
}
