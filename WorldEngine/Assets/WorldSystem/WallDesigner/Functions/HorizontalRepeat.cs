using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
using WallDesigner;

public class HorizontalRepeat : FunctionItem, IFunctionItem
{
    private WallPartItem Base;
    private WallPartItem RepeatPart;
    private float inBetween = 1;
    private float fromLeft = 1;
    private float fromRight = 1;

    public HorizontalRepeat(int gets, int gives)
    {
        Init();
        Name = "Horrizontal Repeat";
        Base = new WallPartItem();
        RepeatPart = new WallPartItem();
        ClassName = typeof(HorizontalRepeat).FullName;
        basecolor = Color.white;
        myFunction = Execute;

        GetNode gnode = new GetNode();
        gnode.AttachedFunctionItem = this;
        GetNodes.Add(gnode);

        GetNode gnode2 = new GetNode();
        gnode2.id = 1;
        gnode2.AttachedFunctionItem = this;
        GetNodes.Add(gnode2);

        GiveNode givenode1 = new GiveNode();
        givenode1.AttachedFunctionItem = this;
        GiveNodes.Add(givenode1);

        /*GiveNode givenode2 = new GiveNode();
        givenode2.AttachedFunctionItem = this;
        givenode2.id = 1;
        GiveNodes.Add(givenode2);*/

        CalculateRect();

        Rect at1Rect = new Rect(position.x, rect.height / 2 + position.y, rect.width, rect.height);

        FloatAttrebute ta1 = new FloatAttrebute(at1Rect);
        ta1.mFloat = inBetween;
        ta1.SetName("Dist");
        attrebutes.Add(ta1);

        Rect at2Rect = new Rect(position.x, rect.height / 2 + position.y + 20, rect.width, rect.height);

        FloatAttrebute fl1 = new FloatAttrebute(at2Rect);
        fl1.mFloat = fromLeft;
        fl1.SetName("Left");
        attrebutes.Add(fl1);

        Rect at3Rect = new Rect(position.x, rect.height / 2 + position.y + 40, rect.width, rect.height);

        FloatAttrebute fl2 = new FloatAttrebute(at3Rect);
        fl2.mFloat = fromRight;
        fl2.SetName("Right");
        attrebutes.Add(fl2);
    }

    public override void LoadNodeConnections(SerializedFunctionItem item, List<FunctionItem> functionItems)
    {
        if (item.getnodeConnectedFI.Count > 0)
            GetNodes[0].ConnectedNode = functionItems[item.getnodeConnectedFI[0]].GiveNodes[item.getnodeItems[0]];
        if (item.getnodeConnectedFI.Count > 1)
            GetNodes[1].ConnectedNode = functionItems[item.getnodeConnectedFI[1]].GiveNodes[item.getnodeItems[1]];
        if (item.givenodeConnectedFI.Count > 0)
            GiveNodes[0].ConnectedNode = functionItems[item.givenodeConnectedFI[0]].GetNodes[item.givenodeItems[0]];
    }

    public override void LoadSerializedAttributes(SerializedFunctionItem item)
    {
        Name = item.name;
        ClassName = item.ClassName;

        FloatAttrebute att = (FloatAttrebute)attrebutes[0];
        att.mFloat = float.Parse(item.attributeValue[0]);
        attrebutes[0] = att;

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[1];
        att1.mFloat = float.Parse(item.attributeValue[1]);
        attrebutes[1] = att1;

        FloatAttrebute att2 = (FloatAttrebute)attrebutes[2];
        att2.mFloat = float.Parse(item.attributeValue[2]);
        attrebutes[2] = att2;
    }

    public override SerializedFunctionItem SaveSerialize()
    {
        SerializedFunctionItem item = new SerializedFunctionItem();
        item.name = Name;
        item.ClassName = ClassName;
        item.Position = position;
        item.attributeName.Add("FloatAttrebute");

        FloatAttrebute att1 = (FloatAttrebute)attrebutes[0];
        string stringtexturePath = att1.mFloat.ToString();
        item.attributeValue.Add(stringtexturePath);

        FloatAttrebute att2 = (FloatAttrebute)attrebutes[1];
        string stringtexturePath2 = att2.mFloat.ToString();
        item.attributeValue.Add(stringtexturePath2);

        FloatAttrebute att3 = (FloatAttrebute)attrebutes[2];
        string stringtexturePath3 = att3.mFloat.ToString();
        item.attributeValue.Add(stringtexturePath3);

        if (GetNodes[0].ConnectedNode != null)
        {
            int connectedGetNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GetNodes[0].ConnectedNode.AttachedFunctionItem);
            item.getnodeConnectedFI.Add(connectedGetNodeNumber);
            item.getnodeItems.Add(GetNodes[0].ConnectedNode.id);
        }

        if (GetNodes[1].ConnectedNode != null)
        {
            int connectedGetNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GetNodes[1].ConnectedNode.AttachedFunctionItem);
            item.getnodeConnectedFI.Add(connectedGetNodeNumber);
            item.getnodeItems.Add(GetNodes[1].ConnectedNode.id);
        }

        if (GiveNodes[0].ConnectedNode != null)
        {
            int connectedGiveNodeNumber = WallEditorController.Instance.GetAllCreatedItems().IndexOf(GiveNodes[0].ConnectedNode.AttachedFunctionItem);
            item.givenodeConnectedFI.Add(connectedGiveNodeNumber);
            item.givenodeItems.Add(GiveNodes[0].ConnectedNode.id);
        }

        return item;
    }

    public object Execute(object mMesh,object id)
    {
        FloatAttrebute fa1 = (FloatAttrebute)attrebutes[0];
        inBetween = (float)fa1.GetValue();

        FloatAttrebute fa2 = (FloatAttrebute)attrebutes[1];
        fromLeft = (float)fa2.GetValue();

        FloatAttrebute fa3 = (FloatAttrebute)attrebutes[2];
        fromRight = (float)fa3.GetValue();

        List<Material> baseMaterials = new List<Material>();

        

        if (GetNodes[1].ConnectedNode != null)
        {
            RepeatPart = (WallPartItem)GetNodes[1].ConnectedNode.AttachedFunctionItem.myFunction(mMesh, GetNodes[1].ConnectedNode.id);
        }
        else
            RepeatPart = null;

        if (GetNodes[0].ConnectedNode != null)
        {
            Base = (WallPartItem)GetNodes[0].ConnectedNode.AttachedFunctionItem.myFunction(mMesh, GetNodes[0].ConnectedNode.id);
            baseMaterials = Base.material;
        }
        else
            Base = null;


        Mesh baseMesh = Base.mesh;
        Mesh repeatMesh = RepeatPart.mesh;
        List<Material> repeatMaterials = RepeatPart.material;
        

        float BaseMinX = float.MaxValue;
        for(int i=0;i < baseMesh.vertices.Length;i++)
        {
            if (baseMesh.vertices[i].x < BaseMinX)
                BaseMinX = baseMesh.vertices[i].x;
        }

        float RMMinX = float.MaxValue;
        for (int i = 0; i < repeatMesh.vertices.Length; i++)
        {
            if (repeatMesh.vertices[i].x < RMMinX)
                RMMinX = repeatMesh.vertices[i].x;
        }

        float moveToZ = RMMinX - BaseMinX;

        int numberOfRepeats = (int)Mathf.Floor((baseMesh.bounds.size.x-fromLeft-fromRight )/ (repeatMesh.bounds.size.x+inBetween));
        //float distanceBetween = inBetween + (((baseMesh.bounds.size.x - fromLeft - fromRight) % numberOfRepeats)/numberOfRepeats);
        float distanceBetween=0;
        if (numberOfRepeats > 1)
        {
            distanceBetween = (baseMesh.bounds.size.x - fromLeft - fromRight - repeatMesh.bounds.size.x * numberOfRepeats) / (numberOfRepeats - 1);
        } else
        {
            distanceBetween = 0;
        }
        if (numberOfRepeats < 0)
            numberOfRepeats = 0;

        int numberOfSideMeshes = Mathf.Max(numberOfRepeats - 1,1);

        if (fromLeft > 0)
            numberOfSideMeshes++;
        if (fromRight > 0)
            numberOfSideMeshes++;

        CombineInstance[] combineInstances1 = new CombineInstance[numberOfRepeats* repeatMesh.subMeshCount + numberOfSideMeshes];
        List<Material> combinedMaterials = new List<Material>();
        int mIndex = 0;
        for (int i = 0; i < numberOfRepeats; i++)
        {
            /*if (GetNodes[1].ConnectedNode != null)
            {
                WallPartItem RepeatPart = (WallPartItem)GetNodes[1].ConnectedNode.AttachedFunctionItem.myFunction(mMesh, GetNodes[1].ConnectedNode.id);
            }*/


            foreach (Material material in RepeatPart.material)
            {
                combinedMaterials.Add(material);
            }
            Mesh repeatmeshTemp = new Mesh();
            //Vector3[] vertices = RepeatPart.mesh.vertices;
            //int[] teriangles = RepeatPart.mesh.triangles;
            Vector2[] UVs = RepeatPart.mesh.uv;
            Vector3[] normals = RepeatPart.mesh.normals;
            Vector3 movePos=new Vector3();
            movePos = new Vector3( i * (distanceBetween+ repeatMesh.bounds.size.x), 0,0);
            movePos.x -=  /*baseMesh.bounds.size.x/2 - repeatMesh.bounds.size.x/2*/ moveToZ - fromLeft;
            Vector3[] vertices = RepositionMesh(RepeatPart.mesh.vertices, movePos); 
            repeatmeshTemp.vertices = vertices;
            //repeatmeshTemp.triangles = teriangles;
            repeatmeshTemp.uv = UVs;
            repeatmeshTemp.normals = normals;
            repeatmeshTemp.subMeshCount = repeatMesh.subMeshCount;
            int numSubMeshes = RepeatPart.mesh.subMeshCount;
            for (int k = 0; k < numSubMeshes; k++)
            {
                int[] originalTriangles = RepeatPart.mesh.GetTriangles(k);
                repeatmeshTemp.SetTriangles(originalTriangles, k);
            }
            repeatmeshTemp.RecalculateBounds();

            for (int j = 0; j < numSubMeshes; j++)
            {
                combineInstances1[i* repeatmeshTemp.subMeshCount + j] = new CombineInstance();
                combineInstances1[i* repeatmeshTemp.subMeshCount + j].mesh = repeatmeshTemp;
                combineInstances1[i* repeatmeshTemp.subMeshCount + j].transform = Matrix4x4.identity;
                combineInstances1[i* repeatmeshTemp.subMeshCount + j].subMeshIndex = j;
                mIndex = i * repeatmeshTemp.subMeshCount + j;
            }
        }

        mIndex++;
        if (fromLeft > 0)
        {
            Mesh leftmesh = generatePlane(fromLeft, baseMesh.bounds.size.z);
            //Mesh leftmesh = generatePlane(fromLeft, 2);
            Vector3 movePos = new Vector3(0,0,0);
            movePos.x -= baseMesh.bounds.size.x/2- fromLeft/2;
            leftmesh.vertices = RepositionMesh(leftmesh.vertices, movePos);
            combineInstances1[mIndex] = new CombineInstance();
            combineInstances1[mIndex].mesh = leftmesh;
            combineInstances1[mIndex].transform = Matrix4x4.identity;
            combineInstances1[mIndex].subMeshIndex = 0;
            foreach (Material material in baseMaterials)
            {
                combinedMaterials.Add(material);
            }
            //combinedMaterials.Add(Base.material[0]);
            mIndex++;
            numberOfSideMeshes--;
        }

        if(fromRight > 0)
        {
            Mesh rightmesh = generatePlane(fromRight, baseMesh.bounds.size.z);
            Vector3 movePos = new Vector3(0, 0, 0);
            movePos.x += baseMesh.bounds.size.x / 2 - fromRight / 2;
            rightmesh.vertices = RepositionMesh(rightmesh.vertices, movePos);
            combineInstances1[mIndex] = new CombineInstance();
            combineInstances1[mIndex].mesh = rightmesh;
            combineInstances1[mIndex].transform = Matrix4x4.identity;
            combineInstances1[mIndex].subMeshIndex = 0;
            foreach (Material material in baseMaterials)
            {
                combinedMaterials.Add(material);
            }
            //combinedMaterials.Add(baseMaterials[0]);
            mIndex++;
            numberOfSideMeshes--;
        }

        for(int i=0;i< numberOfSideMeshes;i++)
        {
            Mesh meshside = generatePlane(distanceBetween, baseMesh.bounds.size.z);
            Vector3 movePos = new Vector3(0, 0, 0);
            movePos = new Vector3(i * (distanceBetween + repeatMesh.bounds.size.x), 0, 0);
            movePos.x -=  baseMesh.bounds.size.x/2 - fromLeft - repeatMesh.bounds.size.x- distanceBetween/2;
            meshside.vertices = RepositionMesh(meshside.vertices, movePos);
            combineInstances1[mIndex] = new CombineInstance();
            combineInstances1[mIndex].mesh = meshside;
            combineInstances1[mIndex].transform = Matrix4x4.identity;
            combineInstances1[mIndex].subMeshIndex = 0;
            foreach (Material material in baseMaterials)
            {
                combinedMaterials.Add(material);
            }
            //combinedMaterials.Add(baseMaterials[0]);
            mIndex++;
        }


        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstances1, false, false);

        WallPartItem wallPartItem = new WallPartItem();
        wallPartItem.mesh = combinedMesh;
        wallPartItem.material = combinedMaterials;
        return wallPartItem;
    }

    Vector3[] RepositionMesh(Vector3[] vertices,Vector3 movePos)
    {
        for(int i=0;i<vertices.Length;i++)
        {
            vertices[i] += movePos;
        }

        return vertices;
    }

    public static Mesh generatePlane(float width,float height)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(-width / 2, 0, height / 2);
        vertices[1] = new Vector3(width / 2, 0, height / 2);
        vertices[2] = new Vector3(width / 2, 0, -height / 2);
        vertices[3] = new Vector3(-width / 2, 0, -height / 2);
        vertices[4] = new Vector3(-width / 2, 0, -height / 2);
        vertices[5] = new Vector3(width / 2, 0, height / 2);

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(1, 0);
        uv[3] = new Vector2(0, 0);
        uv[4] = new Vector2(0, 0);
        uv[5] = new Vector2(1, 1);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 2;
        triangles[4] = 4;
        triangles[5] = 5;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

}
