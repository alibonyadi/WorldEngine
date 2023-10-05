using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WallDesigner
{
    public class FunctionProccesor
    {
        Stack<FunctionItem> Items;
        private Mesh mesh;
        private static FunctionProccesor instance;
        private FunctionProccesor()
        {

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

        }
            
    }
}
