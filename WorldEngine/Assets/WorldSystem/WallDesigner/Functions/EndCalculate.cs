using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class EndCalculate : FunctionItem, IFunctionItem
{
    WallPartItem wallItem;

    public EndCalculate()
    {
        Name = "End Calculate";
        ClassName = typeof(EndCalculate).FullName;
        basecolor = Color.red;
        myFunction = Execute;
        wallItem = new WallPartItem();
        GiveNodes = new List<Node>();
        GetNodes = new List<Node>();
        GetNode node = new GetNode();
        node.AttachedFunctionItem = this;
        GetNodes.Add((Node)node);
        node = new GetNode();
        node.color = Color.yellow;
        node.AttachedFunctionItem = this;
        GetNodes.Add((Node)node);
        //position = new Vector2(200, 200);
        CalculateRect();
        rect = new Rect(position.x, position.y, rect.width, rect.height);
    }

    public object Execute(object mesh)
    {
        if (GetNodes[0].ConnectedNode == null)
            return wallItem;

        wallItem = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(wallItem);
        return wallItem;
    }
}