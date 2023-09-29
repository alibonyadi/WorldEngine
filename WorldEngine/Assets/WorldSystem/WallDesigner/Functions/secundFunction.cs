using Assets.WorldSystem.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class secundFunction : FunctionItem, IFunctionItem
{
    public secundFunction(string name, string description, Vector2 position, List<INode> getNodes, List<INode> giveNodes) : base(name, description, position, getNodes, giveNodes)
    {
    }

    public secundFunction(string name, string description, Vector2 position, List<INode> getNodes, List<INode> giveNodes, Rect rect) : base(name, description, position, getNodes, giveNodes, rect)
    {
    }

    public void Execute()
    {

    }
}
