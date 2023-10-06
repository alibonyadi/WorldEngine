using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Progress;

namespace WallDesigner
{
    public class FunctionProccesor
    {
        Stack<FunctionItem> Items;
        private Mesh mesh;
        private static FunctionProccesor instance;
        private FunctionProccesor()
        { 
            Items = new Stack<FunctionItem>();
        }
        public static FunctionProccesor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FunctionProccesor();
                }
                return instance;
            }
        }


        public void ProcessFunctions()
        {
            if (WallEditorController.Instance.EndItem == null)
                return;

            FunctionItem endItem = WallEditorController.Instance.EndItem;

            if (endItem.GetNodes[0].ConnectedNode == null)
                return;

            Items.Push(endItem);

            FunctionItem item = endItem.GetNodes[0].ConnectedNode.AttachedFunctionItem;
            Items.Push(item);
            Debug.Log(endItem.GetNodes[0].AttachedFunctionItem);
            //just for test
            int i = 2;
            while(item.GetNodes != null && item.GetNodes.Count>0)
            {
                if (item.GetNodes[0].ConnectedNode != null)
                {
                    item = item.GetNodes[0].ConnectedNode.AttachedFunctionItem;
                    Items.Push(item);
                    i++;
                }
                else
                    break;
            } 
            Debug.Log(i + " function added to stack!!!");
            //FunctionItem functionItem = Items.Pop();
            Mesh mesh = new Mesh();
            while(Items.Count>0)
            {
                mesh = Items.Pop().myFunction(mesh);
            }

            DrawEndMesh(mesh);
            
        }

        private static void DrawEndMesh(Mesh mesh)
        {
            WallEditorController.Instance.inEditeObject.GetComponent<MeshFilter>().mesh = mesh;
        }
         
        public Mesh processOneAction(Mesh mesh,Action<Mesh> ac)
        {

            return mesh;
        }
            
    }
}
