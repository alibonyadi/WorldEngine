using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class IntProperty : FunctionItem, IFunctionItem
{
    private int Int = 0;
    public IntProperty(int gets, int gives)
    {
        Init();
        Name = "Int";
        ClassName = typeof(IntProperty).FullName;
        basecolor = Color.white;
        //myFunction = Execute;


        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        IntAttrebute fl1 = new IntAttrebute(at1Rect, this);
        fl1.mInt = Int;
        fl1.SetMinMax(0, 10);
        fl1.SetName("Int");
        attrebutes.Add(fl1);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {

    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        IntAttrebute att = (IntAttrebute)attrebutes[0];
        att.mInt = int.Parse(item.attributeValue[0]);
        attrebutes[0] = att;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("Int");

        IntAttrebute att1 = (IntAttrebute)attrebutes[0];
        string stringint = att1.mInt.ToString();
        item.attributeValue.Add(stringint);

        return item;
    }

    public object Execute(object mMesh, object id)
    {
        return mMesh;
    }

}
