using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WallDesigner;


public class CombineItems : FunctionItem, IFunctionItem
{
    public CombineItems()
    {
        Init();
        Name = "Combine Meshes";
        ClassName = typeof(CombineItems).FullName;
        basecolor = Color.white;
        GiveNodes = new List<Node>();
        GetNodes = new List<Node>();

        GiveNode node = new GiveNode();
        node.AttachedFunctionItem = this;
        GiveNodes.Add((Node)node);

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        gnode.id = 0;
        GetNodes.Add(gnode);

        GetNode gnode2 = new GetNode();
        gnode2.AttachedFunctionItem = this;
        gnode2.id = 1;
        GetNodes.Add(gnode2);

        myFunction = Execute;
        CalculateRect();
        rect = new Rect(position.x, position.y, rect.width, rect.height);


    }

    public object Execute(object mesh, object id)
    {
        WallPartItem item = new WallPartItem();
        if (GetNodes[0].ConnectedNode != null)
            item = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(mesh, GetNodes[0].ConnectedNode.id);
        else
            item = null;

        //Debug.Log("first Call material count:"+item.material.Count);

        WallPartItem item2 = new WallPartItem();
        if (GetNodes[1].ConnectedNode != null)
            item2 = (WallPartItem)GetNodes[1].ConnectedNode.AttachedFunctionItem.myFunction(mesh, GetNodes[1].ConnectedNode.id);
        else
            item2 = null;

        //Debug.Log("first Call material count item2:"+item2.material.Count);

        if (item == null && item2 != null)
        {
            Debug.Log("Just Node 2 !!!");
            return item2;
        }
        else if(item2 == null && item != null)
        {
            Debug.Log("Just Node 1 !!!");
            return item;
        }
        else if(item2 != null && item != null)
        {
            Debug.Log(item.material.Count);
            Debug.Log(item2.material.Count);
            Debug.Log("2 nodes !!!");
            return CombineTwoItem(item.mesh,item2.mesh,item.material,item2.material);
        }
        else
        {
            Debug.Log("No Node!!!");
            return mesh;
        }
    }

    public WallPartItem CombineTwoItem(Mesh mesh1, Mesh mesh2, List<Material> materials1, List<Material> materials2)
    {
       
        int submeshCount1 = mesh1.subMeshCount;
        int submeshCount2 = mesh2.subMeshCount;

        Debug.Log("sub1="+submeshCount1 + "--- sub2=" + submeshCount2);

        CombineInstance[] combineInstances1 = new CombineInstance[submeshCount1+ submeshCount2];
        //CombineInstance[] combineInstances2 = new CombineInstance[submeshCount2];

        for (int i = 0; i < submeshCount1; i++)
        {
            combineInstances1[i] = new CombineInstance();
            combineInstances1[i].mesh = mesh1;
            combineInstances1[i].transform = Matrix4x4.identity;
            combineInstances1[i].subMeshIndex = i;
        }

        // Populate the arrays with data from the second mesh
        for (int i = submeshCount1; i < submeshCount2 + submeshCount1; i++)
        {
            combineInstances1[i] = new CombineInstance();
            combineInstances1[i].mesh = mesh2;
            combineInstances1[i].transform = Matrix4x4.identity;
            combineInstances1[i].subMeshIndex = i - submeshCount1;
        }

        //CombineInstance[] combineInstances3 = new CombineInstance[submeshCount1+ submeshCount2];

        Mesh combinedMesh = new Mesh();

        combinedMesh.CombineMeshes(combineInstances1,false,false);
        //combinedMesh.CombineMeshes(combineInstances2,false,false);

        /*combine[0].mesh = mesh1;
        combine[0].transform = Matrix4x4.identity;
        combine[0].subMeshIndex = 0;

        combine[1].mesh = mesh2;
        combine[1].transform = Matrix4x4.identity;
        combine[1].subMeshIndex = 0;

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, false, false);*/

        List<Material> combinedMaterials = new List<Material>();

        foreach (Material material in materials1)
        {
            combinedMaterials.Add(material);
        }
        foreach (Material material in materials2)
        {
            combinedMaterials.Add(material);
        }
        //combinedMaterials.AddRange(materials2);

        WallPartItem item = new WallPartItem();
        item.mesh = combinedMesh;
        item.material = combinedMaterials;

        return item;
    }

}
