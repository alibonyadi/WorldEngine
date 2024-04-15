using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfluenceSideWalkHelper
{
    [SerializeField]
    private GameObject myObj = null;
    [SerializeField]
    private List<Vector3> startVerices = new List<Vector3>();
    [SerializeField] 
    private List<Vector3> endVertices = new List<Vector3>();
    [SerializeField] 
    private ControllerPoint mainPoint = null;
    [SerializeField] 
    private ControllerPoint otherPoint = null;
 

    public GameObject GetObject() => myObj;
    public void SetObject(GameObject obj) => myObj = obj;
    public List<Vector3> GetStartVertices() => startVerices;
    public List<Vector3> GetEndvertices() => endVertices;
    public void SetStartVertices(List<Vector3> V) => startVerices = V;
    public void SetEndVertices(List<Vector3> V) => endVertices = V;
    public void SetPoint(ControllerPoint cp)=> mainPoint = cp;
    public void SetOtherPoint(ControllerPoint cp) => otherPoint = cp;
    public ControllerPoint GetMainPoint() => mainPoint;
    public ControllerPoint GetOtherPoint() => otherPoint;

    public bool CheckEquality(ControllerPoint point,ControllerPoint other)
    {
        bool isEqual = false;
        if((point == mainPoint && other == otherPoint) || (other == mainPoint && point == otherPoint))
            isEqual = true;

        return isEqual;
    }
}
