using UnityEngine;
using WallDesigner;

[System.Serializable]
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

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        gnode.color = Color.green;
        GetNodes.Add((Node)gnode);

        GiveNode node = new GiveNode();
        node.AttachedFunctionItem = this;
        node.color = Color.yellow;
        GiveNodes.Add((Node)node);
        CalculateRect();
        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
    }

    public object Execute(object material, object id)
    {
        return _material;
    }
}
