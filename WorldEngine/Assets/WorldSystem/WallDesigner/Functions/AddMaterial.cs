using System;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

[System.Serializable]
public class AddMaterial : FunctionItem, IFunctionItem
{
    [NonSerialized]
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

    public static List<Material> CopyMaterials(WallPartItem item)
    {
        List<Material> materials = new List<Material>();

        for (int i = 0; i < item.material.Count; i++)
        {
            Material mat1 = new Material(Shader.Find("Standard"));
            mat1.color = item.material[i].color;
            if (item.material[i].mainTexture != null)
                mat1.mainTexture = item.material[i].mainTexture;
            materials.Add(mat1);
        }

        return materials;
    }
}
