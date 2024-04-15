using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class StreetController : MonoBehaviour
{
    [SerializeField]
    private string streetName = string.Empty;
    [SerializeField]
    private float streetWidth = 7;
    [SerializeField]
    private bool haveLeftSideWalk = true;
    [SerializeField]
    private float leftSideWalkWidth = 2.5f;
    [SerializeField]
    private bool haveRightSideWalk = true;
    [SerializeField]
    private float rightSideWalkWidth = 2.5f;
    private bool streetWidthChanged = false;
    private bool leftSidewalkWidthChanged = false;
    private bool rightSidewalkWidthChanged = false;
    [SerializeField]
    private StreetNetworkManager manager;
    [SerializeField]
    private List<Confluence> confluences=new List<Confluence>(); 
    [SerializeField]
    private bool initialize = false;
    [SerializeField]
    private PointManager pointManager;
    [SerializeField]
    private LineManager lineManager;
    [SerializeField]
    private SideWalkManager sideWalkManager;
    [SerializeField]
    private bool generate = false;
    [SerializeField]
    private bool clearAll = false;
    [SerializeField]
    private bool clearMesh = false;
    [SerializeField]
    private UnityEngine.Object roadProcedure;
    [SerializeField]
    private UnityEngine.Object sideWalkprocedure;
    // Update is called once per frame
    void Update()
    {
        if(generate)
        {
            generate = false;
        }
    }
    /*public void init(StreetNetworkManager m,float width,float sideWalkwidth,float sidewalkheight)
    {
        manager = m;
        streetWidth = width;
        sidewalkheight = s
    }*/
    public void AddConfluence(Confluence c)
    {
        confluences.Add(c);
    }

    public List<Confluence> GetConfluences() => confluences;
    public Confluence FindConfluenceByController(ConfluenceController cc)
    {
        for(int i=0;i<confluences.Count;i++)
        {
            if (confluences[i].GetController() == cc)
                return confluences[i];
        }
        return null;
    }

    public void RemoveConfluence(Confluence c)
    {
        //Debug.Log("Try to Remove From:" + name);
        for (int i = 0; i < confluences.Count; i++)
        {
            if (confluences[i].CheckEquals(c))
                confluences.Remove(confluences[i]);
        }
    }
    public void ResetPointsForConfluence()=> pointManager.ResetPointsForConfluence();
    public void SetStreetLinesWidth()=> lineManager.SetStreetLinesWidth(streetWidth);
    public void CreateEdgePoints(Confluence c,float distance, bool isMainStreet)
    {
        //Debug.Log("is main street = "+isMainStreet +"  -----  "+ c.GetController().name);
        ControllerPoint point = isMainStreet ? c.GetOtherPoint() : c.GetPoint();
        int index = point.GetID();
        if (isMainStreet && !c.GetIsOnPoint())
        {
            /*if(c.GetController().name == "ConFluencePoint0")
            {
                Debug.Log("ConFluencePoint0 index is "+index+" ----- main point = "+ c.GetController().GetMainPoint());
            }*/
            //
            if (c.GetController().GetMainPoint() == null)
            {
                index++;
                pointManager.CreatePointOnIndex(index);
                //Debug.Log("created point on index "+index);
                c.GetController().SetMainPoint(pointManager.GetControllerPoints()[index]);
                pointManager.GetControllerPoints()[index].transform.position = point.transform.position;
                //pointManager.GetControllerPoints()[index].SetIsPointOnConfluence(true);
            }
            pointManager.GetControllerPoints()[index].SetIsPointOnConfluence(true);
            //c.GetController().GetMainPoint().SetIsPointOnConfluence(true);
        }
        //Debug.Log("Index = "+index);
        if (index > 0)//if there was point before this one
        {
            
            Transform prePoint = pointManager.GetControllerPoints()[index - 1].transform;
            float dist = Vector3.Distance(prePoint.transform.position, point.transform.position);
            //Debug.Log("dist = "+dist+" ---  max distance = "+ distance);//
            if ((!isMainStreet && c.GetPrePoint() != null) || (isMainStreet && c.GetController().GetPrePoint() != null))
            {

            }
            else if (dist < (distance / 2) - 0.1f)
            {
                prePoint.GetComponent<ControllerPoint>().SetIsPointOnConfluenceEdge(true);
                float moveDist = (distance / 2) - dist;// + (streetWidth/90 * angle);
                Vector3 moveDirection = Vector3.Normalize(prePoint.transform.position - point.transform.position);
                prePoint.transform.Translate(moveDirection * moveDist);
                if (isMainStreet)
                    c.GetController().SetPrePoint(pointManager.GetControllerPoints()[index-1]);
            }
            else if (dist > (distance / 2) + 0.5f)
            {
                float moveDist = (distance / 2);// + (streetWidth / 90 * angle);
                Vector3 moveDirection = Vector3.Normalize(prePoint.transform.position - point.transform.position);
                pointManager.CreatePointOnIndex(index);
                /*if (isMainStreet && c.GetIsOnPoint())
                    point.SetID(index+1);
                else if(isMainStreet)*/
                point.SetID(index);
                if (isMainStreet)
                    c.GetController().SetPrePoint(pointManager.GetControllerPoints()[index]);
                else
                    c.SetPrePoint(pointManager.GetControllerPoints()[index]);
                pointManager.GetControllerPoints()[index].SetIsPointOnConfluenceEdge(true);
                pointManager.GetControllerPoints()[index].transform.position = point.transform.position;
                pointManager.GetControllerPoints()[index].transform.Translate(moveDirection * moveDist, Space.World);
            }
            else
            {
                if (isMainStreet)
                    c.GetController().SetPrePoint(pointManager.GetControllerPoints()[index-1]);
                prePoint.GetComponent<ControllerPoint>().SetIsPointOnConfluenceEdge(true);
            }
        }
        if (!isMainStreet)
        {
            if (c.GetPrePoint() != null)
                index = point.GetID() + 1;
            else
                index = point.GetID();
        }
        else
        {
            if (c.GetController().GetPrePoint() != null)
                index = point.GetID() + 1;
            else
                index = point.GetID();
        }
        //Debug.Log(index);
        if (index < pointManager.GetControllerPoints().Count - 1)
        {
            Transform nextPoint = pointManager.GetControllerPoints()[index + 1].transform;
            float dist = Vector3.Distance(nextPoint.transform.position, point.transform.position);
            if ((!isMainStreet && c.GetPostPoint() != null) || (isMainStreet && c.GetController().GetPostPoint() != null))
            {

            }
            else if (dist < (distance / 2) - 0.1f)
            {
                nextPoint.GetComponent<ControllerPoint>().SetIsPointOnConfluenceEdge(true);
                float moveDist = (distance / 2) - dist;
                Vector3 moveDirection = Vector3.Normalize(nextPoint.transform.position - point.transform.position);
                nextPoint.transform.Translate(moveDirection * moveDist);
                if (isMainStreet)
                    c.GetController().SetPostPoint(pointManager.GetControllerPoints()[index + 1]);
            }
            else if (dist > (distance / 2) + 0.5f)
            {
                float moveDist = (distance / 2);
                Vector3 moveDirection = Vector3.Normalize(nextPoint.transform.position - point.transform.position);
                pointManager.CreatePointOnIndex(index + 1);
                if (isMainStreet)
                    c.GetController().SetPostPoint(pointManager.GetControllerPoints()[index + 1]);
                else
                    c.SetPostPoint(pointManager.GetControllerPoints()[index + 1]);

                pointManager.GetControllerPoints()[index + 1].transform.position = point.transform.position;
                pointManager.GetControllerPoints()[index + 1].transform.Translate(moveDirection * moveDist, Space.World);
                pointManager.GetControllerPoints()[index + 1].SetIsPointOnConfluenceEdge(true);
            }
            else
            {
                if (isMainStreet)
                    c.GetController().SetPostPoint(pointManager.GetControllerPoints()[index + 1]);
                nextPoint.GetComponent<ControllerPoint>().SetIsPointOnConfluenceEdge(true);
            }
        }
    }
    public void RelaculatePointsForConfluence(Confluence c,float distance,bool isMainStreet)
    {
        ControllerPoint point = isMainStreet?c.GetOtherPoint():c.GetPoint();
        ControllerPoint otherPoint =isMainStreet?c.GetPoint():c.GetOtherPoint();
        point.SetIsPointOnConfluence(true);
        otherPoint.SetIsPointOnConfluence(true);
        int index = point.GetID();
        /*if (isMainStreet && !c.GetIsOnPoint())
        {
            index++;
            if (c.GetController().GetMainPoint() == null)
            {
                pointManager.CreatePointOnIndex(index);
                c.GetController().SetMainPoint(pointManager.GetControllerPoints()[index]);
                pointManager.GetControllerPoints()[index].transform.position = point.transform.position;
                //pointManager.GetControllerPoints()[index].SetIsPointOnConfluence(true);
            }
            //pointManager.GetControllerPoints()[index].SetIsPointOnConfluence(true);
        }*/

        if (isMainStreet && c.GetIsOnPoint())
        {
            if (c.GetController().GetPrePoint())
                c.GetController().GetPrePoint().GetPostPoint().SetIsPointOnConfluence(true);
            else
                c.GetController().GetPostPoint().GetPrePoint().SetIsPointOnConfluence(true);
        }

        if (!isMainStreet)
            point.transform.position = otherPoint.transform.position;
        //else
        //    pointManager.GetControllerPoints()[index].SetIsPointOnConfluence(true);// Error
        if (index > 0)//if there was point before this one
        {
            Transform prePoint = pointManager.GetControllerPoints()[index-1].transform;
            if(!isMainStreet && c.GetPrePoint()!=null)
            {
                c.GetPrePoint().SetIsPointOnConfluenceEdge(true);
                //prePoint = c.GetPrePoint().transform;
                //ControllerPoint PreprePoint = prePoint.GetComponent<ControllerPoint>().GetManager().GetControllerPoints()[prePoint.GetComponent<ControllerPoint>().GetID() - 1];
                //prePoint.GetComponent<ControllerPoint>().SetIsPointOnConfluenceEdge(true);
                //float moveDist = (distance / 2) - dist;// + (streetWidth / 90 * angle);
                //Vector3 moveDirection = Vector3.Normalize(PreprePoint.transform.position - point.transform.position);
                //prePoint.transform.Translate(moveDirection * moveDist);
            }
            else if(isMainStreet && c.GetController().GetPrePoint()!=null)
            {
                //Rotation Bug
                EdgePointConfluenceInfo info = c.GetController().GetMultyRoadInfoController().GetEdgeInfo(c.GetController().GetPrePoint());
                prePoint = c.GetController().GetPrePoint().transform;
                prePoint.GetComponent<ControllerPoint>().SetIsPointOnConfluenceEdge(true);
                float moveDist = info.GetMoveDistance();
                if (prePoint.GetComponent<ControllerPoint>().GetID() > 0)
                {
                    ControllerPoint PreprePoint = prePoint.GetComponent<ControllerPoint>().GetManager().GetControllerPoints()[prePoint.GetComponent<ControllerPoint>().GetID() - 1];
                    prePoint.transform.rotation = PreprePoint.transform.rotation;
                }
                else
                {

                }
                prePoint.transform.position = point.transform.position;
                prePoint.transform.Translate(0,0,-moveDist);
            }
            else
            {
                prePoint.GetComponent<ControllerPoint>().SetIsPointOnConfluenceEdge(true);
            }
        }
        index = point.GetID()+1;
        if (index < pointManager.GetControllerPoints().Count-1)
        {
            Transform nextPoint = pointManager.GetControllerPoints()[index + 1].transform;
            if (!isMainStreet && c.GetPostPoint() != null)
            {
                c.GetPostPoint().SetIsPointOnConfluenceEdge(true);
                //nextPoint = c.GetPostPoint().transform;
                //nextPoint.GetComponent<ControllerPoint>().SetIsPointOnConfluenceEdge(true);
                //float moveDist = (distance / 2) - dist;
                //Vector3 moveDirection = Vector3.Normalize(nextPoint.transform.position - point.transform.position);
                //nextPoint.transform.Translate(moveDirection * moveDist);
            }
            else if (isMainStreet && c.GetController().GetPostPoint() != null)
            {
                EdgePointConfluenceInfo info = c.GetController().GetMultyRoadInfoController().GetEdgeInfo(c.GetController().GetPostPoint());
                nextPoint = c.GetController().GetPostPoint().transform;
                c.GetController().GetPostPoint().SetIsPointOnConfluenceEdge(true);
                //float moveDist = (distance / 2);
                float moveDist = info.GetMoveDistance();
                nextPoint.transform.rotation = point.transform.rotation;
                nextPoint.transform.position = point.transform.position;
                //Vector3 moveDirection = Vector3.Normalize(nextPoint.transform.position - point.transform.position);
                nextPoint.transform.Translate(0,0, moveDist);
            }
            else
            {
                nextPoint.GetComponent<ControllerPoint>().SetIsPointOnConfluenceEdge(true);
            }
        }
    }
    public void RecalculateEdgePoints(Confluence c,Vector3 preEdgePoint,Vector3 postEdgePoint)
    {
        ControllerPoint point = c.GetPoint();
        ControllerPoint otherPoint = c.GetOtherPoint();

        if (c.GetPrePoint() != null)
        {
            bool isRightOfMainRoad = c.GetOtherPoint().transform.InverseTransformPoint(c.GetPrePoint().GetPrePoint().transform.position).x > 0;
            Transform prePoint = c.GetPrePoint().transform;
            prePoint.position = c.GetOtherPoint().transform.position;
            prePoint.LookAt(c.GetPrePoint().GetPrePoint().transform.position);
            c.GetPrePoint().SetRotationTemp();
            

            Quaternion rot = Quaternion.LookRotation(otherPoint.transform.position - c.GetPrePoint().GetPrePoint().transform.position);
            float angle = Quaternion.Angle(rot, otherPoint.transform.rotation)-90;
            if (angle == 0)
            {
                //Debug.Log("Pre  ----  angle " + angle + "-- is right of main : " + isRightOfMainRoad + " -- Confluence = " + c.GetOtherPoint().name +" --- dist = "+ Mathf.Abs(prePoint.position.z - preEdgePoint.z));
                prePoint.Translate(0, 0, Mathf.Abs(prePoint.position.z - preEdgePoint.z));
            }
            else
            {
                prePoint.position = preEdgePoint;
                bool isRightEdge = isRightOfMainRoad ? angle > 0 : angle < 0;
                Vector3 edgePoint;
                Vector3 localEdgePos = isRightEdge ? new Vector3(-streetWidth / 2, 0, 0) : new Vector3(streetWidth / 2, 0, 0);
                edgePoint = prePoint.TransformPoint(localEdgePos);
                prePoint.position -= edgePoint - preEdgePoint;
            }
            //prePoint.GetComponent<ControllerPoint>().GetManager().GetControllerPoints()[prePoint.GetComponent<ControllerPoint>().GetID()-1].ResetRotation();
        }

        if (c.GetPostPoint() != null)
        {
            bool isRightOfMainRoad = c.GetOtherPoint().transform.InverseTransformPoint(c.GetPostPoint().GetPostPoint().transform.position).x > 0;
            Transform postPoint = c.GetPostPoint().transform;
            postPoint.position = c.GetOtherPoint().transform.position;
            postPoint.LookAt(c.GetPostPoint().GetPostPoint().transform.position);
            c.GetPostPoint().SetRotationTemp();
            
            Quaternion rot = Quaternion.LookRotation(otherPoint.transform.position - c.GetPostPoint().GetPostPoint().transform.position);
            float angle = Quaternion.Angle(rot, otherPoint.transform.rotation) - 90;
            if (angle == 0)
            {
                //Debug.Log("post  ----  angle " + angle + "-- is right of main : " + isRightOfMainRoad + " -- Confluence = " + c.GetOtherPoint().name+" ---- dist = "+Mathf.Abs(postEdgePoint.z - postPoint.position.z));
                postPoint.Translate(0, 0, Mathf.Abs(postEdgePoint.z - postPoint.position.z));
            }
            else
            {
                postPoint.position = postEdgePoint;
                bool isRightEdge = isRightOfMainRoad ? angle >= 0 : angle <= 0;
                Vector3 edgePoint;
                Vector3 localEdgePos = isRightEdge ? new Vector3(-streetWidth / 2, 0, 0) : new Vector3(streetWidth / 2, 0, 0);
                edgePoint = postPoint.TransformPoint(localEdgePos);
                postPoint.position -= edgePoint - postEdgePoint;
            }
        }
    }

    public void Initialize()
    {
        if (!initialize)
        {
            initialize = true;
            if (GetComponent<LineManager>() == null)
                lineManager = gameObject.AddComponent<LineManager>();
            if (lineManager == null)
                lineManager = gameObject.GetComponent<LineManager>();
            lineManager.SetStreetController(this);

            if (GetComponent<PointManager>() == null)
                pointManager = gameObject.AddComponent<PointManager>();
            if (pointManager == null)
                pointManager = gameObject.GetComponent<PointManager>();
            pointManager.SetStreetController(this);

            if (GetComponent<SideWalkManager>() == null)
                sideWalkManager = gameObject.AddComponent<SideWalkManager>();
            if (sideWalkManager == null)
                sideWalkManager = gameObject.GetComponent<SideWalkManager>();

            if (GetComponent<AddLinePoint>() == null)
                gameObject.AddComponent<AddLinePoint>();
        }
    }

    public void OnDrawGizmos()
    {
        if (!initialize)
        {
            Initialize();
        }

        if(clearAll)
        {
            clearAll=false;
            ClearAll();
        }

        if(clearMesh)
        {
            clearMesh = false;
            ClearMeshes();
        }

    }
    public void GenerateBaseMesh()
    {
        SetStreetLinesWidth();
        bool isFirst=true;
        for (int i=0;i<lineManager.Lines.Count;i++)
        {
            bool leftEmpty = sideWalkManager.GetLeftSideWalks()[i].GetEmptySide();
            bool rightEmpty = sideWalkManager.GetRightSideWalks()[i].GetEmptySide();
            //Debug.Log("Generate Line " + i);
            if (!lineManager.Lines[i].GetStartPoint().IsOnConfluence() && !lineManager.Lines[i].GetEndPoint().IsOnConfluence())// && (!lineManager.Lines[i].GetStartPoint().IsOnConfluenceEdge() || !lineManager.Lines[i].GetEndPoint().IsOnConfluenceEdge())) 
            {
                //Debug.Log(" Is Normal Street Line!!!");
                //if (i == 0 || isFirst)
                if (isFirst)
                    lineManager.Lines[i].GenerateBaseMesh();
                else
                    lineManager.Lines[i].GenerateBaseMesh(lineManager.GetTempFrontVertices());

                float length = Vector3.Magnitude(lineManager.Lines[i].GetStartPoint().transform.position - lineManager.Lines[i].GetEndPoint().transform.position);
                sideWalkManager.GenerateBaseMesh(i, length, isFirst,true,true);
                isFirst = false;
            }
            else
            {
                //Debug.Log(" Is On Confluence!!!");
                if (lineManager.Lines[i].GetStartPoint().GetComponent<MeshFilter>())
                    lineManager.Lines[i].GetStartPoint().GetComponent<MeshFilter>().mesh = null;
                if (lineManager.Lines[i].GetStartPoint().GetComponent<PruceduralRoad>())
                    DestroyImmediate(lineManager.Lines[i].GetStartPoint().GetComponent<PruceduralRoad>());
                sideWalkManager.RemoveMeshProcedure(i);

                float length = Vector3.Magnitude(lineManager.Lines[i].GetStartPoint().transform.position - lineManager.Lines[i].GetEndPoint().transform.position);
                if (leftEmpty)
                {
                    sideWalkManager.GenerateBaseMesh(i, length, false, true, false);
                }
                else if (rightEmpty)
                {
                    sideWalkManager.GenerateBaseMesh(i, length, false, false, true);
                }
                
                isFirst = true;
            }
        }
    }
    public void SetRoadPrucedure(UnityEngine.Object p) => lineManager.SetPrucedure(p);
    public void SetSideWalkProcedure(UnityEngine.Object p) => sideWalkManager.SetPrucedure(p);
    public void GeneratePrucedure()
    {
        lineManager.GeneratePrucedure();
        sideWalkManager.GeneratePrucedure();
    }
    public bool IsFirstConfluence(StreetController otherstreet)
    {
        for(int i=0;i< confluences.Count;i++)
        {
            if (otherstreet.CheckConfluences(confluences[i]))
                return false;
        }
        return true;
    }
    public bool CheckConfluences(Confluence c)
    {
        for(int i=0;i<confluences.Count;i++)
        {
            if (confluences[i].CheckEquals(c))
                return true;
        }
        return false;
    }
    public float GetStreetWidth() => streetWidth;
    public void SetStreetWidth(float w) => streetWidth = w;
    public void ClearAll()
    {
        //Debug.Log(lineManager);
        lineManager.ClearAll();
        for (int i = 0; i < pointManager.GetControllerPoints().Count; i++)
        {
            DestroyImmediate(pointManager.GetControllerPoints()[i].gameObject);
        }
        pointManager.ClearAll();
    }
    public void ClearMeshes()
    {
        MeshFilter[] gos = GetComponentsInChildren<MeshFilter>();
        for(int i = 0;i<gos.Length;i++)
            gos[i].mesh = null;
    }
    public ControllerPoint GetInMovePoint() => pointManager.GetInMovePoint();
    public void PointMoved() => manager.CheckStreetContact(this);
    public void MoveFinished() => manager.MoveFinished();
    public void SetManager(StreetNetworkManager SNM) => manager = SNM;
    public void recreateLines() => lineManager.reCreateLines();
    public PointManager GetPointManager() => pointManager;
    public LineManager GetLineManager() => lineManager;
    public SideWalkManager GetSideWalkManager() => sideWalkManager;
}