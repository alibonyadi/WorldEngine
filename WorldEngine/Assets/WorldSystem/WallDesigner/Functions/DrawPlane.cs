using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class DrawPlane : FunctionItem , IFunctionItem
{
    public DrawPlane()
    {
        Name = "Draw Plane";
        ClassName = typeof(DrawPlane).FullName;
        basecolor = Color.white;
        action = Execute;
        GiveNodes = new List<Node>();
        GetNodes = new List<Node>();
        Node node = new Node();
        GiveNodes.Add((Node)node);
        //position = new Vector2(200, 200);
        CalculateRect();
        rect = new Rect(position.x,position.y, rect.width,rect.height);
    }

    public void Execute()
    {
        Debug.Log("First Function Executed!!!");
    }
}