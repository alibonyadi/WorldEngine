using UnityEditor;
using UnityEngine;
using WallDesigner;

public class WallDesignedEditor : EditorWindow
{

    WallEditorController walleditor;
    RightClickMenu menuController;
    [UnityEditor.MenuItem("WorldEngine/WallEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(WallDesignedEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("My Custom Editor Window", EditorStyles.boldLabel);

        if (GUILayout.Button("Initialize WallEdiotr"))
        {
            walleditor = new WallEditorController();
            menuController = new RightClickMenu( walleditor);
        }

        if (Event.current.type == EventType.ContextClick)
        {
            //GenericMenu menu = new GenericMenu();
            GenericMenu menu = menuController.GetAllMenuItems();
            menu.ShowAsContext();
        }

    }

    public void AddToInstanceList(int item)
    {
        //walleditor.al
    }
}
