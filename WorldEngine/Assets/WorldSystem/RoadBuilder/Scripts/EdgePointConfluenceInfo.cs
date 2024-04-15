using System;
using UnityEngine;

[Serializable]
public class EdgePointConfluenceInfo
{
    [SerializeField]
    private bool isMainStreet;
    [SerializeField]
    private bool hasLeftContact;
    [SerializeField]
    private bool hasRightContact;
    [SerializeField]
    private ControllerPoint myPoint;
    [SerializeField]
    private ControllerPoint leftPoint;
    [SerializeField]
    private ControllerPoint rightPoint;
    [SerializeField]
    private float angle;
    [SerializeField]
    private float moveDistance;
    [SerializeField]
    private bool isPreEdge;
    [SerializeField]
    private bool isRightOfMainStreet;
    [SerializeField]
    private bool isRightSideWalkCalculated;
    [SerializeField]
    private bool isLeftSideWalkCalculated;

    public void SetRightSideWalkCalc(bool b) => isRightSideWalkCalculated = b;
    public bool GetRightSideWalkCalc() => isRightSideWalkCalculated;
    public void SetLeftSideWalkCalc(bool b) => isLeftSideWalkCalculated = b;
    public bool GetLeftSideWalkCalc() => isLeftSideWalkCalculated;
    public void SetLeftContact(ControllerPoint cp)
    {
        if(cp != null)
            hasLeftContact = true;
        else
            hasLeftContact= false;
        leftPoint = cp;
    }
    public void SetRightContact(ControllerPoint cp)
    {
        if(cp != null)
            hasRightContact = true;
        else
            hasRightContact= false;
        rightPoint = cp;
    }
    public void SetMoveDistance(float f)=> moveDistance = f;
    public float GetMoveDistance()=> moveDistance;
    public void SetIsRightOfMainStreet(bool b)=> isRightOfMainStreet = b;
    public bool IsRightOfMainStreet() => isRightOfMainStreet;
    public void SetAngle(float a) => angle = a;
    public float GetAngle() => angle;
    public void SetIsPreEdge(bool b) => isPreEdge = b;
    public bool GetIsPreEdge() => isPreEdge;
    public void SetMainStreet(bool b) => isMainStreet = b;
    public bool GetIsMainStreet() => isMainStreet;
    public void SetPoint(ControllerPoint cp) => myPoint = cp;
    public bool HasLeftContact
    {
        get => hasLeftContact;
        set => hasLeftContact = value;
    }
    public bool HasRightContact
    {
        get => hasRightContact;
        set => hasRightContact = value;
    }
    public ControllerPoint GetPoint() => myPoint;
    public ControllerPoint GetLeft()
    {
        //if(hasLeftContact)
            return leftPoint;

        Debug.LogWarning("No Left Contact Found!!!");
        return null;
    }

    public ControllerPoint GetRight()
    {
        //if (hasRightContact)
            return rightPoint;

        Debug.LogWarning("No Right Contact Found!!!");
        return null;
    }
}