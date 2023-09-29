using UnityEditor;
using UnityEngine;
using WallDesigner;

public class WallDesignedEditor : EditorWindow
{

    WallEditorController walleditor;
    RightClickMenu menuController;
    [MenuItem("WorldEngine/WallEditor")]
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
            menuController = new RightClickMenu();
        }

        if (Event.current.type == EventType.ContextClick)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Option 1"), false, OnOption1Selected);
            menu.AddItem(new GUIContent("Option 2"), false, OnOption2Selected);
            menu.ShowAsContext();
        }

    }

    void OnOption1Selected()
    {
        Debug.Log("Option 1 selected");
    }

    void OnOption2Selected()
    {
        Debug.Log("Option 2 selected");
    }
}
