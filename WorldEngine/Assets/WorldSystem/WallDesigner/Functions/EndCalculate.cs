using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class EndCalculate : FunctionItem, IFunctionItem
{
    public EndCalculate()
    {
        Name = "End Calculate";
        ClassName = typeof(EndCalculate).FullName;
        basecolor = Color.red;
        action = Execute;
        GiveNodes = new List<Node>();
        GetNodes = new List<Node>();
        GetNode node = new GetNode();
        node.AttachedFunctionItem = this;
        GetNodes.Add((Node)node);
        //position = new Vector2(200, 200);
        CalculateRect();
        rect = new Rect(position.x, position.y, rect.width, rect.height);
    }

    public void Execute()
    {
        Debug.Log("END Function Executed!!!");
    }
}
