using System;
using System.Collections.Generic;
using System.IO;
using WallDesigner;
using UnityEngine;

namespace WallDesigner
{
    public class WallEditorController
    {
        Mesh mesh;
        GameObject holder;
        GameObject inEditeObject;
        List<FunctionItem> allFItems;


        public WallEditorController() 
        {
            holder = new GameObject("Holder");
            holder.AddComponent<Camera>();

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
            
            RefreshClasses();
            //allFItems =
        }

        private void RefreshClasses()
        {
            allFItems.Clear();

            string path = Application.dataPath + "/WorldSystem/WallDesigner/Functions";
            string[] files = Directory.GetFiles(path, "*.cs");

            foreach (string file in files)
            {
                string className = Path.GetFileNameWithoutExtension(file);
                Type type = Type.GetType(className);

                if (type != null && type.IsSubclassOf(typeof(FunctionItem)))
                {
                    FunctionItem item = Activator.CreateInstance(type) as FunctionItem;
                    allFItems.Add(item);
                }
            }

        }

        public List<FunctionItem> GetAllFunctionItems()
        {
            return allFItems;
        }
    }
}
