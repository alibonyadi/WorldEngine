using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WallDesigner;


public class SecondFunction : FunctionItem, IFunctionItem
{
    public SecondFunction()
    {
        Name = "second Function";
        ClassName = typeof(SecondFunction).FullName;
        GiveNodes = new List<Node>();
        GetNodes = new List<Node>();
        Node node = new Node();
        GiveNodes.Add((Node)node);
        myFunction = Execute;
        CalculateRect();
        rect = new Rect(position.x, position.y, rect.width, rect.height);
    }

    public object Execute(object mesh)
    {
        Debug.Log("SecondFunction Executed!!!");

        /*public static Mesh CombineMeshes(Mesh[] meshes, Material[] materials)
        {
            CombineInstance[] combineInstances = new CombineInstance[meshes.Length];
            for (int i = 0; i < meshes.Length; i++)
            {
                combineInstances[i].mesh = meshes[i];
                combineInstances[i].transform = Matrix4x4.identity;
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combineInstances, true);
            combinedMesh.subMeshCount = materials.Length;

            for (int i = 0; i < materials.Length; i++)
            {
                int[] submeshIndices = new int[meshes.Length];
                for (int j = 0; j < meshes.Length; j++)
                    submeshIndices[j] = i;

                combinedMesh.SetTriangles(combinedMesh.GetTriangles(i).Concat(meshes.SelectMany(mesh => mesh.GetTriangles(i))).ToArray(), i);
            }

            combinedMesh.name = "CombinedMesh";
            combinedMesh.RecalculateBounds();
            return combinedMesh;
        }*/
        return mesh;
    }

}
