using UnityEditor;
using UnityEngine;
using WallDesigner;

public class WallDesignedEditor : EditorWindow
{
    WallEditorController walleditor;
    RightClickMenu menuController;
    BoardController boardController;
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
                menuController = new RightClickMenu();
                boardController = BoardController.Instance;
            }
        }
        else
        {
            if(walleditor == null)
                walleditor = WallEditorController.Instance;

            if (walleditor.holder == null)
            {
                walleditor.CreateOrGetHolder();
            }

            walleditor.mousePos = Event.current.mousePosition;
            BoardController.Instance.BoardControlling();
            /*int selected = 0;
            string[] menus = { "Reset", "Draw", "ccc" };
            selected = GUILayout.Toolbar(selected, menus);*/

            if (GUILayout.Button("Reset"))
            {
                walleditor.Reset();
                BoardController.Instance.boardPosition = Vector2.zero;
            }
            if (GUILayout.Button("Draw"))
            {
                FunctionProccesor.Instance.ProcessFunctions();
            }
            if (GUILayout.Button("SaveWall"))
            {
                SaveLoadManager.SaveAllItems();
            }
            if (GUILayout.Button("LoadWall"))
            {
                SaveLoadManager.LoadAllItems();
            }
            GUILayout.Label(BoardController.Instance.boardPosition.ToString());

            walleditor.autoDraw = GUILayout.Toggle(walleditor.autoDraw, "Auto Draw");
            walleditor.WireFrame = GUILayout.Toggle(walleditor.WireFrame, "WireFrame");
            if(walleditor.WireFrame)
            {
                walleditor.holder.GetComponent<Camera>().SetReplacementShader(Shader.Find("VR/SpatialMapping/Wireframe"), "");
            }
            else
                walleditor.holder.GetComponent<Camera>().SetReplacementShader(Shader.Find("Standard"), ""); 

            walleditor.DrawFunctionItemGUI();

            if (walleditor.canRepaint)
            {
                walleditor.canRepaint = false;
                Repaint();
            }
        }

        if (Event.current.type == EventType.ContextClick)
        {
            //GenericMenu menu = new GenericMenu();
            GenericMenu menu = menuController.GetAllMenuItems();
            menu.ShowAsContext();

        }

        if (Event.current.type == EventType.MouseUp)
        {
            ConnectLineController.Instance.CheckWindowsClick();
        }   
    }
}
