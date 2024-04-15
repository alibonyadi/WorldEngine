using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class MinusFloat : FunctionItem, IFunctionItem
{
    private int Float1 = 0;
    private int Float2 = 0;
    private int ressult = 0;

    public MinusFloat(int gets, int gives)
    {
        Init();
        Name = "Minus Float";
        ClassName = typeof(MinusFloat).FullName;
        basecolor = Color.white;
        //myFunction = Execute;

        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);
        MinusAttribute fl1 = new MinusAttribute(at1Rect, this);
        //fl1.mFloat = Float1;
        //fl1.SetMinMax(0, 10);
        fl1.SetName("- Minus");
        attrebutes.Add(fl1);
    }

    public object Execute(object mesh, object id)
    {
        return mesh;
    }
}
