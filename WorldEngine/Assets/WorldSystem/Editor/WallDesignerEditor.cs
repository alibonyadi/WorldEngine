using UnityEditor;
using UnityEngine;
using WallDesigner;

public class WallDesignedEditor : EditorWindow
{

    WallEditorController walleditor;
    RightClickMenu menuController;
    ConnectLineController connectLineController;
    bool IsInitialized=false;
    [UnityEditor.MenuItem("WorldEngine/WallEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(WallDesignedEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("Right Click For Menu!!!", EditorStyles.boldLabel);

        if (!WallEditorController.Instance.IsInitialized)
        {
            if (GUILayout.Button("Initialize WallEdiotr"))
            {
                IsInitialized = true;
                WallEditorController.Instance.IsInitialized = true;
                walleditor = WallEditorController.Instance;
                menuController = new RightClickMenu(walleditor);
            }
        }
        else
        {
            walleditor.DrawFunctionItemGUI();
        }

        if (Event.current.type == EventType.ContextClick)
        {
            //GenericMenu menu = new GenericMenu();
            GenericMenu menu = menuController.GetAllMenuItems();
            menu.ShowAsContext();
        }

        if(Event.current.type == EventType.MouseUp)
        {
            ConnectLineController.Instance.CheckClick();
        }

        Repaint();
    }

}
