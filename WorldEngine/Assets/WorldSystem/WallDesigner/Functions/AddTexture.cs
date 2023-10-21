using UnityEngine;
using WallDesigner;

public class AddTexture : FunctionItem, IFunctionItem
{
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
        CalculateRect();
        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        TextureAttribute att1 = new TextureAttribute(at1Rect);
        att1.SetName(Name);
        attrebutes.Add(att1);
    }

    public object Execute(object item, object id)
    {
        WallPartItem wallitem = (WallPartItem)item;
        TextureAttribute att1 = (TextureAttribute)attrebutes[0];
        wallitem.material.mainTexture = att1.texture;
        return wallitem;
    }
}
