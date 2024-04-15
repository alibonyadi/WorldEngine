using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    [SerializeField]
    private List<ControllerPoint> controllerPoints = new List<ControllerPoint>();
    [SerializeField]
    private StreetController streetController;

    [SerializeField]
    private int inMoveId = 0;

    public List<ControllerPoint> GetControllerPoints()
    {
        return controllerPoints;
    }

    public void SetStreetController(StreetController street)
    {
        streetController = street;
    } 

    public void ClearAll()
    {
        controllerPoints.Clear();
    }

    public void SetInMove(int id)
    {
        inMoveId = id;
        

        if(id < controllerPoints.Count-1 && !controllerPoints[id].IsRotationLocked())
            controllerPoints[id].transform.LookAt(controllerPoints[id + 1].transform);

        if (controllerPoints.Count > 1 && !controllerPoints[controllerPoints.Count - 1].IsRotationLocked())
            controllerPoints[controllerPoints.Count - 1].transform.rotation = controllerPoints[controllerPoints.Count - 2].transform.rotation;

        if (id > 0 && controllerPoints[id - 1].IsRotationLocked())
            controllerPoints[id - 1].transform.LookAt(controllerPoints[id].transform);

        //Debug.Log("Some Rotation change!!!");

        streetController.PointMoved();
    }

    public void MoveFinished()
    {
        streetController.MoveFinished();

        //Debug.Log("PointManager Get Finish move!");
    }

    public void ResetPointsForConfluence()
    {
        SetAllRotations();
        //Debug.Log("RotationsReseted!!!");
        for (int i = 0; i < controllerPoints.Count; i++)
        {   
            controllerPoints[i].ResetForConfluence();
        }
    }

    public void SetAllRotations()
    {
        for (int i = 0; i < controllerPoints.Count; i++)
        {
            if (i < controllerPoints.Count - 1 && !controllerPoints[i].IsRotationLocked())
                controllerPoints[i].transform.LookAt(controllerPoints[i + 1].transform);
        }
    }

    public StreetController GetController()
    {
        return streetController;
    }

    public ControllerPoint GetInMovePoint()
    {
        return controllerPoints[inMoveId];
    }

    public void AddPoint(ControllerPoint c)
    {
        ControllerPoint point = c;
        point.SetController(this);
        point.SetID(controllerPoints.Count);
        controllerPoints.Add(point);
        if(controllerPoints.Count>1)
            streetController.GetLineManager().CreateLine(controllerPoints[controllerPoints.Count - 2], controllerPoints[controllerPoints.Count-1]);
    }

    public void AddPointOnIndex(ControllerPoint c, int index)
    {
        ControllerPoint point = c;
        point.SetController(this);
        point.SetID(index);
        controllerPoints.Insert(index,point);
        SetAllPointIDs();
        if (controllerPoints.Count > 1)
            streetController.GetLineManager().reCreateLines();
    }

    public void SetAllPointIDs()
    {
        for(int i=0; i< controllerPoints.Count;i++)
        {
            controllerPoints[i].SetID(i);
        }
    }

    public void CreatePointOnIndex(int index)
    {
        GameObject go = new GameObject("Point");
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        if (controllerPoints.Count > index + 1 || index==0)
            go.transform.LookAt(controllerPoints[index + 1].transform.position);
        else
            go.transform.rotation = controllerPoints[index-1].transform.rotation;
        ControllerPoint cp = go.AddComponent<ControllerPoint>();
        cp.SetColor(Color.red);
        cp.SetRadius(0.3f);
        AddPointOnIndex(cp,index);
    }

    public void RemovePoint(int id)
    {
        DestroyImmediate(controllerPoints[id].gameObject);
        controllerPoints.RemoveAt(id);
        streetController.recreateLines();
    }
}
