using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WallDesigner;

[System.Serializable]
public class AddTexture : FunctionItem, IFunctionItem
{
    [NonSerialized]
    private Texture outputTexture;

     
    public AddTexture(int gets, int gives)
    {
        Init();
        Name = "Texture";
        //outputTexture = new Texture();
        ClassName = typeof(AddTexture).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GiveNode node = new GiveNode();
        node.AttachedFunctionItem = this;
        GiveNodes.Add((Node)node);

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        gnode.id = 0;
        GetNodes.Add(gnode);

        CalculateRect();
        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        TextureAttribute att1 = new TextureAttribute(at1Rect,this);
        att1.SetName(Name);
        //Debug.Log(att1.texture.ToString());
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

        TextureAttribute att = (TextureAttribute)attrebutes[0];
        att.adress = item.attributeValue[0];
        att.texture = TextureAttribute.GenerateTextureFromPath(att.adress);
        attrebutes[0] = att;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("TextureAttribute");

        TextureAttribute att1 = (TextureAttribute)attrebutes[0];
        if (att1.texture != null)
        {
            string stringtexturePath = att1.adress;
            item.attributeValue.Add(stringtexturePath);
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

    public object Execute(object item, object id)
    {
        WallItem mitem = new WallItem();
        TextureAttribute att1 = (TextureAttribute)attrebutes[0];
        Material mat = new Material(Shader.Find("Standard"));
        mat.mainTexture = att1.texture;
        if (GetNodes[0].ConnectedNode != null)
        {
            mitem = (WallItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(mitem, GetNodes[0].ConnectedNode.id);
            for (int j = 0; j < mitem.wallPartItems.Count; j++)
            {
                if (mitem.wallPartItems[j] == null)
                    continue;

                if (mitem.wallPartItems[j].material.Count > 0)
                {
                    for (int i = 0; i < mitem.wallPartItems[j].material.Count; i++)
                    {
                        mitem.wallPartItems[j].material[i].mainTexture = att1.texture;
                    }
                }
                else
                {
                    mitem.wallPartItems[j].material.Add(mat);
                }
            }
            return mitem;
        }
        else
        {
            WallItem wallitem = (WallItem)item;
            for (int j = 0; j < wallitem.wallPartItems.Count; j++)
            {
                if (wallitem.wallPartItems[j] == null)
                    continue;
                if (wallitem.wallPartItems[j].material.Count > 0)
                {
                    //int count = wallitem.material.Count;
                    //wallitem.material.Clear();
                    List<Material> mats = new List<Material>();
                    for (int i = 0; i < wallitem.wallPartItems[j].material.Count; i++)
                    {
                        Material mat1 = new Material(Shader.Find("Standard"));
                        mat1.color = wallitem.wallPartItems[j].material[i].color;
                        mat1.mainTexture = att1.texture;
                        mats.Add(mat1);
                    }
                    wallitem.wallPartItems[j].material.Clear();
                    wallitem.wallPartItems[j].material = mats;
                }
                else
                {
                    wallitem.wallPartItems[j].material.Add(mat);
                }
            }
            return wallitem;
        }
    }
}
