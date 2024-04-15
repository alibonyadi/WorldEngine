using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Lot:MonoBehaviour
{
    [SerializeField]
    private List<Vector3> points = new List<Vector3>();
    [SerializeField]
    private List<ConfluenceController> confluences = new List<ConfluenceController>();
    [SerializeField]
    private float height = 0.3f;
    public void SetPoints(List<Vector3> p) => points = p;
    public List<Vector3> GetPoints() => points;
    public void SetConfluences(List<ConfluenceController> CCs) => confluences = CCs;
    public List<ConfluenceController> GetConfluences() => confluences;
    public void SetHeight(float h) => height = h;
    public bool CheckIsEquality(Lot h2)
    {
        int matchCount = 0;
        if (confluences.Count != h2.GetConfluences().Count)
            return false;

        for (int i = 0; i < confluences.Count; i++)
        {
            for (int j = 0; j < h2.GetConfluences().Count; j++)
            {
                if (confluences[i] == h2.GetConfluences()[j])
                {
                    matchCount++;
                }
            }
        }

        if (matchCount >= confluences.Count)
            return true;
        else
            return false;
    }

    public void SetPointsHeight(float height)
    {
        for(int i=0;i<points.Count;i++)
        {
            points[i] += new Vector3(0, height,0);
        }
    }
    public void GenerateBaseMesh()
    {
        SetPointsHeight(height);
        if (GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        if(GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();
        if(GetComponent<PruceduralRoad>()==null)
            gameObject.AddComponent<PruceduralRoad>();
        //Debug.Log
        Mesh mesh = MeshUtility.GenerateFlatMeshOnVertices(MeshUtility.WeldVertices(points,0.1f));
        //Mesh mesh = MeshUtility.GenerateFlatMeshOnVertices(points);

        GetComponent<MeshFilter>().mesh = mesh;
    }
}

public class LotPoint : MonoBehaviour
{
    [SerializeField]
    private Vector3 position;
}