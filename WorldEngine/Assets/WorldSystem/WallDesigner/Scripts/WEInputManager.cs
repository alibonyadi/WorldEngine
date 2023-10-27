using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WEInputManager
{
    public bool isItemOnDrag;
    public bool isBoardOnDrag;

    private static WEInputManager instance;
    private WEInputManager()
    {

    }

    public static WEInputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WEInputManager();
            }
            return instance;
        }
    }



}
