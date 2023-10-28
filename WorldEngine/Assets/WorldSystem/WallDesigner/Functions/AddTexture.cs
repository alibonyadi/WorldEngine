using System;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

[System.Serializable]
public class AddTexture : FunctionItem, IFunctionItem
{
    [NonSerialized]
    private Texture outputTexture;

     
    public AddTexture()
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
        TextureAttribute att1 = new TextureAttribute(at1Rect);
        att1.SetName(Name);
        //Debug.Log(att1.texture.ToString());
        attrebutes.Add(att1);
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
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
            item.getnodeConnected.Add(connectedGetNodeNumber);
        }

        if (GiveNodes[0].ConnectedNode != null)
        {
            int connectedGiveNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GiveNodes[0].ConnectedNode.AttachedFunctionItem);
            item.getnodeConnected.Add(connectedGiveNodeNumber);
        }

        return item;
    }

    public object Execute(object item, object id)
    {
        WallPartItem mitem = new WallPartItem();
        TextureAttribute att1 = (TextureAttribute)attrebutes[0];
        Material mat = new Material(Shader.Find("Standard"));
        mat.mainTexture = att1.texture;
        if (GetNodes[0].ConnectedNode != null)
        {
            mitem = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(mitem, GetNodes[0].ConnectedNode.id);
            if (mitem.material.Count > 0)
            {
                for (int i = 0; i < mitem.material.Count; i++)
                {
                    mitem.material[i].mainTexture = att1.texture;
                }
            }
            else
            {
                mitem.material.Add(mat);
            }
            return mitem;
        }
        else
        {
            WallPartItem wallitem = (WallPartItem)item;
            if(wallitem.material.Count > 0)
            {
                //int count = wallitem.material.Count;
                //wallitem.material.Clear();
                List<Material> mats = new List<Material>();
                for (int i = 0; i < wallitem.material.Count; i++)
                {
                    Material mat1 = new Material(Shader.Find("Standard"));
                    mat1.color = wallitem.material[i].color;
                    mat1.mainTexture = att1.texture;
                    mats.Add(mat1);
                }
                wallitem.material.Clear();
                wallitem.material = mats;
            }
            else
            {
                wallitem.material.Add(mat);
            }
            return wallitem;
        }
    }
}
