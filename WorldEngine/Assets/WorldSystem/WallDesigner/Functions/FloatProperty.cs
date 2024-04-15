using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class FloatProperty : FunctionItem, IFunctionItem
{
    private float Float = 0;
    public FloatProperty(int gets, int gives)
    {
        Init();
        Name = "Float";
        ClassName = typeof(FloatProperty).FullName;
        basecolor = Color.white;
        //myFunction = Execute;


        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        FloatAttrebute fl1 = new FloatAttrebute(at1Rect, this);
        fl1.mFloat = Float;
        fl1.SetMinMax(0, 10);
        fl1.SetName("Float");
        attrebutes.Add(fl1);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {

    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        FloatAttrebute att = (FloatAttrebute)attrebutes[0];
        //Debug.Log(att);
        att.mFloat = float.Parse(item.attributeValue[0]);
        attrebutes[0] = att;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("Float");

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[0];
        string stringint = att1.mFloat.ToString();
        item.attributeValue.Add(stringint);

        return item;
    }

    public object Execute(object mMesh, object id)
    {
        return mMesh;
    }

}
