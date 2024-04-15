using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddLinePoint : MonoBehaviour
{
    [SerializeField]
    private bool canGeneratePoints = false;

    [SerializeField] 
    private PointManager pointManager;

    public ControllerPoint GeneratePoint()
    {
        if (pointManager == null)
            pointManager = GetComponent<StreetController>().GetPointManager();
        GameObject go = new GameObject("Point");
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        ControllerPoint cp = go.AddComponent<ControllerPoint>();
        cp.SetColor(Color.red);
        cp.SetRadius(0.3f);
        pointManager.AddPoint(cp);
        return cp;
    }

    private void OnDrawGizmos()
    {
        if (canGeneratePoints)
        {
            canGeneratePoints = false;
            GeneratePoint();
        }
    }
}
