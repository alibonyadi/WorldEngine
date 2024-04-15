using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LotManager : MonoBehaviour
{
    [SerializeField]
    private List<Lot> lots = new List<Lot>();
    [SerializeField]
    private CityManager cityManager = new CityManager();
    [SerializeField]
    private UnityEngine.Object Apartments;
    [SerializeField]
    private UnityEngine.Object VilaHouses;
    [SerializeField]
    private UnityEngine.Object Industrials;
    [SerializeField]
    private bool initialized = false;

    private ControllerPoint tempPrePointConfluence;

    public void OnDrawGizmos()
    {
        if (!initialized)
        {
            initialized = true;
            Debug.Log(Application.dataPath);
            cityManager = GetComponent<CityManager>();
            Apartments = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath("Assets/WorldSystem/WallDesigner/CreatedFunctions/SimpleCity.wall", typeof(UnityEngine.Object));
            VilaHouses = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath("Assets/WorldSystem/WallDesigner/CreatedFunctions/SideWalkSimple.wall", typeof(UnityEngine.Object));
            Industrials = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath("Assets/WorldSystem/WallDesigner/CreatedFunctions/CrossRoad.wall", typeof(UnityEngine.Object));
            //junctionProcedure = (UnityEngine.Object)AssetDatabase.LoadAssetAtPath("Assets/WorldSystem/WallDesigner/CreatedFunctions/CrossRoad.wall", typeof(UnityEngine.Object));
        }
    }
    public void FindAllCityLots()
    {
        List<ConfluenceController> confluenceControllers = cityManager.GetRoadNetwork().GetAllConfluences();
        
        for(int i=0;i<confluenceControllers.Count;i++)
        {
            FindNewLot(confluenceControllers[i]);
        }
    }

    public void ClearAll()
    {
        for(int i=0;i<lots.Count;i++)
        {
            DestroyImmediate(lots[i].gameObject);
        }
        lots.Clear();
    }
    public void GenerateAllLots()
    {
        ClearAll();
        FindAllCityLots();
        GenerateLotBaseMeshes();
        GeneratePrucedures();
    }

    public void GenerateLotBaseMeshes()
    {
        for(int i=0;i<lots.Count;i++)
        {
            lots[i].GenerateBaseMesh();
        }
    }

    public void GeneratePrucedures()
    {
        for(int i=0;i<lots.Count;i++)
        {
            lots[i].GetComponent<PruceduralRoad>().SetProcedure(Apartments);
            lots[i].GetComponent<PruceduralRoad>().GenerateMeshes();
        }
    }

    public void FindNewLot(ConfluenceController cc)
    {
        List<Vector3> points = new List<Vector3>();
        List<ConfluenceController> tempConfluenceControllers = new List<ConfluenceController>();

        List<Confluence> confluences = cc.GetAllConfluences();
        bool inSearch=true;

        //right side 
        if(!cc.GetComponent<ConfluenceSideWalkController>().IsRightOfMainEmpty())
        {
            CheckClosedLine(cc, cc.GetPostPoint(), true);
            CheckClosedLine(cc, cc.GetPrePoint(), false);
        }

        //left side
        if(!cc.GetComponent<ConfluenceSideWalkController>().IsLeftOfMainEmpty())
        {
            CheckClosedLine(cc, cc.GetPostPoint(), false);
            CheckClosedLine(cc, cc.GetPrePoint(), true);
        }

        for(int i=0;i<confluences.Count;i++)
        {
            if (confluences[i].GetPrePoint() != null)
            {
                CheckClosedLine(cc, confluences[i].GetPrePoint(), true);
                CheckClosedLine(cc, confluences[i].GetPrePoint(), false);
            }

            if (confluences[i].GetPostPoint() != null)
            {
                CheckClosedLine(cc, confluences[i].GetPostPoint(), true);
                CheckClosedLine(cc, confluences[i].GetPostPoint(), false);
            }

        }
        
    }

    public void CheckClosedLine(ConfluenceController cc,ControllerPoint nextCp,bool isRight)
    {
        bool inSearch = true;
        List<Vector3> points = new List<Vector3>();
        List<ConfluenceController> LotEdgeCCs = new List<ConfluenceController>();
        ConfluenceController nextCC = cc;
        //ConfluenceController TempCC = cc;
        ControllerPoint nextCP = nextCp;
        int counter = 0;
        do
        {
            if (counter > 0)
                nextCP = FindNextPoint(nextCC, tempPrePointConfluence, isRight);

            if (nextCP == null)
                break;
            points.AddRange(GetPoints(nextCC, nextCP, isRight));
            LotEdgeCCs.Add(nextCC);
            //TempCC = nextCC;
            nextCC = FindNextConfluenceCOntroller(nextCP);
            

            if (nextCC == null)
            {
                counter = 0;
                break;
            }

            if (counter > 0 && nextCC == cc)
            {
                AddLot(points, LotEdgeCCs);
                counter = 0;
                //points.Clear();
                break;
            }
            counter++;
            if (counter > 20)
            {
                counter = 0;
                break;
            }
        }
        while (inSearch);
    }

    public ControllerPoint FindNextPoint(ConfluenceController nextCC, ControllerPoint PreCp,bool isRight)
    {
        ControllerPoint resultPoint = null;

        if(isRight)
            resultPoint = nextCC.GetMultyRoadInfoController().GetEdgeInfo(PreCp).GetRight();
        else
            resultPoint = nextCC.GetMultyRoadInfoController().GetEdgeInfo(PreCp).GetLeft();

        if(resultPoint == null)
        {
            if(PreCp.GetPostPoint().IsOnConfluence())
            {
                resultPoint = PreCp.GetPostPoint().GetPostPoint();
            }
            else
            {
                resultPoint = PreCp.GetPrePoint().GetPrePoint();
            }
        }
        return resultPoint;
    }

    public void AddLot(List<Vector3> points,List<ConfluenceController> edgeCCs)
    {
        Lot lot = new Lot();
        lot.SetPoints(points);
        lot.SetConfluences(edgeCCs);
        //float x, y, z;
        for (int i = 0; i < lots.Count; i++)
        {
            if(lots[i].CheckIsEquality(lot))
            {
                //lots[i].SetConfluences(edgeCCs);
                lots[i].SetPoints(points);
                lots[i].SetHeight(cityManager.GetRoadNetwork().GetSideWalkHeight());
                return;
            }
        }
        
        GameObject lotObject = new GameObject("Lot"+ lots.Count);
        //lotObject.transform.position = points[0];
        lotObject.transform.parent = transform;
        Lot l = lotObject.AddComponent<Lot>();
        PruceduralRoad pr = lotObject.AddComponent<PruceduralRoad>();
        pr.SetProcedure(Apartments);
        l.SetConfluences(edgeCCs);
        l.SetPoints(points);
        l.SetHeight(cityManager.GetRoadNetwork().GetSideWalkHeight());
        lots.Add(l);
    }

    public ConfluenceController FindNextConfluenceCOntroller(ControllerPoint cp)
    {
        //ConfluenceController cc = null;
        bool additive = false;
        if(cp.GetID()>0 && cp.GetManager().GetControllerPoints()[cp.GetID()-1].IsOnConfluence())
        {
            additive = true;
        }
        else
        {
            additive = false;
        }
        //Debug.Log("Searching for next CC is Additive = " + additive);
        int index = cp.GetID();
        List<ControllerPoint> allpoints = cp.GetManager().GetControllerPoints();
        while (true)
        {
            if(additive && index < allpoints.Count-1)
            {
                index++;
                if (allpoints[index].IsOnConfluenceEdge())
                {
                    tempPrePointConfluence = allpoints[index];
                }
                if (allpoints[index].IsOnConfluence())
                    return cityManager.FindCunfluenceByPoint(allpoints[index]);
            }
            else if(!additive && index>0)
            {
                index--;
                if (allpoints[index].IsOnConfluenceEdge())
                {
                    tempPrePointConfluence = allpoints[index];
                }
                if (allpoints[index].IsOnConfluence())
                    return cityManager.FindCunfluenceByPoint(allpoints[index]);
            }
            else 
            {
                return null; 
            }
        }

    }
    public List<Vector3> GetPoints(ConfluenceController cc,ControllerPoint cp,bool isRight)
    {
        bool isPrePoint = cp.GetPostPoint().IsOnConfluence();
        bool isDefaulType;
        float angle = 10000;
        if (!cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetIsMainStreet())
            angle = cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetAngle();
        else
        {
            if(isRight && cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetLeft() != null)
                angle = cc.GetMultyRoadInfoController().GetEdgeInfo(cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetLeft()).GetAngle();
            else if(!isRight && cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetRight() != null)
                angle = cc.GetMultyRoadInfoController().GetEdgeInfo(cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetRight()).GetAngle();

        }

        if (isRight)
        {
            isDefaulType = cc.GetMultyRoadInfoController().GetEdgeInfo(cp).HasLeftContact;// : cc.GetMultyRoadInfoController().GetEdgeInfo(cp).HasLeftContact;
            if (!isDefaulType && (cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetLeft() == null || angle < 5))
                isDefaulType = true;
        }
        else 
        {
            isDefaulType = cc.GetMultyRoadInfoController().GetEdgeInfo(cp).HasRightContact;// : cc.GetMultyRoadInfoController().GetEdgeInfo(cp).HasRightContact;
            if(!isDefaulType && (cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetRight()==null || angle < 5))
                isDefaulType = true;
        }
        List<Vector3> points = new List<Vector3>();
        if(!isPrePoint)//post point
        {
            if(isRight)
            {
                if (!isDefaulType)
                {
                    ControllerPoint otherpoint = cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetLeft();
                    List<ConfluenceSideWalkHelper> helpers = cc.GetComponent<ConfluenceSideWalkController>().GetHelpers();
                    ConfluenceSideWalkHelper helper = null;
                    for (int i = 0; i < helpers.Count; i++)
                    {
                        if (helpers[i].CheckEquality(cp, otherpoint))
                        {
                            helper = helpers[i];
                            break;
                        }
                    }
                    if (helper.GetMainPoint() == cp)
                        points.Add(helper.GetObject().GetComponent<ConfluenceSideWalk>().GetEndVertices()[0]);
                    else
                        points.Add(helper.GetObject().GetComponent<ConfluenceSideWalk>().GetEndVertices()[1]);
                }
                Vector3 point1 = cp.GetManager().GetController().GetSideWalkManager().GetRightSideWalks()[cp.GetID()].GetStartVertices()[1];
                Vector3 point2 = cp.GetManager().GetController().GetSideWalkManager().GetRightSideWalks()[cp.GetID()].GetEndVertices()[1];
                point1 = cp.GetManager().GetController().GetSideWalkManager().GetRightSideWalks()[cp.GetID()].transform.TransformPoint(point1);
                point2 = cp.GetManager().GetController().GetSideWalkManager().GetRightSideWalks()[cp.GetID()].transform.TransformPoint(point2);
                points.Add(point1);
                points.Add(point2);
                
                
            }
            else
            {
                if (!isDefaulType)
                {
                    ControllerPoint otherpoint = cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetRight();
                    List<ConfluenceSideWalkHelper> helpers = cc.GetComponent<ConfluenceSideWalkController>().GetHelpers();
                    ConfluenceSideWalkHelper helper = null;
                    for (int i = 0; i < helpers.Count; i++)
                    {
                        if (helpers[i].CheckEquality(cp, otherpoint))
                        {
                            helper = helpers[i];
                            break;
                        }
                    }
                    if (helper.GetMainPoint() == cp)
                        points.Add(helper.GetObject().GetComponent<ConfluenceSideWalk>().GetEndVertices()[0]);
                    else
                        points.Add(helper.GetObject().GetComponent<ConfluenceSideWalk>().GetEndVertices()[1]);
                }
                Vector3 point1 = cp.GetManager().GetController().GetSideWalkManager().GetLeftSideWalks()[cp.GetID()].GetStartVertices()[0];
                Vector3 point2 = cp.GetManager().GetController().GetSideWalkManager().GetLeftSideWalks()[cp.GetID()].GetEndVertices()[0];
                point1 = cp.GetManager().GetController().GetSideWalkManager().GetLeftSideWalks()[cp.GetID()].transform.TransformPoint(point1);
                point2 = cp.GetManager().GetController().GetSideWalkManager().GetLeftSideWalks()[cp.GetID()].transform.TransformPoint(point2);
                points.Add(point1);
                points.Add(point2);
            }
        }
        else// pre point
        {
            if (isRight)
            {
                if (!isDefaulType)
                {
                    ControllerPoint otherpoint = cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetLeft();
                    List<ConfluenceSideWalkHelper> helpers = cc.GetComponent<ConfluenceSideWalkController>().GetHelpers();
                    ConfluenceSideWalkHelper helper = null;
                    for (int i = 0; i < helpers.Count; i++)
                    {
                        if (helpers[i].CheckEquality(cp, otherpoint))
                        {
                            helper = helpers[i];
                            break;
                        }
                    }
                    if (helper.GetMainPoint() == cp)
                        points.Add(helper.GetObject().GetComponent<ConfluenceSideWalk>().GetEndVertices()[1]);
                    else
                        points.Add(helper.GetObject().GetComponent<ConfluenceSideWalk>().GetEndVertices()[0]);
                }
                Vector3 point1 = cp.GetManager().GetController().GetSideWalkManager().GetLeftSideWalks()[cp.GetID()-1].GetStartVertices()[0];
                Vector3 point2 = cp.GetManager().GetController().GetSideWalkManager().GetLeftSideWalks()[cp.GetID()-1].GetEndVertices()[0];
                point1 = cp.GetManager().GetController().GetSideWalkManager().GetLeftSideWalks()[cp.GetID()-1].transform.TransformPoint(point1);
                point2 = cp.GetManager().GetController().GetSideWalkManager().GetLeftSideWalks()[cp.GetID() - 1].transform.TransformPoint(point2);
                points.Add(point1);
                points.Add(point2);

                
            }
            else
            {
                if (!isDefaulType)
                {
                    ControllerPoint otherpoint = cc.GetMultyRoadInfoController().GetEdgeInfo(cp).GetRight();
                    List<ConfluenceSideWalkHelper> helpers = cc.GetComponent<ConfluenceSideWalkController>().GetHelpers();
                    ConfluenceSideWalkHelper helper = null;
                    for (int i = 0; i < helpers.Count; i++)
                    {
                        if (helpers[i].CheckEquality(cp, otherpoint))
                        {
                            helper = helpers[i];
                            break;
                        }
                    }
                    if (helper.GetMainPoint() == cp)
                        points.Add(helper.GetObject().GetComponent<ConfluenceSideWalk>().GetEndVertices()[0]);
                    else
                        points.Add(helper.GetObject().GetComponent<ConfluenceSideWalk>().GetEndVertices()[1]);
                }
                Vector3 point1 = cp.GetManager().GetController().GetSideWalkManager().GetRightSideWalks()[cp.GetID() - 1].GetStartVertices()[1];
                Vector3 point2 = cp.GetManager().GetController().GetSideWalkManager().GetRightSideWalks()[cp.GetID() - 1].GetEndVertices()[1];
                point1 = cp.GetManager().GetController().GetSideWalkManager().GetRightSideWalks()[cp.GetID() - 1].transform.TransformPoint(point1);
                point2 = cp.GetManager().GetController().GetSideWalkManager().GetRightSideWalks()[cp.GetID() - 1].transform.TransformPoint(point2);
                points.Add(point1);
                points.Add(point2);

                
            }
        }
        //Debug.Log("Founded points = "+points.Count);
        return points;
    }
}