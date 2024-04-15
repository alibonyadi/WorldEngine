using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfluenceSideWalkCrossHelper
{
    [SerializeField]
    private GameObject myObj = null;
    [SerializeField]
    private GameObject junctionObj = null;
    [SerializeField]
    private List<Vector3> crossVerices = new List<Vector3>();
    [SerializeField]
    private List<Vector3> junctionVerices = new List<Vector3>();
    [SerializeField]
    private ControllerPoint mainPoint = null;
    [SerializeField]
    private ControllerPoint otherPoint = null;
    [SerializeField]
    private bool isDefault = false;

    public GameObject GetObject() => myObj;
    public void SetObject(GameObject obj) => myObj = obj;
    public void SetJunctionObject(GameObject obj) => junctionObj = obj;
    public GameObject GetJunctionObject() => junctionObj;
    public List<Vector3> GetCrossVertices() => crossVerices;
    public List<Vector3> GetJunctionvertices() => junctionVerices;
    public void SetCrossVertices(List<Vector3> V) => crossVerices = V;
    public void SetJunctionVertices(List<Vector3> V) => junctionVerices = V;
    public void SetPoint(ControllerPoint cp) => mainPoint = cp;
    public void SetOtherPoint(ControllerPoint cp) => otherPoint = cp;
    public void SetIsDefault(bool b)=> isDefault = b;
    public ControllerPoint GetMainPoint() => mainPoint;
    public ControllerPoint GetOtherPoint() => otherPoint;
    public bool GetIsDefault() => isDefault;
}
