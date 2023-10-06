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
        myFunction = Execute;
        CalculateRect();
        rect = new Rect(position.x, position.y, rect.width, rect.height);
    }

    public Mesh Execute(Mesh mesh)
    {
        Debug.Log("SecondFunction Executed!!!");
        return mesh;
    }

}
