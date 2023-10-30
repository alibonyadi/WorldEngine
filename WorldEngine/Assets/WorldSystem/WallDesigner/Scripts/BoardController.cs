using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WallDesigner;

public class BoardController
{
    public Vector2 boardPosition;
    Vector2 tempMousePosition;
    bool oneTimeCheck = false;
    private static BoardController instance;
    private BoardController()
    {
       
    }

    public static BoardController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BoardController();
            }
            return instance;
        }
    }

    public void BoardControlling()
    {
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 2 && !WEInputManager.Instance.isItemOnDrag)
        {
            //Debug.Log("On Drag!!! StartMousepos = "+ tempMousePosition+"  Currentmouse"+ Event.current.mousePosition);
            if (!oneTimeCheck)
            {
                tempMousePosition = Event.current.mousePosition;
                oneTimeCheck=true;
            }

            WEInputManager.Instance.isBoardOnDrag = true;

            boardPosition -= tempMousePosition - Event.current.mousePosition;

            foreach(FunctionItem item in WallEditorController.Instance.GetAllCreatedItems())
            {
                item.UpdateBoardPos();
            }

            WallEditorController.Instance.RepaintBoard();
            tempMousePosition = Event.current.mousePosition;
        }

        if(oneTimeCheck && (Event.current.type == EventType.MouseUp || WEInputManager.Instance.isItemOnDrag))
        {
            //Debug.Log("Finished Drag!!!");
            oneTimeCheck = false;
            WEInputManager.Instance.isBoardOnDrag = false;
        }
    }
}
