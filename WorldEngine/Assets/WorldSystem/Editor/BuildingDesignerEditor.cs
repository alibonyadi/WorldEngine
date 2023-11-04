/*using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WallDesigner;

public class BuildingDesignerEditor : EditorWindow
{
    BuildingDesignerController BuildingEditor;
    RightClickMenu menuController;
    BoardController boardController;
    ConnectLineController connectLineController;

    [UnityEditor.MenuItem("WorldEngine/BuildingDesigner")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(BuildingDesignerEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("Building Editor V0.0.1", EditorStyles.boldLabel);
        if (!WallEditorController.Instance.IsInitialized)
        {
            if (GUILayout.Button("Initialize WallEdiotr"))
            {
                //IsInitialized = true;
                BuildingDesignerController.Instance.IsInitialized = true;
                BuildingEditor = BuildingDesignerController.Instance;
                menuController = new RightClickMenu();
                boardController = BoardController.Instance;
            }
        }
        else
        {
            if (BuildingEditor == null)
                BuildingEditor = BuildingDesignerController.Instance;

            if (BuildingEditor.holder == null)
            {
                BuildingEditor.CreateOrGetHolder();
            }
            BuildingEditor.mousePos = Event.current.mousePosition;
            BoardController.Instance.BoardControlling();

        }
    }
*/