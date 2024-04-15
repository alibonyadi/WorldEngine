using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class MultyRoadConfluence : MonoBehaviour
{
    [SerializeField]
    List<EdgePointConfluenceInfo> edgePoints = new List<EdgePointConfluenceInfo>();
    [SerializeField]
    bool isCalculated = false;
    public void CalculateEdgePointContacts(List<EdgePointConfluenceInfo> CPs)
    {
        CalculateAngles();
        CalculateSideOfMainStreeet();
        isCalculated = true;
        for(int i=0;i<edgePoints.Count;i++)
        {
            FindLeftContact(i);
            FindRightContact(i);
        }
        CalCulateMainStreetMoveDistance();
    }
    private void CalCulateMainStreetMoveDistance()
    {
        for(int i=0;i<edgePoints.Count;i++)
        {
            if (edgePoints[i].GetIsMainStreet())
            {
                CalculateDistance(i);
            }
        }
    }
    private void CalculateDistance(int i)
    {
        //Debug.Log("Distance index = " + i);
        bool isPre = edgePoints[i].GetIsPreEdge();
        //Debug.Log(edgePoints[i].GetPoint()+"--i = "+ i +" ------ counfluence = "+name);
        float mindist = 0;
        if (edgePoints[i].GetPoint().GetComponent<Line>() == null)
            mindist = edgePoints[i].GetPoint().GetManager().GetController().GetStreetWidth() / 2;
        else
            mindist = edgePoints[i].GetPoint().GetComponent<Line>().GetWidth() / 2;
        float distanceLeft = mindist;
        float distanceRight = mindist;
        int index = i;
        while (edgePoints[index].HasLeftContact)
        {
            index = FindEdgePoint(edgePoints[index].GetLeft());
            //Debug.Log("left distance number = " + index);
            float angle = edgePoints[index].GetAngle();
            float width = 0;
            if (edgePoints[index].GetIsPreEdge() && edgePoints[index].GetPoint().GetPrePoint() != null)
                width = edgePoints[index].GetPoint().GetPrePoint().GetComponent<Line>().GetWidth();
            //else if (!edgePoints[index].GetIsPreEdge() && edgePoints[index].GetPoint().GetPostPoint() !=null)
            //    width = edgePoints[index].GetPoint().GetPostPoint().GetComponent<Line>().GetWidth();
            else
                width = edgePoints[index].GetPoint().GetComponent<Line>().GetWidth();
            distanceLeft += Math.Abs((width/90) * angle);
            //Debug.Log("leftDistance = " + distanceLeft);
        }

        index = i;
        while (edgePoints[index].HasRightContact)
        {
            index = FindEdgePoint(edgePoints[index].GetRight());
            //Debug.Log("right distance number = " + index);
            float angle = edgePoints[index].GetAngle();
            float width = 0;
            if (edgePoints[index].GetIsPreEdge() && edgePoints[index].GetPoint().GetPrePoint() != null)
                width = edgePoints[index].GetPoint().GetPrePoint().GetComponent<Line>().GetWidth();
            //else if (!edgePoints[index].GetIsPreEdge() && edgePoints[index].GetPoint().GetPostPoint() != null)
            //    width = edgePoints[index].GetPoint().GetPostPoint().GetComponent<Line>().GetWidth();
            else
                width = edgePoints[index].GetPoint().GetComponent<Line>().GetWidth();
            distanceRight += Math.Abs((width / 90) * angle);
            //Debug.Log("rightDistance = " + distanceRight);
        }

        float result = Mathf.Max(Mathf.Abs(distanceLeft), Mathf.Abs(distanceRight));
        //Debug.Log("Distance index "+i+" = "+result);
        edgePoints[i].SetMoveDistance(result);
    }
    private void FindLeftContact(int index)//Big problem On multyRoad --> must check min Angle bigger than self
    {
        List<EdgePointConfluenceInfo> leftEdges = new List<EdgePointConfluenceInfo>();
        for(int i=0;i<edgePoints.Count;i++)
        {
            if (edgePoints[index].GetIsMainStreet() && i == index)
                continue;

            if (edgePoints[index].GetIsMainStreet())
            {
                if (!edgePoints[i].GetIsMainStreet() && !edgePoints[i].IsRightOfMainStreet()  && edgePoints[index].GetIsPreEdge()) 
                {
                    leftEdges.Add(edgePoints[i]);
                }
                else if (!edgePoints[i].GetIsMainStreet() && edgePoints[i].IsRightOfMainStreet() && !edgePoints[index].GetIsPreEdge())
                {
                    leftEdges.Add(edgePoints[i]);
                }
            }
            else
            {
                if (!edgePoints[i].GetIsMainStreet() && edgePoints[index].IsRightOfMainStreet() == edgePoints[i].IsRightOfMainStreet())// && ((!edgePoints[i].IsRightOfMainStreet() && edgePoints[i].GetAngle() > 0) || (edgePoints[i].IsRightOfMainStreet() && edgePoints[i].GetAngle() < 0)))
                {
                    leftEdges.Add(edgePoints[i]);
                }
            }
        }

        if (leftEdges.Count < 1)
            return;

        float minAngle = 1000;
        int minAngleIndex = 0;
        for(int i=0;i<leftEdges.Count;i++)
        {
            if (leftEdges[i].GetAngle()<minAngle)
            {
                minAngleIndex = i;
                minAngle = leftEdges[i].GetAngle();
            }
        }

        if (edgePoints[index].GetIsMainStreet() && !edgePoints[index].GetIsPreEdge())
        {
            if (leftEdges[minAngleIndex].GetAngle() <= 0)
            {
                edgePoints[index].SetLeftContact(leftEdges[minAngleIndex].GetPoint());
                edgePoints[index].HasLeftContact = false;
                return;
            }
        }
        else if (edgePoints[index].GetIsMainStreet() && edgePoints[index].GetIsPreEdge())
        {
            if (leftEdges[minAngleIndex].GetAngle() > 0)
            {
                edgePoints[index].SetLeftContact(leftEdges[minAngleIndex].GetPoint());
                edgePoints[index].HasLeftContact = false;
                return;
            }
        }

        if (!edgePoints[index].GetIsMainStreet() && minAngle == edgePoints[index].GetAngle())
        {

            if (edgePoints[index].IsRightOfMainStreet() && minAngle < 0)
            {
                edgePoints[index].SetLeftContact(edgePoints[0].GetPoint());
                //Debug.Log("left point of edge point " + index + " setted : 0");
            }
            else if (!edgePoints[index].IsRightOfMainStreet()&& minAngle >0)
            {
                edgePoints[index].SetLeftContact(edgePoints[1].GetPoint());
                //Debug.Log("left point of edge point " + index + " setted : 1");
            }
            else if (edgePoints[index].IsRightOfMainStreet())
            {
                edgePoints[index].SetLeftContact(edgePoints[0].GetPoint());
                if(minAngle == 0)
                    edgePoints[index].HasLeftContact=true;
                else
                    edgePoints[index].HasLeftContact=false;
            }
            else
            {
                edgePoints[index].SetLeftContact(edgePoints[1].GetPoint());
                if(minAngle == 0)
                    edgePoints[index].HasLeftContact = true;
                else
                    edgePoints[index].HasLeftContact = false;
            }
            return;
        }
        //Debug.Log("Left point of index "+index+" Setted as :"+ minAngleIndex);
        edgePoints[index].SetLeftContact(leftEdges[minAngleIndex].GetPoint());
    }
    private void FindRightContact(int index)
    {
        List<EdgePointConfluenceInfo> rightEdges = new List<EdgePointConfluenceInfo>();
        for (int i = 0; i < edgePoints.Count; i++)
        {
            if (edgePoints[index].GetIsMainStreet() && i == index)
                continue;

            if (edgePoints[index].GetIsMainStreet())
            {
                if (!edgePoints[i].GetIsMainStreet() && edgePoints[i].IsRightOfMainStreet() && edgePoints[index].GetIsPreEdge())
                {
                    rightEdges.Add(edgePoints[i]);
                }
                else if (!edgePoints[i].GetIsMainStreet() && !edgePoints[i].IsRightOfMainStreet() && !edgePoints[index].GetIsPreEdge())
                {
                    rightEdges.Add(edgePoints[i]);
                }
            }
            else
            {
                if (!edgePoints[i].GetIsMainStreet() && edgePoints[index].IsRightOfMainStreet() == edgePoints[i].IsRightOfMainStreet())//must reCheck this line
                {
                    rightEdges.Add(edgePoints[i]);
                }
            }
        }

        if (rightEdges.Count < 1)
            return;

        float maxAngle = -2000;
        int maxAngleIndex = 0;
        for (int i = 0; i < rightEdges.Count; i++)
        {
            if (rightEdges[i].GetAngle() > maxAngle)
            {
                maxAngleIndex = i;
                maxAngle = rightEdges[i].GetAngle();
            }
        }

        if (edgePoints[index].GetIsMainStreet() && !edgePoints[index].GetIsPreEdge())
        {
            if (rightEdges[maxAngleIndex].GetAngle() <= 0)
            {
                edgePoints[index].SetRightContact(rightEdges[maxAngleIndex].GetPoint());
                edgePoints[index].HasRightContact = false;
                return;
            }
                
        }
        else if (edgePoints[index].GetIsMainStreet() && edgePoints[index].GetIsPreEdge())
        {
            if (rightEdges[maxAngleIndex].GetAngle() > 0)
            {
                edgePoints[index].SetRightContact(rightEdges[maxAngleIndex].GetPoint());
                edgePoints[index].HasRightContact = false;
                return;
            }
                
        }


        if (!edgePoints[index].GetIsMainStreet() && maxAngle == edgePoints[index].GetAngle())
        {
            if (edgePoints[index].IsRightOfMainStreet() && maxAngle > 0)
                edgePoints[index].SetRightContact(edgePoints[1].GetPoint());
            else if (!edgePoints[index].IsRightOfMainStreet() && maxAngle < 0)
                edgePoints[index].SetRightContact(edgePoints[0].GetPoint());
            else if(edgePoints[index].IsRightOfMainStreet())
            {
                edgePoints[index].SetRightContact(edgePoints[1].GetPoint());
                if(maxAngle != 0)
                    edgePoints[index].HasRightContact = false;
            }
            else
            {
                edgePoints[index].SetRightContact(edgePoints[0].GetPoint());
                if(maxAngle != 0)
                    edgePoints[index].HasRightContact = false;

            }
            return;
        }

        edgePoints[index].SetRightContact(rightEdges[maxAngleIndex].GetPoint());
    }
    private void CalculateAngles()
    {
        for (int i = 0; i < edgePoints.Count; i++)
        {
            if (edgePoints[i].GetIsMainStreet())
                continue;

            if (edgePoints[i].GetIsPreEdge())
            {
                Quaternion rot = Quaternion.LookRotation(transform.position - edgePoints[i].GetPoint().GetPrePoint().transform.position, Vector3.up);
                float angle = (Quaternion.Angle(rot, transform.rotation) - 90);
                edgePoints[i].SetAngle(angle);
            }
            else
            {
                if (edgePoints[i].GetPoint().GetPostPoint() != null)
                {
                    Quaternion rot = Quaternion.LookRotation(transform.position - edgePoints[i].GetPoint().GetPostPoint().transform.position, Vector3.up);
                    float angle = (Quaternion.Angle(rot, transform.rotation) - 90);
                    //Debug.Log(angle);
                    edgePoints[i].SetAngle(angle);
                }
                else
                {
                    float angle = 90;
                    edgePoints[i].SetAngle(angle);
                }
            }
        }
    }
    private void CalculateSideOfMainStreeet()
    {   
        for(int i=0;i<edgePoints.Count;i++)
        {
            if (edgePoints[i].GetIsMainStreet())
                continue;

            bool isRightOfMainRoad = transform.InverseTransformPoint(edgePoints[i].GetPoint().transform.position).x > 0;

            /*if (edgePoints[i].GetIsPreEdge())
            {
                if (edgePoints[i].GetPoint().GetPrePoint()!=null)
                {
                    bool isRightOfMainRoad = transform.InverseTransformPoint(edgePoints[i].GetPoint().GetPrePoint().transform.position).x > 0;
                }
                else
                {
                    bool isRightOfMainRoad = transform.InverseTransformPoint(edgePoints[i].GetPoint().transform.position).x > 0;

                }
            }
            else
            {
                if (edgePoints[i].GetPoint().GetPostPoint()!=null)
                {
                    bool isRightOfMainRoad = transform.InverseTransformPoint(edgePoints[i].GetPoint().GetPostPoint().transform.position).x > 0;
                }
                else
                {
                    bool isRightOfMainRoad = transform.InverseTransformPoint(edgePoints[i].GetPoint().transform.position).x > 0;
                }
            }*/

            edgePoints[i].SetIsRightOfMainStreet(isRightOfMainRoad);

        }
    }
    public void AddEdgeInfo(EdgePointConfluenceInfo edgeInfo)
    {
        edgePoints.Add(edgeInfo);
    }
    public void Reset()
    {
        edgePoints.Clear();
        isCalculated = false;
    }
    public int FindEdgePoint(ControllerPoint cp)
    {
        if(edgePoints.Count < 1)
            return -1;
        for(int i=0;i<edgePoints.Count;i++)
        {
            if (edgePoints[i].GetPoint() == cp)
                return i;
        }
        return -1;
    }
    public EdgePointConfluenceInfo GetEdgeInfo(ControllerPoint cp)
    {
        if (isCalculated)
        {
            //CalculateEdgePointContacts(edgePoints);
            //Debug.Log("street = "+cp.GetManager().GetController()+" point id = "+cp.GetID());
            int index = FindEdgePoint(cp);
            
            return edgePoints[index];
        }
        else
        {
            //Debug.Log(cp.GetManager().GetController().name + " --- point " + cp.GetID());
            CalculateEdgePointContacts(edgePoints);

            int index = FindEdgePoint(cp);
            return edgePoints[index];
        }
    }
}
