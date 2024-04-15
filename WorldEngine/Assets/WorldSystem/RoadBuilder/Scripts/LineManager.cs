using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LineManager : MonoBehaviour
{
    [SerializeField]
    public List<Line> Lines = new List<Line>();
    [SerializeField]
    private StreetController streetController;

    private List<Vector3> tempFrontVertices = new List<Vector3>();

    public void CreateLine(ControllerPoint startPoint, ControllerPoint endPoint)
    {
        //Line line = new Line(startPoint, endPoint);
        if(startPoint.GetComponent<Line>()==null)
            startPoint.gameObject.AddComponent<Line>();

        startPoint.GetComponent<Line>().SetPoints(startPoint, endPoint);
        startPoint.GetComponent<Line>().SetWidth(streetController.GetStreetWidth());
        startPoint.GetComponent<Line>().SetManager(this);
        if(startPoint.gameObject.GetComponent<PruceduralRoad>() == null)
            startPoint.gameObject.AddComponent<PruceduralRoad>();
        Lines.Add(startPoint.GetComponent<Line>());

        streetController.GetSideWalkManager().CreateSideWalkObjects(startPoint.GetComponent<Line>());
    }



    public void ClearAll()
    {
        Lines.Clear();
    }

    public void SetPrucedure(UnityEngine.Object p)
    {
        for (int i = 0; i < Lines.Count; i++)
        {
            if(Lines[i].GetComponent<PruceduralRoad>())
                Lines[i].GetComponent<PruceduralRoad>().SetProcedure(p);
        }
    }

    public void GeneratePrucedure()
    {
        for (int i = 0; i < Lines.Count; i++)
        {
            if (Lines[i].GetComponent<PruceduralRoad>())
                Lines[i].GetComponent<PruceduralRoad>().GenerateMeshes();
        }
    }

    public void SetStreetLinesWidth(float w)
    {
        for(int i=0;i< Lines.Count;i++)
            Lines[i].SetWidth(w);
    }

    public void reCreateLines()
    {
        List<ControllerPoint> points = streetController.GetPointManager().GetControllerPoints();
        Lines.Clear();
        streetController.GetSideWalkManager().ClearLists();
        for (int i = 0; i < points.Count; i++)
        {
            if (points.Count > 1 && i < points.Count-1)
                CreateLine(points[i], points[i+1]);
        }
    }

    public StreetController GetController()=> streetController;

    public void SetTempFrontVertices(Vector3 v1, Vector3 v2)
    {
        tempFrontVertices.Clear();
        tempFrontVertices.Add(v1);
        tempFrontVertices.Add(v2);
    }

    public List<Vector3> GetTempFrontVertices() => tempFrontVertices;

    public void SetStreetController(StreetController street) => streetController = street;

    public void Draw()
    {
        foreach (Line line in Lines)
        {
            line.Draw();
        }
    }
}
