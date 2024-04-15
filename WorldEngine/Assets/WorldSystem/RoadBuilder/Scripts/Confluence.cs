using System;
using UnityEngine;

[Serializable]
public class Confluence
{
    [SerializeField]
    private ConfluenceController controller;
    [SerializeField]
    private bool isContactOnPoint = false;
    [SerializeField]
    private ControllerPoint confluencePoint;// this street contact point
    [SerializeField]
    private ControllerPoint otherConfluencePoint;// Other street contact point
    [SerializeField]
    private int PointerId = 0;
    [SerializeField]
    private ControllerPoint preConfluencePoint = null;
    [SerializeField]
    private ControllerPoint postConfluencePoint = null;
    //private int PointerId=0;
    public Confluence(ControllerPoint cp1, ControllerPoint cpOther,bool isPointOnContact,ConfluenceController cc)
    {
        confluencePoint = cp1;
        otherConfluencePoint = cpOther;
        isContactOnPoint=isPointOnContact;
        controller = cc;
    }
    public bool CheckEquals(Confluence c)
    {
        if((c.GetPoint()==this.GetPoint() && c.GetOtherPoint()==this.GetOtherPoint()) || (c.GetPoint()==this.GetOtherPoint()&&c.GetOtherPoint()==this.GetPoint()))
            return true;

        return false;
    }
    public ControllerPoint GetPoint() => confluencePoint;
    public void SetPrePoint(ControllerPoint cp) => preConfluencePoint = cp;
    public void SetPostPoint(ControllerPoint cp) => postConfluencePoint = cp;
    public ControllerPoint GetPrePoint() => preConfluencePoint;
    public ControllerPoint GetPostPoint() => postConfluencePoint;
    public ConfluenceController GetController() => controller;
    public void SetController(ConfluenceController cc)=> controller = cc;
    public bool GetIsOnPoint() => isContactOnPoint;
    public bool ContainPoint(ControllerPoint cp)
    {
            return (confluencePoint == cp || otherConfluencePoint == cp);
    }
    public ControllerPoint GetOtherPoint() => otherConfluencePoint;
    public bool isStillContacting() => Vector3.Distance(confluencePoint.transform.position, otherConfluencePoint.transform.position) < 1.5f;
    public bool isStillContacting(float treshold) => Vector3.Distance(confluencePoint.transform.position, otherConfluencePoint.transform.position) < treshold;
}