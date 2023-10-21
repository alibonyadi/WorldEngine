using UnityEngine;

namespace WallDesigner
{
    public class WallPartItem
    {
        public Mesh mesh;
        public Material material;

        public WallPartItem()
        {
            mesh = new Mesh();
            material = new Material(Shader.Find("Standard"));
        }
    }
}
