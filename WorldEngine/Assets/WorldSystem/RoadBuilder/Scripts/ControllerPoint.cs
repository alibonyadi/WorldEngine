using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ControllerPoint : MonoBehaviour
{
    [SerializeField]
    private int id = -1;
    [SerializeField]
    private Color color = Color.red;
    [SerializeField]
    private float radius = 0.3f;
    [SerializeField]
    private PointManager pointManager;
    private Vector3 tempPos;
    private bool rotationLocked=false;
    private Quaternion mRotation;
    [SerializeField]
    private bool isInmove = false;
    [SerializeField]
    private bool isFirstTime = true;
    [SerializeField]
    private bool isOnConfluenceEgde = false;
    [SerializeField]
    private bool isOnConfluence = false;
    public void SetColor(Color color) => this.color = color;
    public void SetIsPointOnConfluence(bool b) => isOnConfluence = b;
    public void SetIsPointOnConfluenceEdge(bool b) => isOnConfluenceEgde = b;
    public bool IsOnConfluenceEdge() => isOnConfluenceEgde;
    public bool IsOnConfluence() => isOnConfluence;
    public Quaternion GetRotationTemp() => mRotation;
    public void SetRotationTemp()
    {
        rotationLocked = true;
        mRotation = transform.rotation;
    }
    public void UnlockRotation() => rotationLocked = false;
    public bool IsRotationLocked() => rotationLocked;
    public void ResetForConfluence()
    {
        isOnConfluenceEgde = false;
        isOnConfluence=false;
    }
    public void ResetRotation()
    {
        if(pointManager.GetControllerPoints().Count>id+1)
            transform.LookAt(pointManager.GetControllerPoints()[id+1].transform);
        else if(id > 0)
            transform.rotation = pointManager.GetControllerPoints()[id - 1].transform.rotation;
        Debug.Log("point "+id+" rotation Reseted!!!");
    }
    public Color GetColor() => color;
    public void SetRadius(float radius) => this.radius = radius;
    public float GetRadius() => radius;
    public ControllerPoint GetPrePoint()
    {
        if(id>0)
            return pointManager.GetControllerPoints()[id - 1];
        else
            return null;    
    }
    public ControllerPoint GetPostPoint()
    {
        if(id < pointManager.GetControllerPoints().Count-1)
            return pointManager.GetControllerPoints()[id + 1];
        else
            return null;
    }
    public void SetController(PointManager lp) => pointManager = lp;
    public PointManager GetManager() => pointManager;
    public void SetID(int id) => this.id = id;
    public int GetID() => id;
    // Start is called before the first frame update
    void Start()
    {
        tempPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (tempPos != transform.position)
        {
            Changed();
            isInmove = true;
        }
    }
    public void Changed()
    {
        if(pointManager!=null)
            pointManager.SetInMove(id);
    }
    public void MoveFinished()
    {
        if (pointManager != null)
            pointManager.MoveFinished();
        //Debug.Log("PointController Move Finished!");
    }
    private void OnDrawGizmos()
    {
        if (!isFirstTime && tempPos != transform.position)
        {
            Changed();
            if(Selection.activeTransform == transform)
                isInmove = true;
        }
        isFirstTime = false;
        if(isInmove && (Selection.activeTransform != transform || Selection.count < 1))
        {
            isInmove  = false;
            MoveFinished();
        }
        /*else if (isInmove)
        {
            isInmove = false;
            MoveFinished();
        }*/
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
        /*if (Selection.activeTransform == transform )
        {
            
        }*/
        tempPos = transform.position;
    }
}