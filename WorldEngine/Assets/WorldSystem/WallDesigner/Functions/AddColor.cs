using System;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

[System.Serializable]
public class AddColor : FunctionItem, IFunctionItem
{
    [NonSerialized]
    private Color outputColor;

    public AddColor()
    {
        Init();
        Name = "Color";
        outputColor = Color.white;
        ClassName = typeof(AddColor).FullName;
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
        RandomColorAttrebute colorAtt1 = new RandomColorAttrebute(at1Rect);
        colorAtt1.mColor = Color.white;
        colorAtt1.SetName("Random Color");
        attrebutes.Add(colorAtt1);
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

        RandomColorAttrebute att = (RandomColorAttrebute)attrebutes[0];
        att.SetColor(att.mColor);
        //att.
        attrebutes[0] = att;
    }

    private Color parseColor(string colorString)
    {
        string[] colorValues = colorString.Split('(', ',', ')'); // this splits the string into an array containing ["RGBA", "1.000", "1.000", "1.000", "1.000"]

        float r = float.Parse(colorValues[1]);
        float g = float.Parse(colorValues[2]);
        float b = float.Parse(colorValues[3]);
        float a = float.Parse(colorValues[4]);

        Color parsedColor = new Color(r, g, b, a); // create the new color object from the parsed values
        return parsedColor;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("RandomColorAttrebute");

        RandomColorAttrebute att1 = (RandomColorAttrebute)attrebutes[0];
        string stringcolor = att1.mColor.ToString();
        item.attributeValue.Add(stringcolor);

        if (GetNodes[0].ConnectedNode!=null)
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
        List<WallPartItem> mitem = new List<WallPartItem>();
        RandomColorAttrebute att1 = (RandomColorAttrebute)attrebutes[0];
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = att1.mColor;
        if (GetNodes[0].ConnectedNode != null)
        {
            mitem = (List<WallPartItem>)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(mitem, GetNodes[0].ConnectedNode.id);
            for (int j = 0; j < mitem.Count; j++)
            {
                if (mitem[j] == null)
                    continue;

                if (mitem[j].material.Count > 0)
                {
                    for (int i = 0; i < mitem[j].material.Count; i++)
                    {
                        mitem[j].material[i].color = att1.mColor;
                    }
                }
                else
                {
                    mitem[j].material.Add(mat);
                }
            }
            return mitem;
        }
        else
        {
            List<WallPartItem> wallitem = (List<WallPartItem>)item;
            for (int j = 0; j < wallitem.Count; j++)
            {

                if (wallitem[j] == null) 
                    continue;

                if (wallitem[j].material.Count > 0)
                {
                    //int count = wallitem.material.Count;
                    //wallitem.material.Clear();
                    List<Material> mats = new List<Material>();
                    for (int i = 0; i < wallitem[j].material.Count; i++)
                    {
                        Material mat1 = new Material(Shader.Find("Standard"));
                        if (wallitem[j].material[i].mainTexture != null)
                            mat1.mainTexture = wallitem[j].material[i].mainTexture;
                        mat1.color = att1.mColor;
                        mats.Add(mat1);
                    }

                    //wallitem.material.Clear();
                    wallitem[j].material = mats;
                }
                else
                {
                    wallitem[j].material.Add(mat);
                }
            }
            return wallitem;
        }
    }
}