using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class MultiplyFloat : FunctionItem, IFunctionItem
{
    private int Float1 = 0;
    private int Float2 = 0;
    private int ressult = 0;

    public MultiplyFloat(int gets, int gives)
    {
        Init();
        Name = "Multiply Float";
        ClassName = typeof(MultiplyFloat).FullName;
        basecolor = Color.white;
        //myFunction = Execute;

        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        MultiplyAttribute fl1 = new MultiplyAttribute(at1Rect,this);
        //fl1.mFloat = Float1;
        //fl1.SetMinMax(0, 10);
        fl1.SetName("* Multiply");
        attrebutes.Add(fl1);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {

    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        //MultiplyAttribute att = (MultiplyAttribute)attrebutes[0];
        //att.mFloat = float.Parse(item.attributeValue[0]);
        //attrebutes[0] = att;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("Multiple");

        //RandomFloatAttrebute att1 = (RandomFloatAttrebute)attrebutes[0];
        //string stringint = att1.mFloat.ToString();
        //item.attributeValue.Add(stringint);

        return item;
    }

    public object Execute(object mesh, object id)
    {
        return mesh;
    }
}
