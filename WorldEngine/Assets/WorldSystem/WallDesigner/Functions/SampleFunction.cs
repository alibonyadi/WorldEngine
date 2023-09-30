using Assets.WorldSystem.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class SampleFunction : FunctionItem , IFunctionItem
{
    public SampleFunction()
    {
        Name = "FirstFunction";
        action = Execute;
    }

    public void Execute()
    {
        Debug.Log("First Function Executed!!!");
    }
}