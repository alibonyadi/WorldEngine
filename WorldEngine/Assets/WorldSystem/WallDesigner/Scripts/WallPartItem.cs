using System;
using System.Collections.Generic;
using UnityEngine;

namespace WallDesigner
{
    [System.Serializable]
    public class WallPartItem
    {
        [NonSerialized]
        public Mesh mesh;
        [NonSerialized]
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
