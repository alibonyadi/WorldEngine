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
        Stack<FunctionItem> MeshItems;

        private Mesh mesh;
        private static FunctionProccesor instance;
        private FunctionProccesor()
        { 
            MeshItems = new Stack<FunctionItem>();
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

            WallPartItem item = new WallPartItem();
            item = (WallPartItem)endItem.myFunction(item,0);
            DrawEndMesh(item.mesh);
            SetEndMaterials(item.material);
        }

        private void SetEndMaterials(Material material)
        {
            WallEditorController.Instance.inEditeObject.GetComponent<MeshRenderer>().material = material;
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
