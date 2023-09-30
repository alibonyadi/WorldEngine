using Assets.WorldSystem.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class secundFunction : FunctionItem, IFunctionItem
{
    public secundFunction()
    {
        Name = "SecondFunction";
        action = Execute;
    }
    public void Execute()
    {
        Debug.Log("SecondFunction Executed!!!");
    }
}
