using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class RandomFloat : FunctionItem, IFunctionItem
{
    private float Float = 0;
    public RandomFloat(int gets, int gives)
    {
        Init();
        Name = "Random Float";
        ClassName = typeof(RandomFloat).FullName;
        basecolor = Color.white;
        //myFunction = Execute;

        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        RandomFloatAttrebute fl1 = new RandomFloatAttrebute(at1Rect, this);
        fl1.SetMinMax(0, 1);
        //fl1.mFloat = Float;
        fl1.SetName("Random Float");
        attrebutes.Add(fl1);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {

    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        //RandomFloatAttrebute att = (RandomFloatAttrebute)attrebutes[0];
        //att.mFloat = float.Parse(item.attributeValue[0]);
        //attrebutes[0] = att;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("Random");

        //RandomFloatAttrebute att1 = (RandomFloatAttrebute)attrebutes[0];
        //string stringint = att1.mFloat.ToString();
        //item.attributeValue.Add(stringint);

        return item;
    }

    public object Execute(object mMesh, object id)
    {
        return mMesh;
    }

}
