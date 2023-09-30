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
        action = Execute;
    }

    public void Execute()
    {
        Debug.Log("SecondFunction Executed!!!");
    }

}
