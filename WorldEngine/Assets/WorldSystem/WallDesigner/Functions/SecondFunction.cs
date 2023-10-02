using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;


public class SecondFunction : FunctionItem, IFunctionItem
{
    public SecondFunction()
    {
        Name = "second Function";
        ClassName = typeof(SecondFunction).FullName;
        GiveNodes = new List<Node>();
        GetNodes = new List<Node>();
        Node node = new Node();
        GiveNodes.Add((Node)node);
        action = Execute;
        CalculateRect();
        rect = new Rect(position.x, position.y, rect.width, rect.height);
    }

    public void Execute()
    {
        Debug.Log("SecondFunction Executed!!!");
    }

}
