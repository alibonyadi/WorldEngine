using UnityEngine;
using WallDesigner;

public class AddMaterial : FunctionItem, IFunctionItem
{
    private Material _material;
    
    public AddMaterial()
    {
        Init();
        Name = "Material";
        //outputTexture = new Texture();
        ClassName = typeof(AddMaterial).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GiveNode node = new GiveNode();
        node.AttachedFunctionItem = this;
        GiveNodes.Add((Node)node);
        CalculateRect();
        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);

    }

    public object Execute(object material)
    {
        return _material;
    }
}
