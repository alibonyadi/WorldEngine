using Assets.WorldSystem.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class SampleFunction : FunctionItem , IFunctionItem
{
    public SampleFunction(string name, string description, Vector2 position, List<INode> getNodes, List<INode> giveNodes) : base(name, description, position, getNodes, giveNodes)
    {
    }

    public SampleFunction(string name, string description, Vector2 position, List<INode> getNodes, List<INode> giveNodes, Rect rect) : base(name, description, position, getNodes, giveNodes, rect)
    {
    }

    public void Execute()
    {

    }
}
