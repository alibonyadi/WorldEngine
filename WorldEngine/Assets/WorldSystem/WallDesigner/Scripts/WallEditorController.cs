using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WallDesigner
{
    public class WallEditorController
    {
        Mesh mesh;
        public bool canRepaint=false;
        private static WallEditorController instance;
        public GameObject holder;//Camera and root for Objects instantiated
        public GameObject inEditeObject;
        List<FunctionItem> allFItems;
        List<FunctionItem> allFunctions;
        List<Action> FIMenuFunctions;
        public int EndItemIndex;
        public FunctionItem EndItem;
        public Vector2 mousePos;
        public bool canShowMenu = true;
        public bool IsInitialized { get; set; }
        public bool autoDraw { get; set; } = true;
        public bool WireFrame { get; set; }
        private WallEditorController() 
        {
            //Material material = new Material(Shader.Find("VR/SpatialMapping/Wireframe"));
            Material material = new Material(Shader.Find("Standard"));
     
            holder = new GameObject("Holder");
            holder.AddComponent<Camera>();
            holder.GetComponent<Camera>().clearFlags= CameraClearFlags.SolidColor;
            holder.GetComponent<Camera>().backgroundColor= Color.black;
            allFunctions = new List<FunctionItem>();
            allFItems = new List<FunctionItem>();
            inEditeObject = new GameObject("InEdit");
            inEditeObject.AddComponent<MeshFilter>();
            inEditeObject.AddComponent<MeshRenderer>();

            inEditeObject.transform.parent = holder.transform;
            inEditeObject.transform.rotation = holder.transform.rotation;
            inEditeObject.transform.position = holder.transform.position;
            inEditeObject.transform.Translate(0, 0, 5);
            inEditeObject.transform.Rotate(270, 0, 0);
            mesh = new Mesh();
            inEditeObject.GetComponent<MeshFilter>().mesh = mesh;
            inEditeObject.GetComponent<MeshRenderer>().material = material;
            //IsInitialized = true;
            RefreshClasses();
            //allFItems = 
        }
        public void Reset()
        {
            IsInitialized = false;
            ConnectLineController.Instance.SetInDragNode(null);
            EndItem = null;
            allFunctions.Clear();
            allFItems.Clear();
            RefreshClasses();
        }
        public static WallEditorController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WallEditorController();
                }
                return instance;
            }
        }
        public void DrawFunctionItemGUI()
        {
            if (allFItems.Count > 0)
            {
                for (int i = 0; i < allFItems.Count; i++)
                {
                    allFItems[i].Draw();
                }
            }
        }
        private void RefreshClasses()
        {
            allFunctions.Clear();
            string path = Application.dataPath + "/WorldSystem/WallDesigner/Functions";
            string[] files = Directory.GetFiles(path, "*.cs");
            foreach (string file in files)
            {
                string className = Path.GetFileNameWithoutExtension(file);
                Type type = Type.GetType(className);
                if (type != null && type.IsSubclassOf(typeof(FunctionItem)))
                {
                    FunctionItem item = (FunctionItem)Activator.CreateInstance(type);
                    allFunctions.Add(item);
                }
                if (allFunctions[allFunctions.Count - 1].GetType() == typeof(EndCalculate))
                {
                    EndItemIndex = allFunctions.Count - 1;
                    //CreateAction(EndItemIndex);
                }
            }
        }
        public List<FunctionItem> GetAllFunctionItems() => allFunctions;
        public List<FunctionItem> GetAllCreatedItems() => allFItems;
        public void SetAllCreatedItems(List<FunctionItem> newFunctionsItems) => allFItems = newFunctionsItems;
        public Action<object> GetCreateAction()
        {
            Action<object> action = null;
            action = CreateAction;
            return action;
        }
        public void CreateAction(object index)
        {
            if (EndItem != null && (int)index == EndItemIndex)
            {
                Debug.LogWarning("Just One " + allFunctions[(int)index].ClassName + " can exist!!!");
                return;
            }
            Type type = Type.GetType(allFunctions[(int)index].ClassName);
            FunctionItem item = (FunctionItem)Activator.CreateInstance(type);
            item.position = mousePos - BoardController.Instance.boardPosition;
            Vector2 tempposition = item.position + BoardController.Instance.boardPosition;
            item.rect = new Rect(tempposition.x - item.rect.width/2, tempposition.y - item.rect.height / 2, item.rect.width, item.rect.height);
            allFItems.Add(item);
            if ((int)index == EndItemIndex)
            {
                EndItem = item;
            }
        }

        public void RepaintBoard()
        {
            canRepaint = true;
        }
    }
    
}