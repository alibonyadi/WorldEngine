using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StreetNetworkManager : MonoBehaviour
{
    //bool isInited = false;
    [SerializeField]
    private bool initialized = false;
    [SerializeField]
    private float distanceTreshold = 3f;
    [SerializeField]
    private List<StreetController> streets = new List<StreetController>();
    [SerializeField]
    private List<ConfluenceController> confluences = new List<ConfluenceController>();
    [SerializeField]
    private bool addStreet=false;
    [SerializeField]
    private GameObject streetHolder = null;
    [SerializeField]
    private bool reCalculateStreets = false;
    [SerializeField]
    private float streetWidth = 18;
    [SerializeField]
    private bool haveLeftSideWalk = true;
    [SerializeField]
    private float leftSideWalkWidth = 2.5f;
    [SerializeField]
    private bool haveRightSideWalk = true;
    [SerializeField]
    private float rightSideWalkWidth = 2.5f;
    [SerializeField]
    private float sideWalkHeight = 0.3f;
    [SerializeField]
    private UnityEngine.Object roadProcedure;
    [SerializeField]
    private UnityEngine.Object sideWalkProcedure;
    [SerializeField]
    private UnityEngine.Object confluenceProcedure;
    [SerializeField]
    private UnityEngine.Object junctionProcedure;
    [SerializeField]
    private bool setProcedures;
    [SerializeField]
    private bool meshGenerate = false;
    [SerializeField]
    private bool ProcedureGenerate = false;
    [SerializeField]
    private bool allGenerate = false;
    [SerializeField]
    private bool autoMeshGenerate = false;


    public float GetSideWalkHeight() => sideWalkHeight;

    private void RemoveTempPoint(GameObject obj)
    {
        //tempConfluencePoints.Remove(obj);
        //DestroyImmediate(obj);
    }
    private void AddTempPoint(GameObject obj)
    {
        //tempConfluencePoints.Add(obj);
    }

    public List<ConfluenceController> GetAllConfluences() => confluences;

    private void OnDrawGizmos()
    {
        if(!initialized)
        {
            initialized = true;
            Debug.Log(Application.dataPath);
            roadProcedure = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath("Assets/WorldSystem/WallDesigner/CreatedFunctions/simpleRoad.wall", typeof(UnityEngine.Object));
            sideWalkProcedure = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath("Assets/WorldSystem/WallDesigner/CreatedFunctions/SideWalkSimple.wall", typeof(UnityEngine.Object));
            confluenceProcedure = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath("Assets/WorldSystem/WallDesigner/CreatedFunctions/CrossRoad.wall", typeof(UnityEngine.Object));
            junctionProcedure = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath("Assets/WorldSystem/WallDesigner/CreatedFunctions/CrossRoad.wall", typeof(UnityEngine.Object));
            streetHolder = GameObject.Find("StreetNetwork");
            if (streetHolder == null)
                streetHolder = new GameObject("StreetNetwork");
            streetHolder.transform.parent = transform;
        }

        if (allGenerate)
        {
            GenerateAllRoads();
        }

        if (reCalculateStreets)
        {
            reCalculateStreets = false;
            RecalculateStreets();
        }

        if(meshGenerate)
        {
            GenerateAllMeshes();
        }

        if(setProcedures)
        {
            SetAllProcedures();
        }

        if(ProcedureGenerate)
        {
            GenerateAllProcedures();
        }

        for (int i=0;i< confluences.Count;i++)
        {
            //Debug.Log(confluences[i].GetNumberOfConfluences());
            //Confluence contact = tempContacts[i];
            confluences[i].Draw();
            /*if (contact.isStillContacting(distanceTreshold))
            {
                //Gizmos.color = Color.green;
                //Gizmos.DrawSphere(contact.GetOtherPoint().transform.position, 0.5f);
                //Gizmos.DrawLine(contact.GetOtherPoint().transform.position, contact.GetPoint().transform.position);

            }
            else
            {
                if (!contact.GetIsOnPoint())
                    RemoveTempPoint(contact.GetOtherPoint().gameObject);
                tempContacts.Remove(contact);
            }*/
        }
        if(addStreet)
        {
            addStreet = false;
            AddStreet();
        }
    }

    private void CheckStreetHolder()
    {
        if (streetHolder == null)
        {
            streetHolder = GameObject.Find("StreetNetwork");
            if (streetHolder == null)
                streetHolder = new GameObject("StreetNetwork");
            streetHolder.transform.parent = transform;
        }
    }

    public void RemoveStreet(StreetController sc)
    {
        streets.Remove(sc);
    }

    public StreetController AddStreet()
    {
        CheckStreetHolder();
        GameObject sto = new GameObject("Street" + streets.Count);
        sto.transform.parent = streetHolder.transform;
        StreetController sc = sto.AddComponent<StreetController>();
        sc.SetStreetWidth(streetWidth);
        sc.SetManager(this);
        sc.Initialize();
        streets.Add(sc);
        return sc;
    }

    public void GenerateAllProcedures()
    {
        ProcedureGenerate = false;
        for (int i = 0; i < streets.Count; i++)
        {
            streets[i].GeneratePrucedure();
        }

        for (int i = 0; i < confluences.Count; i++)
        {
            confluences[i].GeneratePrucedure();
        }
    }

    public void ClearAll()
    {
        streets.Clear();
        confluences.Clear();
    }
    public void SetAllProcedures()
    {
        setProcedures = false;
        for (int i = 0; i < streets.Count; i++)
        {
            streets[i].SetRoadPrucedure(roadProcedure);
            streets[i].SetSideWalkProcedure(sideWalkProcedure);
        }

        for (int i = 0; i < confluences.Count; i++)
        {
            confluences[i].SetPrucedure(confluenceProcedure);
            //Debug.Log("Path network ok!!!");
            confluences[i].SetSideWalkPrucedure(sideWalkProcedure);
            confluences[i].SetJunctionPrucedure(junctionProcedure);
        }
    }
    public void GenerateAllMeshes()
    {
        meshGenerate = false;
        for (int i = 0; i < streets.Count; i++)
        {
            streets[i].GenerateBaseMesh();
        }

        for (int i = 0; i < confluences.Count; i++)
        {
            confluences[i].GenerateBaseMesh();
        }
    }

    public void GenerateAllRoads()
    {
        allGenerate = false;
        //reCalculateStreets = true;
        RecalculateStreets();
        //meshGenerate = true;
        GenerateAllMeshes();
        //setProcedures = true;
        SetAllProcedures();
        //ProcedureGenerate = true;
        GenerateAllProcedures();
    }

    private void RecalculateStreets()
    {
        for(int i=0;i<streets.Count;i++)
        {
            streets[i].ResetPointsForConfluence();
        }

        for(int i=0; i< confluences.Count;i++)
        {
            //Debug.Log("Width = "+confluences[i].GetStreet().GetStreetWidth());
            confluences[i].RecalculateStreets();
        }
    }

    private void AddConfluenceController(ConfluenceController cc)
    {
        confluences.Add(cc);
    }

    public void MoveFinished()
    {
        RecalculateStreets();
    }

    private ConfluenceController GetOtherConfluence(ControllerPoint cp)//, ConfluenceController CC)
    {
        //StreetController street = cp.GetManager().GetController();

        for(int i=0;i<confluences.Count;i++)
        {
            if (/*confluences[i] != CC &&*/ CheckConfluenceStreetOnController(cp, confluences[i]))
            {
                return confluences[i];
            }
        }

        return null;
    }

    public void CheckStreetContact(StreetController street)
    {
        if(autoMeshGenerate)
            allGenerate = true;
        //Debug.Log("Temp Confluences = " + tempContacts.Count);
        ControllerPoint point1 = street.GetInMovePoint();
        distanceTreshold = streetWidth / 2;
        //ConfluenceController otherConfluence = GetOtherConfluence(point1);
        //bool hasOtherConfluence = otherConfluence != null;
        for (int i = 0; i < confluences.Count; i++)
        {
            //bool hasOtherConfluence = 1;
            bool isNewStreet = !CheckConfluenceStreetOnController(point1, confluences[i]);
            bool isNewPoint = !CheckConfluencePointOnController(point1, confluences[i]);
            //Debug.Log(point1.transform.position);
            //Debug.Log(confluences[i]);
            
            bool isInDistance = Vector3.Distance(point1.transform.position, confluences[i].GetPoint().transform.position) < distanceTreshold;
            Vector3 nearestPoint = new Vector3();
            bool nearPointsOfLine = true;
            /*if (isInDistance && hasOtherConfluence && otherConfluence != confluences[i] && otherConfluence.GetNumberOfConfluences() <= 1)
            {
                Debug.Log("Remove Confluence with main point " + confluences[i].GetPoint().GetComponent<ControllerPoint>().GetID());
                confluences.Remove(otherConfluence);
                otherConfluence.RemoveStreet(point1);
                break;
            }*/
            if (confluences[i].IsOnLine())
            {
                nearestPoint = NearestPointOnFiniteLine(confluences[i].GetlinePoint1().transform.position, confluences[i].GetlinePoint2().transform.position, point1.transform.position);
                nearPointsOfLine = Vector3.Distance(nearestPoint, confluences[i].GetlinePoint2().transform.position) < distanceTreshold || Vector3.Distance(nearestPoint, confluences[i].GetlinePoint1().transform.position) < distanceTreshold;
            }
            if (isInDistance && (isNewStreet || (!isNewStreet && !isNewPoint) ) )
            {
                GameObject tempConfluencePoint = confluences[i].GetPoint();
                if (confluences[i].IsOnLine() && confluences[i].GetNumberOfConfluences() < 2 && !isNewPoint && !nearPointsOfLine)
                    tempConfluencePoint.transform.position = nearestPoint;
                ConfluenceController controller = confluences[i];
                if (isNewStreet)
                {
                    Confluence confluence = new Confluence(point1, tempConfluencePoint.GetComponent<ControllerPoint>(), !confluences[i].IsOnLine(),controller);
                    controller.AddConfluence(confluence);
                    street.AddConfluence(confluence);
                    confluences[i].GetStreet().AddConfluence(confluence);
                    //Debug.Log("add new Confluence!!!");
                }
                else
                    //Debug.Log("Change Old Confluence!!!");
                //street2.addConfluence(confluence);
                return;
            }
            else if(isInDistance)
            {
                //New Point On Old Street
                Debug.LogWarning("Can't Build or Add Confluence!!!");
                // Same Point are moving
                return;
            }
            else //Out of bound (Distance)
            {
                if (!isNewPoint)
                {
                    if (!confluences[i].RemoveStreet(point1))
                    {
                        Debug.Log("Removing Complete Confluence !!!");
                        confluences.RemoveAt(i);
                    }
                    return;
                }
            }
        }
        for (int i = 0; i < streets.Count; i++)
        {
            if (streets[i] != street && street.IsFirstConfluence(streets[i]))
            {
                //Debug.Log("Street Different "+i);
                if (CheckContact(street, streets[i]))
                    return;
            }
        }
    }
    public bool CheckContact(StreetController street1, StreetController street2)
    {
        Confluence confluence;// = new Confluence();
        ControllerPoint point1 = street1.GetInMovePoint();
        List<ControllerPoint> CPs = street2.GetPointManager().GetControllerPoints();
        distanceTreshold = streetWidth / 2;
        for(int i=0;i < CPs.Count;i++)
        {
            bool isInDistance = Vector3.Distance(point1.transform.position, CPs[i].transform.position) < distanceTreshold;
            bool ispointOnConfluence = isPointOnConfluence(CPs[i]);
            if ( isInDistance)// && CheckOtherConfluenceOnPoint(point1, true))
            {
                if(ispointOnConfluence)
                    return false;
                //Debug.Log("Create new Confluence On Point!!!");
                GameObject tempConfluencePoint = new GameObject("ConFluencePoint"+confluences.Count);
                tempConfluencePoint.transform.position = CPs[i].transform.position;
                tempConfluencePoint.transform.rotation = CPs[i].transform.rotation;
                tempConfluencePoint.transform.parent = transform;
                ControllerPoint cp = tempConfluencePoint.AddComponent<ControllerPoint>();
                cp.SetColor(Color.green);
                cp.SetRadius(0.5f);
                cp.SetID(CPs[i].GetID());
                ConfluenceController controller = tempConfluencePoint.AddComponent<ConfluenceController>();
                PruceduralRoad PR = tempConfluencePoint.AddComponent<PruceduralRoad>();
                MultyRoadConfluence MRC = tempConfluencePoint.AddComponent<MultyRoadConfluence>();
                ConfluenceSideWalkController confluenceSideWalkController = tempConfluencePoint.AddComponent<ConfluenceSideWalkController>();
                controller.SetMultyRoadInfoController(MRC);
                controller.SetConfluenceSideWalkController(confluenceSideWalkController);
                PR.SetProcedure(confluenceProcedure);
                PR.SetMeshHolder(tempConfluencePoint);
                //controller.SetPoint(tempConfluencePoint);
                controller.SetStreet(street2);
                controller.SetOnLine(false);
                confluence = new Confluence(point1, cp, true,controller);
                //confluence = new Confluence(point1, CPs[i],true);
                controller.AddConfluence(confluence);
                street1.AddConfluence(confluence);
                street2.AddConfluence(confluence);
                AddConfluenceController(controller);
                return true;
            }
        }
        for (int i = 0; i < CPs.Count; i++)
        {
            if (i < CPs.Count - 1)
            {
                Vector3 nearestPoint = NearestPointOnFiniteLine(CPs[i].transform.position, CPs[i + 1].transform.position, point1.transform.position);
                if (Vector3.Distance(nearestPoint, point1.transform.position) < distanceTreshold)// && CheckOtherConfluenceOnPoint(point1, false))
                {
                    //Debug.Log("Create new Confluence On Line!!!");
                    GameObject tempConfluencePoint = new GameObject("ConFluencePoint" + confluences.Count);
                    tempConfluencePoint.transform.position = nearestPoint;
                    tempConfluencePoint.transform.LookAt(CPs[i + 1].transform.position);
                    tempConfluencePoint.transform.parent = transform;
                    ControllerPoint cp = tempConfluencePoint.AddComponent<ControllerPoint>();
                    //cp.SetController(street2.GetPointManager());
                    cp.SetColor(Color.green);
                    cp.SetRadius(0.5f);
                    cp.SetID(CPs[i].GetID());
                    ConfluenceController controller = tempConfluencePoint.AddComponent<ConfluenceController>();
                    PruceduralRoad PR = tempConfluencePoint.AddComponent<PruceduralRoad>();
                    MultyRoadConfluence MRC = tempConfluencePoint.AddComponent<MultyRoadConfluence>();
                    ConfluenceSideWalkController confluenceSideWalkController = tempConfluencePoint.AddComponent<ConfluenceSideWalkController>();
                    controller.SetMultyRoadInfoController(MRC);
                    controller.SetConfluenceSideWalkController(confluenceSideWalkController);
                    PR.SetProcedure(confluenceProcedure);
                    PR.SetMeshHolder(tempConfluencePoint);
                    //controller.SetPoint(tempConfluencePoint);
                    controller.SetStreet(street2);
                    controller.SetLinePoint1(CPs[i]);
                    controller.SetLinePoint2(CPs[i+1]);
                    controller.SetOnLine(true);
                    confluence = new Confluence(point1, tempConfluencePoint.GetComponent<ControllerPoint>(), false, controller);
                    controller.AddConfluence(confluence);
                    street1.AddConfluence(confluence);
                    street2.AddConfluence(confluence);
                    AddConfluenceController(controller);
                    return true;
                }
            }
        }
        return false;
    }

    private bool isPointOnConfluence(ControllerPoint cp)
    {
        bool ispointOnConfluence = false;
        for(int i=0;i<confluences.Count;i++)
        {
            if(confluences[i].ContainPoint(cp))
                ispointOnConfluence = true;
        }

        return ispointOnConfluence;
    }

    private bool CheckConfluenceStreetOnController(ControllerPoint cp,ConfluenceController cc) => 
        cc.ContainStreet(cp.GetManager().GetController());

    private bool CheckConfluencePointOnController(ControllerPoint cp, ConfluenceController cc) =>
        cc.ContainPoint(cp);
     
    private bool CheckOtherConfluenceOnPoint(ControllerPoint cp, bool isOnPoint)
    {
        List<int> mustRemove = new List<int>();
        bool returnvalue = true;
        /*if (tempContacts.Count == 0)
            return true;
        for (int i = 0; i < tempContacts.Count; i++)
        {
            if (tempContacts[i].GetPoint() == cp)
            {
                if (isOnPoint || (!isOnPoint && !tempContacts[i].GetIsOnPoint()))
                {
                    mustRemove.Add(i);
                    returnvalue = true;
                }
            }
            else
            {
                returnvalue = true;
            }
        }
        for(int i = 0;i < mustRemove.Count;i++)
        {
            if(!tempContacts[mustRemove[i]].GetIsOnPoint())
            RemoveTempPoint(tempContacts[mustRemove[i]].GetOtherPoint().gameObject);
            tempContacts.RemoveAt(mustRemove[i]);
        }
        if (isOnPoint )
        {
            returnvalue = true;
        }*/
        return returnvalue;
    }
    public static Vector3 NearestPointOnFiniteLine(Vector3 start, Vector3 end, Vector3 pnt)
    {
        var line = (end - start);
        var len = line.magnitude;
        line.Normalize();

        var v = pnt - start;
        var d = Vector3.Dot(v, line);
        d = Mathf.Clamp(d, 0f, len);
        return start + line * d;
    }
}