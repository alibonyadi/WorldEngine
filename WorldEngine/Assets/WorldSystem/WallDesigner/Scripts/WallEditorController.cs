using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WallDesigner
{
    public class WallEditorController
    {
        Mesh mesh;
        GameObject holder;//Camera and root for Objects instantiated
        GameObject inEditeObject;
        List<FunctionItem> allFItems;
        List<FunctionItem> allFunctions;
        List<Action> FIMenuFunctions;
        Vector2 mousePos;
        public bool IsInitialized { get; set; }

        public WallEditorController() 
        {
            holder = new GameObject("Holder");
            holder.AddComponent<Camera>();

            allFunctions = new List<FunctionItem>();
            allFItems = new List<FunctionItem>();

            inEditeObject = new GameObject("InEdit");
            inEditeObject.AddComponent<MeshFilter>();
            inEditeObject.AddComponent<MeshRenderer>();
            inEditeObject.transform.parent = holder.transform;
            inEditeObject.transform.rotation = holder.transform.rotation;
            inEditeObject.transform.position = holder.transform.position;
            inEditeObject.transform.Translate(0, 0, 5);
            inEditeObject.transform.Rotate(0, 180, 0);
            mesh = new Mesh();
            inEditeObject.GetComponent<MeshFilter>().mesh = mesh;
            IsInitialized = true;
            RefreshClasses();
            //allFItems =
        }
        public void DrawFunctionItemGUI()
        {
            mousePos = Event.current.mousePosition;
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

                Debug.Log(type.FullName);

                if (type != null && type.IsSubclassOf(typeof(FunctionItem)))
                {
                    FunctionItem item = (FunctionItem)Activator.CreateInstance(type);
                    Debug.Log(item.GetName());
                    allFunctions.Add(item);
                }
            }

        }
        public List<FunctionItem> GetAllFunctionItems()
        {
            return allFunctions;
        }
        public Action<object> GetCreateAction()
        {
            Action<object> action = null;
            action = CreateAction;
            return action;
        }
        public void CreateAction(object index)
        {
            Type type = Type.GetType(allFunctions[(int)index].ClassName);
            FunctionItem item = (FunctionItem)Activator.CreateInstance(type);
            item.position = mousePos;
            item.rect = new Rect(mousePos.x, mousePos.y, item.rect.width, item.rect.height);
            allFItems.Add(item);
        }
    }
}