using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    public class WallPartItem
    {
        public Mesh mesh;
        public List<Material> material;

        public WallPartItem()
        {
            mesh = new Mesh();
            //Material mat = new Material(Shader.Find("Standard"));
            material = new List<Material>();
            //material.Add(mat); 
        }
    }
}
