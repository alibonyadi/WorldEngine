using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class ConfluenceController : MonoBehaviour
{
    [SerializeField]
    private List<Confluence> confluences = new List<Confluence>();
    [SerializeField]
    private MultyRoadConfluence multyRoadConfluence = new MultyRoadConfluence();
    [SerializeField]
    private ConfluenceSideWalkController confluenceSideWalk = new ConfluenceSideWalkController();
    [SerializeField]
    private StreetController lineStreet;
    [SerializeField]
    private bool isOnLine = false;
    [SerializeField]
    private bool typeFounded = false;
    [SerializeField]
    private ControllerPoint linePoint1;
    [SerializeField]
    private ControllerPoint linePoint2;
    [SerializeField]
    private ControllerPoint preConfluecncePoint;
    [SerializeField]
    private ControllerPoint postConfluencePoint;
    [SerializeField]
    private ControllerPoint AddedMainControllerPoint;
    [SerializeField]
    private bool canDraw = true;
    [SerializeField]
    private float biggestStreetWidth = 0;
    [SerializeField]
    private ConfluenceConnectType connectType = ConfluenceConnectType.equalWidth;
    [SerializeField]
    List<Vector3> confluenceMeshVertices;
    [SerializeField]
    private Mesh ConfluenceMesh;
    [SerializeField]
    private List<Mesh> Junctions;

    public List<Confluence> GetAllConfluences() => confluences;
    public void SetOnLine(bool onLine)=> isOnLine = onLine;
    public bool IsOnLine() => isOnLine;
    public void AddConfluence(Confluence c)
    {
        confluences.Add(c);
        FindBiggestStreet();
    }
    public GameObject GetPoint() => gameObject;
    public void SetStreet(StreetController st) => lineStreet = st;
    public StreetController GetStreet() => lineStreet;
    public bool ContainPoint(ControllerPoint cp)
    {
        if(isOnLine && cp == AddedMainControllerPoint)
            return true;

        foreach(Confluence c in confluences)
            if(c.ContainPoint(cp))
                return true;

        return false;
    }
    public void SetConfluenceSideWalkController(ConfluenceSideWalkController cswc) => confluenceSideWalk = cswc;
    public ConfluenceSideWalkController GetConfluenceSideWalkController() => confluenceSideWalk;
    public void SetMultyRoadInfoController(MultyRoadConfluence MRC) => multyRoadConfluence = MRC;
    public MultyRoadConfluence GetMultyRoadInfoController() => multyRoadConfluence;
    public int GetNumberOfConfluences() => confluences.Count;
    public bool ContainStreet(StreetController SC)
    {
        foreach (Confluence c in confluences)
        {
            //Debug.Log("other point = " + c.GetOtherPoint().name);
          //  if (c.GetIsOnPoint() && (c.GetPoint().GetManager().GetController() == SC || c.GetOtherPoint().GetManager().GetController() == SC))
          //      return true;
            //else 
            if (/*!c.GetIsOnPoint() &&*/ (c.GetPoint().GetManager().GetController() == SC || lineStreet == SC ))
                return true;
        }
        return false;
    }

    public void SetPrucedure(UnityEngine.Object p)
    {
        if (GetComponent<PruceduralRoad>())
            GetComponent<PruceduralRoad>().SetProcedure(p);
    }

    public void SetSideWalkPrucedure(UnityEngine.Object p) 
    { 
        //Debug.Log("Path CC ok"); 
        confluenceSideWalk.SetPrucedure(p);
    }

    public void SetJunctionPrucedure(UnityEngine.Object p)
    {
        confluenceSideWalk.SetJunctionPrucedure(p);
    }

    public void GeneratePrucedure()
    {
        GetComponent<PruceduralRoad>().GenerateMeshes();
        confluenceSideWalk.GeneratePrucedure();
    }

    public void SetPrePoint(ControllerPoint cp) => preConfluecncePoint = cp;
    public void SetPostPoint(ControllerPoint cp) => postConfluencePoint = cp;
    public ControllerPoint GetPrePoint() => preConfluecncePoint;
    public ControllerPoint GetPostPoint() => postConfluencePoint;
    public void SetMainPoint(ControllerPoint cp)
    {
        //Debug.Log("Main Center Point Set as" + cp);
        AddedMainControllerPoint = cp;
    }
    public ControllerPoint GetMainPoint() => AddedMainControllerPoint;
    public bool RemoveStreet(ControllerPoint cp)
    {
        foreach (Confluence c in confluences)
        {
            if (c.ContainPoint(cp))
            {
                c.GetPoint().GetManager().GetController().RemoveConfluence(c);
                //Debug.Log("Point street Removed!!");
                /*if (isOnLine)
                {
                    lineStreet.RemoveConfluence(c);
                    Debug.Log("Line street Removed!!");
                }
                else
                {*/
                    //c.GetOtherPoint().GetManager().GetController().RemoveConfluence(c);
                    lineStreet.RemoveConfluence(c);
               //     Debug.Log("Last Point street Removed!!");
               // }
                confluences.Remove(c);
                break;
            }
        }

        if (confluences.Count <= 0)
        {
            DestroyImmediate(gameObject);
            //Debug.Log("GameObject Removed!!");
        }
        FindBiggestStreet();
        return confluences.Count > 0;
    }

    public void GenerateBaseMesh()
    {
        switch(connectType)
        {
            case ConfluenceConnectType.singleConfluence:

                break;

            case ConfluenceConnectType.equalWidth:
                    multyConfluenceGenerateBaseMesh();
                break;

            case ConfluenceConnectType.square:
                break;
        }
    }
    public void multyConfluenceGenerateBaseMesh()
    {
        List<Vector3> backvertices = new List<Vector3>();
        List<Vector3> frontvertices = new List<Vector3>();
        List<Vector3> leftvertices = new List<Vector3>();
        List<float> leftAngles = new List<float>();
        List<Vector3> rightvertices = new List<Vector3>();
        List<float> rightAngles = new List<float>();
        //Debug.Log(name);
        Vector3 edgerightPoint = transform.InverseTransformPoint(preConfluecncePoint.transform.TransformPoint(new Vector3(lineStreet.GetStreetWidth()/2,0,0)));
        Vector3 edgeleftPoint2 = transform.InverseTransformPoint(preConfluecncePoint.transform.TransformPoint(new Vector3(-lineStreet.GetStreetWidth()/2,0,0)));
        backvertices.Add(edgerightPoint);
        backvertices.Add(edgeleftPoint2);

        Vector3[] tempvertices = preConfluecncePoint.GetPrePoint().GetComponent<MeshFilter>().sharedMesh.vertices;
        int[] tempTriangles = preConfluecncePoint.GetPrePoint().GetComponent<MeshFilter>().sharedMesh.triangles;
        Vector2[] tempUVs = preConfluecncePoint.GetPrePoint().GetComponent<MeshFilter>().sharedMesh.uv;
        tempvertices[1] = preConfluecncePoint.GetPrePoint().transform.InverseTransformPoint(preConfluecncePoint.transform.TransformPoint(new Vector3(lineStreet.GetStreetWidth() / 2, 0, 0)));
        tempvertices[0] = preConfluecncePoint.GetPrePoint().transform.InverseTransformPoint(preConfluecncePoint.transform.TransformPoint(new Vector3(-lineStreet.GetStreetWidth() / 2, 0, 0)));
        Mesh preRebuildLSMesh = MeshUtility.CreateMesh(tempvertices, tempUVs, tempTriangles);
        preConfluecncePoint.GetPrePoint().GetComponent<MeshFilter>().mesh = preRebuildLSMesh;

        Vector3 edgerightpostPoint = transform.InverseTransformPoint(postConfluencePoint.transform.TransformPoint(new Vector3(lineStreet.GetStreetWidth() / 2, 0, 0)));
        Vector3 edgeleftpostPoint = transform.InverseTransformPoint(postConfluencePoint.transform.TransformPoint(new Vector3(-lineStreet.GetStreetWidth() / 2, 0, 0)));
        frontvertices.Add(edgerightpostPoint);
        frontvertices.Add(edgeleftpostPoint);

        //Debug.Log(backvertices[0]);
        for (int i = 0; i < confluences.Count; i++)
        {
            bool isRightOfMainRoad = false;
            float angle = 0;
            if (confluences[i].GetPrePoint() != null)
            {
                isRightOfMainRoad = confluences[i].GetOtherPoint().transform.InverseTransformPoint(confluences[i].GetPrePoint().transform.position).x > 0;
                angle = (90 - Quaternion.Angle(confluences[i].GetPrePoint().transform.rotation, confluences[i].GetOtherPoint().transform.rotation));

                Vector3 edgeprepoint = transform.InverseTransformPoint(confluences[i].GetPrePoint().transform.TransformPoint(new Vector3(confluences[i].GetPrePoint().GetComponent<Line>().GetWidth() / 2, 0, 0)));
                Vector3 edgeprepoint2 = transform.InverseTransformPoint(confluences[i].GetPrePoint().transform.TransformPoint(new Vector3(-confluences[i].GetPrePoint().GetComponent<Line>().GetWidth() / 2, 0, 0)));

                Vector3[] vertices = confluences[i].GetPrePoint().GetPrePoint().GetComponent<MeshFilter>().sharedMesh.vertices;
                Vector2[] UVs = confluences[i].GetPrePoint().GetPrePoint().GetComponent<MeshFilter>().sharedMesh.uv;
                int[] triangles = confluences[i].GetPrePoint().GetPrePoint().GetComponent<MeshFilter>().sharedMesh.triangles;
                vertices[0] = confluences[i].GetPrePoint().GetPrePoint().transform.InverseTransformPoint(confluences[i].GetPrePoint().transform.TransformPoint(new Vector3(confluences[i].GetPrePoint().GetComponent<Line>().GetWidth() / 2, 0, 0)));
                vertices[1] = confluences[i].GetPrePoint().GetPrePoint().transform.InverseTransformPoint(confluences[i].GetPrePoint().transform.TransformPoint(new Vector3(-confluences[i].GetPrePoint().GetComponent<Line>().GetWidth() / 2, 0, 0)));
                Mesh preRebuildMesh = MeshUtility.CreateMesh(vertices,UVs,triangles);
                confluences[i].GetPrePoint().GetPrePoint().GetComponent<MeshFilter>().mesh = preRebuildMesh;

                if (isRightOfMainRoad)
                {
                    rightvertices.Add(edgeprepoint);
                    rightvertices.Add(edgeprepoint2);
                }
                else
                {
                    leftvertices.Add(edgeprepoint);
                    leftvertices.Add(edgeprepoint2);
                }
            }

            if(confluences[i].GetPostPoint() != null)
            {
                isRightOfMainRoad = confluences[i].GetOtherPoint().transform.InverseTransformPoint(confluences[i].GetPostPoint().transform.position).x > 0;
                angle = (90 - Quaternion.Angle(confluences[i].GetPostPoint().transform.rotation, confluences[i].GetOtherPoint().transform.rotation));
                Vector3 edgeprepoint = transform.InverseTransformPoint(confluences[i].GetPostPoint().transform.TransformPoint(new Vector3(confluences[i].GetPostPoint().GetComponent<Line>().GetWidth() / 2, 0, 0)));
                Vector3 edgeprepoint2 = transform.InverseTransformPoint(confluences[i].GetPostPoint().transform.TransformPoint(new Vector3(-confluences[i].GetPostPoint().GetComponent<Line>().GetWidth() / 2, 0, 0)));

                if (isRightOfMainRoad)
                {
                    rightvertices.Add(edgeprepoint);
                    rightvertices.Add(edgeprepoint2);
                }
                else
                {
                    leftvertices.Add(edgeprepoint);
                    leftvertices.Add(edgeprepoint2);
                }
            }
        }

        List<Vector3> allvertices = backvertices;
        allvertices.AddRange(leftvertices);
        allvertices.AddRange(frontvertices);
        allvertices.AddRange(rightvertices);

        Mesh mesh = MeshUtility.GenerateFlatMeshOnVertices(MeshUtility.WeldVertices(allvertices,0.1f));
        //Mesh mesh = MeshUtility.GenerateBaseConfluenceMesh(backvertices, leftvertices, frontvertices, rightvertices);
        ConfluenceMesh = mesh;

        if(!GetComponent<MeshFilter>())
            gameObject.AddComponent<MeshFilter>();

        if (!GetComponent<MeshRenderer>())
            gameObject.AddComponent<MeshRenderer>();

        GetComponent<MeshFilter>().mesh = mesh;
        confluenceSideWalk.BuildConfluenceSideWalkBase();
        confluenceSideWalk.BuildConfluenceSideWalkCross();
    }
    private void FindBiggestStreet()
    {
        float biggest = 0;
        for(int i=0;i<confluences.Count;i++)
        {
            if (biggest < confluences[i].GetPoint().GetManager().GetController().GetStreetWidth())
                biggest = confluences[i].GetPoint().GetManager().GetController().GetStreetWidth();
        }
        biggestStreetWidth = biggest;
    }
    private Vector3 FindEdgePoint(bool isRight,bool isFront)
    {
        Transform point = isFront?GetPostPoint().transform:GetPrePoint().transform;
        Vector3 localEdgePos = isRight ? new Vector3(lineStreet.GetStreetWidth() / 2, 0, 0) : new Vector3(-lineStreet.GetStreetWidth() / 2, 0, 0);
        Vector3 edgePoint = point.TransformPoint(localEdgePos);
        return edgePoint;
    }
    private void CalculateMultyRoadInfo()
    {
        multyRoadConfluence.Reset();
        EdgePointConfluenceInfo ePCIpre = new EdgePointConfluenceInfo();
        ePCIpre.SetMainStreet(true);
        //Debug.Log("Main Pre Point = " + GetPrePoint());
        if(GetPrePoint()!=null)
            ePCIpre.SetPoint(GetPrePoint());
        ePCIpre.SetIsPreEdge(true);
        multyRoadConfluence.AddEdgeInfo(ePCIpre);

        EdgePointConfluenceInfo ePCIpost = new EdgePointConfluenceInfo();
        ePCIpost.SetMainStreet(true);
        if (GetPostPoint() != null)
        {
            ePCIpost.SetPoint(GetPostPoint());
        }
        ePCIpost.SetIsPreEdge(false);
        multyRoadConfluence.AddEdgeInfo(ePCIpost);
        
        for(int i=0;i<confluences.Count;i++)
        {
            if (confluences[i].GetPrePoint() != null)
            {
                EdgePointConfluenceInfo ePCIpreC = new EdgePointConfluenceInfo();
                ePCIpreC.SetMainStreet(false);
                ePCIpreC.SetPoint(confluences[i].GetPrePoint());
                ePCIpreC.SetIsPreEdge(true);
                multyRoadConfluence.AddEdgeInfo(ePCIpreC);
            }
            if (confluences[i].GetPostPoint() != null)
            {
                EdgePointConfluenceInfo ePCIpostC = new EdgePointConfluenceInfo();
                ePCIpostC.SetMainStreet(false);
                ePCIpostC.SetPoint(confluences[i].GetPostPoint());
                ePCIpostC.SetIsPreEdge(false);
                multyRoadConfluence.AddEdgeInfo(ePCIpostC);
            }
        }
    }
    private void UpdateConfluenceIndex()
    {
        if (AddedMainControllerPoint != null)
        {
            //Debug.Log(AddedMainControllerPoint.GetID());
            confluences[0].GetOtherPoint().SetID(AddedMainControllerPoint.GetID());
        }
    }

    public void FindConfluenceType()
    {
        //typeFounded = true;

        

        // check number of confluenceSides
        int side = 0;
        if (isOnLine)
        {
            side += 2;
        }
        else
        {
            if (lineStreet.GetPointManager().GetControllerPoints()[confluences[0].GetOtherPoint().GetID()].GetPrePoint() != null)
            {
                side++;
                //Debug.Log("Main pre side!!!");
            }
            if (lineStreet.GetPointManager().GetControllerPoints()[confluences[0].GetOtherPoint().GetID()].GetPostPoint() != null)
            {
                side++;
                //Debug.Log("Main post side!!!");
            }
        }

        for(int i=0;i<confluences.Count;i++)
        {
            if(confluences[i].GetPoint().GetPrePoint()!=null)
            {
                side++;
                //Debug.Log("Confluence "+i+" pre side!!!");
            }
            if (confluences[i].GetPoint().GetPostPoint()!=null)
            {
                side++;
                //Debug.Log("Confluence " + i + " post side!!!");
            }

            //if (side >= 3)
            //    break;
        }

        //Debug.Log(name + " -- Sides = " + side);

        // check is square
        if (confluences.Count > 3 || (confluences.Count == 3 && side!=4 ))
        {
            connectType = ConfluenceConnectType.square;
            Debug.Log("Square Founded!!!");
            return;
        }

        // Check Single Connection
        if (side<3)
        {
            connectType = ConfluenceConnectType.singleConfluence;
            Debug.Log("Single Contact Founded!!!");
            return;
        }

        // Check Different Width Confluence
        bool differentWidth =false;
        float width = lineStreet.GetStreetWidth();


        for(int i = 0;i<confluences.Count;i++)
        {
            if (confluences[i].GetPoint().GetManager().GetController().GetStreetWidth()!=width)
            {
                connectType = ConfluenceConnectType.differentWidth;
                Debug.Log("Different Width Founded!!!");
                return;
            }
        }

        // in the end and if it is not each of above 
        connectType = ConfluenceConnectType.equalWidth;
        //Debug.Log("Equal Width Founded!!!");
        CheckMainStreet();
    }

    public void CheckMainStreet()
    {
        int side = 0;
        if (isOnLine)
        {
            side += 2;
        }
        else
        {
            if (lineStreet.GetPointManager().GetControllerPoints()[confluences[0].GetOtherPoint().GetID()].GetPrePoint() != null)
            {
                side++;
            }
            if (lineStreet.GetPointManager().GetControllerPoints()[confluences[0].GetOtherPoint().GetID()].GetPostPoint() != null)
            {
                side++;
            }
        }

        if (side == 2)
            return;

        for(int i = 0; i < confluences.Count;i++)
        {
            side = 0;
            if (confluences[i].GetPoint().GetPrePoint() != null)
            {
                side++;
            }
            if (confluences[i].GetPoint().GetPostPoint() != null)
            {
                side++;
            }
            if (side == 2)
            {
                ChangeMainStreet(i);
                return;
            }
        }

    }

    public void ChangeMainStreet(int index)
    {
        StreetController newSC = confluences[index].GetPoint().GetManager().GetController();// new linestreet
        int id = confluences[index].GetPoint().GetID();
        Confluence newConfluence = new Confluence(lineStreet.GetPointManager().GetControllerPoints()[confluences[index].GetOtherPoint().GetID()],confluences[index].GetOtherPoint(), confluences[index].GetIsOnPoint(),this);
        newConfluence.GetOtherPoint().SetID(id);
        newConfluence.GetOtherPoint().transform.rotation = confluences[index].GetPoint().transform.rotation;

        lineStreet.RemoveConfluence(lineStreet.FindConfluenceByController(this));
        lineStreet.AddConfluence(newConfluence);

        lineStreet = newSC;
        lineStreet.RemoveConfluence(lineStreet.FindConfluenceByController(this));
        lineStreet.AddConfluence(newConfluence);

        confluences[index] = newConfluence;
        Debug.Log(name + " -- Main Street Changed!!!");
    }

    public void RecalculateStreets()
    {
        //if (!typeFounded)
        FindConfluenceType();

        switch (connectType)
        {
            case ConfluenceConnectType.singleConfluence:
                break;

            case ConfluenceConnectType.equalWidth:
                MultyConfluence();
                break;

            case ConfluenceConnectType.square:

                break;
        }
    }

    private void MultyConfluence()
    {
        bool isRightOfMainRoad = false;
        bool isNearFrontEdge = true;
        UpdateConfluenceIndex();
        //Debug.Log("Added main point Id = "+AddedMainControllerPoint.GetID());
        lineStreet.CreateEdgePoints(confluences[0], biggestStreetWidth, true);
        for (int i = 0; i < confluences.Count; i++)
        {
            confluences[i].GetPoint().GetManager().GetController().CreateEdgePoints(confluences[i], lineStreet.GetStreetWidth(), false);
        }
        CalculateMultyRoadInfo();
        lineStreet.RelaculatePointsForConfluence(confluences[0], biggestStreetWidth, true);
        for (int i = 0; i < confluences.Count; i++)
        {
            confluences[i].GetPoint().GetManager().GetController().RelaculatePointsForConfluence(confluences[i], lineStreet.GetStreetWidth(), false);
            Vector3 preEdgepoint = Vector3.zero;
            Vector3 postEdgepoint = Vector3.zero;
            if (confluences[i].GetPrePoint() != null)
            {
                isRightOfMainRoad = confluences[i].GetOtherPoint().transform.InverseTransformPoint(confluences[i].GetPrePoint().GetPrePoint().transform.position).x > 0;
                Quaternion rot = Quaternion.LookRotation(confluences[i].GetOtherPoint().transform.position - confluences[i].GetPrePoint().GetPrePoint().transform.position, Vector3.up);
                float angle = (Quaternion.Angle(rot, confluences[i].GetOtherPoint().transform.rotation) - 90);
                isNearFrontEdge = angle > 0;
                preEdgepoint = FindEdgePoint(isRightOfMainRoad, isNearFrontEdge);
                //Debug.Log("Main street selecdted pre edge position = "+preEdgepoint);
                //Debug.Log("is Front = "+ isNearFrontEdge +" -- is Right = "+isRightOfMainRoad+" angle = "+ angle);
            }
            if (confluences[i].GetPostPoint() != null)
            {
                isRightOfMainRoad = confluences[i].GetOtherPoint().transform.InverseTransformPoint(confluences[i].GetPostPoint().GetPostPoint().transform.position).x > 0;
                Quaternion rot = Quaternion.LookRotation(confluences[i].GetOtherPoint().transform.position - confluences[i].GetPostPoint().GetPostPoint().transform.position, Vector3.up);
                float angle = (Quaternion.Angle(rot, confluences[i].GetOtherPoint().transform.rotation) - 90);
                isNearFrontEdge = angle > 0;
                postEdgepoint = FindEdgePoint(isRightOfMainRoad, isNearFrontEdge);
                //Debug.Log("Main street selecdted post edge position = " + postEdgepoint);
                //Debug.Log("is Front = " + isNearFrontEdge + " -- is Right = " + isRightOfMainRoad + " angle = " + angle);
            }
            //Debug.Log("Pre Edge point = "+preEdgepoint+" --- post Edge point = "+postEdgepoint);
            confluences[i].GetPoint().GetManager().GetController().RecalculateEdgePoints(confluences[i], preEdgepoint, postEdgepoint);
        }
        confluenceSideWalk.RecalculateSideWalksForConfluence(confluences);
    }

    public bool IsPointRightOfVector(Vector3 vector, Vector3 point)
    {
        Vector3 perpendicular = new Vector3(vector.z, 0f, -vector.x);
        Vector3 pointToOrigin = point - Vector3.zero;
        float dotProduct = Vector3.Dot(perpendicular, pointToOrigin);

        return dotProduct > 0f;
    }

    public ControllerPoint GetlinePoint1() => linePoint1;

    public void SetLinePoint1(ControllerPoint cp) => linePoint1 = cp;

    public ControllerPoint GetlinePoint2() => linePoint2;

    public void SetLinePoint2(ControllerPoint cp) => linePoint2 = cp;
    public void Draw()
    {
        if(canDraw)
        foreach(Confluence c in confluences)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(c.GetOtherPoint().transform.position, c.GetPoint().transform.position);
        }
    }
}
