using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class AddColor : FunctionItem, IFunctionItem
{
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
        colorAtt1.mColor = Random.ColorHSV();
        colorAtt1.SetName("Random Color");
        attrebutes.Add(colorAtt1);
    }

    public object Execute(object item, object id)
    {
        WallPartItem mitem = new WallPartItem();
        RandomColorAttrebute att1 = (RandomColorAttrebute)attrebutes[0];
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = att1.mColor;
        if (GetNodes[0].ConnectedNode != null)
        {
            mitem = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(mitem, GetNodes[0].ConnectedNode.id);
            if (mitem.material.Count > 0)
            {
                for (int i = 0; i < mitem.material.Count; i++)
                {
                    mitem.material[i].color = att1.mColor;
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
            if (wallitem.material.Count > 0)
            {
                //int count = wallitem.material.Count;
                //wallitem.material.Clear();
                List<Material> mats = new List<Material>();
                for (int i = 0; i < wallitem.material.Count; i++)
                {
                    Material mat1 = new Material(Shader.Find("Standard"));
                    if(wallitem.material[i].mainTexture != null)
                        mat1.mainTexture = wallitem.material[i].mainTexture;
                    mat1.color = att1.mColor;
                    mats.Add(mat1);
                }

                //wallitem.material.Clear();
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
