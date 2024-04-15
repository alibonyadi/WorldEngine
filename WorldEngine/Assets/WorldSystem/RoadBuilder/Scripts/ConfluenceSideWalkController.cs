using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class ConfluenceSideWalkController : MonoBehaviour
{
    [SerializeField]
    private ConfluenceController controller;
    [SerializeField]
    private MultyRoadConfluence info;
    [SerializeField]
    private bool isRightOfMainEmpty = false;
    [SerializeField]
    private bool isLeftOfMainEmpty = false;
    [SerializeField]
    private bool isBackOfMainEmpty = false;
    [SerializeField]
    private bool isFrontOfMainEmpty = false;
    [SerializeField]
    private float angleMultyplayer = 3;
    [SerializeField]
    private List<ConfluenceSideWalkHelper> helpers = new List<ConfluenceSideWalkHelper>();
    [SerializeField]
    private List<ConfluenceSideWalkCrossHelper> CrossHelpers = new List<ConfluenceSideWalkCrossHelper>();
    
    public bool IsRightOfMainEmpty() => isRightOfMainEmpty;
    public bool IsLeftOfMainEmpty() => isLeftOfMainEmpty;

    public void CheckAllSidesHasStreet()
    {
        //Debug.Log("right Of Main Is Empty = "+isRightOfMainEmpty);
        if (isLeftOfMainEmpty)
        {
            controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()].SetEmptySide(true);
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()].GetComponent<MeshFilter>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()].gameObject.AddComponent<MeshFilter>();
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()].GetComponent<PruceduralRoad>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()].gameObject.AddComponent<PruceduralRoad>();
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()].GetComponent<MeshRenderer>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()].gameObject.AddComponent<MeshRenderer>();
            /*int index1 = controller.GetPrePoint().GetID();
            float length = Vector3.Magnitude(controller.GetStreet().GetLineManager().Lines[index1].GetStartPoint().transform.position - controller.GetStreet().GetLineManager().Lines[index1].GetEndPoint().transform.position);
            controller.GetStreet().GetComponent<SideWalkManager>().GenerateBaseMesh(controller.GetPrePoint().GetID(), length, false,true,false);*/

            controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()+1].SetEmptySide(true);
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()+1].GetComponent<MeshFilter>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()+1].gameObject.AddComponent<MeshFilter>();
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID() + 1].GetComponent<PruceduralRoad>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID() + 1].gameObject.AddComponent<PruceduralRoad>();
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()+1].GetComponent<MeshRenderer>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()+1].gameObject.AddComponent<MeshRenderer>();
            /*index1 = controller.GetPrePoint().GetID()+1;
            length = Vector3.Magnitude(controller.GetStreet().GetLineManager().Lines[index1].GetStartPoint().transform.position - controller.GetStreet().GetLineManager().Lines[index1].GetEndPoint().transform.position);
            controller.GetStreet().GetComponent<SideWalkManager>().GenerateBaseMesh(index1, length, false,true,false);*/

            //controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID()].SetEmptySide(false);
            //controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() + 1].SetEmptySide(false);
        }
        else
        {
            controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()].SetEmptySide(false);
            controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID() + 1].SetEmptySide(false);
        }
        if(isRightOfMainEmpty)
        {
            //Debug.Log("right is empty On "+ controller.GetStreet().name + " Line Id : "+ controller.GetPrePoint().GetID());
            //controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID()].SetEmptySide(false);
            //controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID() + 1].SetEmptySide(false);

            controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID()].SetEmptySide(true);
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID()].GetComponent<MeshFilter>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID()].gameObject.AddComponent<MeshFilter>();
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID()].GetComponent<PruceduralRoad>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID()].gameObject.AddComponent<PruceduralRoad>();
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID()].GetComponent<MeshRenderer>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID()].gameObject.AddComponent<MeshRenderer>();
            /*int index1 = controller.GetPrePoint().GetID();
            float length = Vector3.Magnitude(controller.GetStreet().GetLineManager().Lines[index1].GetStartPoint().transform.position - controller.GetStreet().GetLineManager().Lines[index1].GetEndPoint().transform.position);
            controller.GetStreet().GetComponent<SideWalkManager>().GenerateBaseMesh(index1, length, false, false, true);*/

            controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() + 1].SetEmptySide(true);
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() + 1].GetComponent<MeshFilter>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() + 1].gameObject.AddComponent<MeshFilter>();
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() +1].GetComponent<PruceduralRoad>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() + 1].gameObject.AddComponent<PruceduralRoad>();
            if (controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() + 1].GetComponent<MeshRenderer>() == null)
                controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() + 1].gameObject.AddComponent<MeshRenderer>();
            /*index1 = controller.GetPrePoint().GetID()+1;
            length = Vector3.Magnitude(controller.GetStreet().GetLineManager().Lines[index1].GetStartPoint().transform.position - controller.GetStreet().GetLineManager().Lines[index1].GetEndPoint().transform.position);
            controller.GetStreet().GetComponent<SideWalkManager>().GenerateBaseMesh(index1, length, false, false, true);*/
        }
        else
        {
            controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID()].SetEmptySide(false);
            controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID()+1].SetEmptySide(false);

        }
    }

    public void BuildConfluenceSideWalkBase()
    {
        //CheckAllSidesHasStreet();
        for (int i=0;i<helpers.Count;i++)
        {
            if (helpers[i].GetObject()==null)
                helpers[i].SetObject(new GameObject("ConfluenceSideWalk"));

            helpers[i].GetObject().transform.parent = transform;
            ConfluenceSideWalk CSW = null;
            if (helpers[i].GetObject().GetComponent<ConfluenceSideWalk>() == null)
                CSW = helpers[i].GetObject().AddComponent<ConfluenceSideWalk>();
            else
                CSW = helpers[i].GetObject().GetComponent<ConfluenceSideWalk>();

            if (helpers[i].GetObject().GetComponent<PruceduralRoad>() == null)
                helpers[i].GetObject().AddComponent<PruceduralRoad>();

            Vector3 edgeMain = CalculateEdgePoint(helpers[i].GetMainPoint().transform, helpers[i].GetOtherPoint().transform);
            Vector3 edgeOther = CalculateEdgePoint(helpers[i].GetOtherPoint().transform, helpers[i].GetMainPoint().transform);
            helpers[i].GetObject().transform.position = edgeMain;
            helpers[i].GetObject().transform.LookAt(edgeOther);
            CSW.Initialize(helpers[i].GetMainPoint(), helpers[i].GetOtherPoint(),edgeMain,edgeOther,this);
            CSW.CreateBase();
        }
    }

    public void BuildConfluenceSideWalkCross()
    {
        for(int i=0;i<CrossHelpers.Count;i++)
        {
            if (CrossHelpers[i].GetObject() == null)
                CrossHelpers[i].SetObject(new GameObject("CrossSideWalk"+i));

            CrossHelpers[i].GetObject().transform.parent = transform;

            ConfluenceSideWalkCross CSWC = null;
            if (CrossHelpers[i].GetObject().GetComponent<ConfluenceSideWalkCross>() == null)
                CSWC = CrossHelpers[i].GetObject().AddComponent<ConfluenceSideWalkCross>();
            else
                CSWC = CrossHelpers[i].GetObject().GetComponent<ConfluenceSideWalkCross>();

            if (CrossHelpers[i].GetObject().GetComponent<PruceduralRoad>() == null)
                CrossHelpers[i].GetObject().AddComponent<PruceduralRoad>();

            Vector3 edgeJucntionOther = CalculateEdgePoint(CrossHelpers[i].GetOtherPoint().transform, CrossHelpers[i].GetMainPoint().transform);
            CrossHelpers[i].GetObject().transform.position = edgeJucntionOther;
            CSWC.Initialize(CrossHelpers[i].GetMainPoint(), CrossHelpers[i].GetOtherPoint(), edgeJucntionOther, CrossHelpers[i].GetIsDefault(),this);
            CSWC.CreateBase();
        }
    }

    public void CheckNewEntry()
    {

    }

    public void SetPrucedure(UnityEngine.Object p)
    {
        for(int i=0;i<helpers.Count;i++)
            helpers[i].GetObject().GetComponent<PruceduralRoad>().SetProcedure(p);

        for (int i = 0; i < CrossHelpers.Count; i++)
            CrossHelpers[i].GetObject().GetComponent<PruceduralRoad>().SetProcedure(p);
    }

    public void SetJunctionPrucedure(UnityEngine.Object p)
    {
        for (int i = 0; i < CrossHelpers.Count; i++)
            CrossHelpers[i].GetObject().GetComponent<ConfluenceSideWalkCross>().SetJunctionPrucedure(p);
    }

    public void GeneratePrucedure()
    {
        for(int i=0;i<helpers.Count;i++)
            helpers[i].GetObject().GetComponent<PruceduralRoad>().GenerateMeshes();

        for (int i = 0; i < CrossHelpers.Count; i++)
            CrossHelpers[i].GetObject().GetComponent<ConfluenceSideWalkCross>().GeneratePruceduralMesh();
        
    }    

    public void ClearHelper()
    {
        for(int i=0;i<helpers.Count;i++)
        {
            DestroyImmediate(helpers[i].GetObject());
        }

        helpers.Clear();
    }

    public List<ConfluenceSideWalkHelper> GetHelpers() => helpers;
    public void ClearCrossHelper()
    {
        for (int i = 0; i < CrossHelpers.Count; i++)
        {
            DestroyImmediate(CrossHelpers[i].GetObject());
            DestroyImmediate(CrossHelpers[i].GetJunctionObject());
        }

        CrossHelpers.Clear();
    }

    public void AddHelper(ConfluenceSideWalkHelper h)
    {
        for(int i=0;i<helpers.Count;i++)
        {
            if (helpers[i].GetMainPoint() == h.GetMainPoint() && helpers[i].GetOtherPoint() == h.GetOtherPoint())
            {
                if (helpers[i].GetObject()==null)
                {
                    GameObject o = new GameObject("ConfluenceSideWalk");
                    o.transform.parent = transform;
                    o.AddComponent<ConfluenceSideWalk>();
                    o.AddComponent<PruceduralRoad>();
                    helpers[i].SetObject(o);
                }
                return;
            }
        }
        GameObject obj = new GameObject("ConfluenceSideWalk");
        obj.transform.parent = transform;
        obj.AddComponent<ConfluenceSideWalk>();
        obj.AddComponent<PruceduralRoad>();
        h.SetObject(obj);
        helpers.Add(h);
    }

    public void AddCrossHelper(ConfluenceSideWalkCrossHelper h)
    {
        for (int i = 0; i < CrossHelpers.Count; i++)
        {
            if ((CrossHelpers[i].GetMainPoint() == h.GetMainPoint() && CrossHelpers[i].GetOtherPoint() == h.GetOtherPoint()) || (CrossHelpers[i].GetMainPoint() == h.GetOtherPoint() && CrossHelpers[i].GetOtherPoint() == h.GetMainPoint()) )
            {
                if (CrossHelpers[i].GetObject()==null)
                {
                    GameObject o = new GameObject("ConfluenceSideWalkCross"+i);
                    o.transform.parent = transform;
                    o.AddComponent<ConfluenceSideWalkCross>();
                    o.AddComponent<PruceduralRoad>();
                    CrossHelpers[i].SetObject(o);
                }
                if (CrossHelpers[i].GetJunctionObject()==null)
                {
                    GameObject o = new GameObject("Junction" + i);
                    o.transform.parent = transform;
                    o.AddComponent<Junction>();
                    o.AddComponent<PruceduralRoad>();
                    CrossHelpers[i].GetObject().GetComponent< ConfluenceSideWalkCross >().SetJunctionObject(o);
                    CrossHelpers[i].SetJunctionObject(o);
                }
                return;
            }
        }
        GameObject obj = new GameObject("ConfluenceSideWalkCross"+ CrossHelpers.Count);
        obj.transform.parent = transform;
        ConfluenceSideWalkCross CSWC = obj.AddComponent<ConfluenceSideWalkCross>();
        obj.AddComponent<PruceduralRoad>();
        GameObject junc = new GameObject("junction"+ CrossHelpers.Count);
        junc.transform.parent = transform;
        junc.AddComponent<Junction>();
        junc.AddComponent<PruceduralRoad>();
        CSWC.SetJunctionObject(junc);
        h.SetJunctionObject(junc);
        h.SetObject(obj);
        CrossHelpers.Add(h);
    }

    public ConfluenceSideWalk GetConfluenceSideWalk(ControllerPoint cp)
    {
        for(int i=0;i<helpers.Count;i++)
        {
            if (helpers[i].GetMainPoint() == cp)
                return helpers[i].GetObject().GetComponent<ConfluenceSideWalk>();
        }

        return null;
    }

    public void RecalculateSideWalksForConfluence(List<Confluence> confluences)
    {
        if (controller == null)
            controller = GetComponent<ConfluenceController>();

        ClearHelper();
        ClearCrossHelper();

        if (!GetInfo())
        {
            Debug.LogWarning("Can't Get Info!!!");
            return;
        } 

        RecalculateMainStreetSideWalks(angleMultyplayer);
        CheckAllSidesHasStreet();
        //Other Streets
        for (int i = 0; i < confluences.Count; i++)
        {
            if (confluences[i].GetPrePoint() != null)
            {
                if (info.GetEdgeInfo(confluences[i].GetPrePoint()).HasLeftContact || Mathf.Abs(info.GetEdgeInfo(confluences[i].GetPrePoint()).GetAngle()) < 5)
                {
                    ControllerPoint leftContactpoint = info.GetEdgeInfo(confluences[i].GetPrePoint()).GetLeft();
                    bool moveStartVertices = false;
                    float contactSize = 0;
                    if (info.GetEdgeInfo(leftContactpoint).GetIsPreEdge())
                    {
                        if (leftContactpoint.GetID() > 0)
                            contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[leftContactpoint.GetID() - 1].GetWidth();
                        else
                            contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightWidth();
                    }
                    else
                    {
                        if (leftContactpoint.GetID() < leftContactpoint.GetManager().GetControllerPoints().Count - 1)
                            contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[leftContactpoint.GetID()].GetWidth();
                    }
                    float angle = Mathf.Abs(info.GetEdgeInfo(confluences[i].GetPrePoint()).GetAngle());
                    float moveDistance = contactSize + (angleMultyplayer * (contactSize / 90) * angle);
                    //Debug.Log(angle + " <=angle -- moveDist => " + moveDistance + " -- size = " + contactSize);
                    ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(confluences[i].GetPrePoint(), info.GetEdgeInfo(confluences[i].GetPrePoint()).GetLeft(), true);
                    AddCrossHelper(crossHelper);
                    if (confluences[i].GetPrePoint().GetID() > 0)
                        confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[confluences[i].GetPrePoint().GetID() - 1].RecalculateSideWalks(moveStartVertices, moveDistance);
                }
                else if ((info.GetEdgeInfo(confluences[i].GetPrePoint()).IsRightOfMainStreet() && !isRightOfMainEmpty) || (!info.GetEdgeInfo(confluences[i].GetPrePoint()).IsRightOfMainStreet() && !isLeftOfMainEmpty))
                {
                    bool moveStartVertices = false;
                    bool mustMove = CalculateMustMove(confluences[i].GetPrePoint(), info.GetEdgeInfo(confluences[i].GetPrePoint()).GetLeft(), controller.transform);
                    if (!mustMove)
                    {
                        ConfluenceSideWalkHelper helper = CreateHelper(confluences[i].GetPrePoint(), info.GetEdgeInfo(confluences[i].GetPrePoint()).GetLeft());
                        AddHelper(helper);
                        ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(confluences[i].GetPrePoint(), info.GetEdgeInfo(confluences[i].GetPrePoint()).GetLeft(), false);
                        AddCrossHelper(crossHelper);

                        if (confluences[i].GetPrePoint().GetID() > 0)
                            confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[confluences[i].GetPrePoint().GetID() - 1].ResetForConfluence(moveStartVertices);
                    }
                    else
                    {
                        if (confluences[i].GetPrePoint().GetID() > 0)
                        {
                            float contactSize = confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[confluences[i].GetPrePoint().GetID() - 1].GetWidth();
                            confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[confluences[i].GetPrePoint().GetID() - 1].RecalculateSideWalks(moveStartVertices, contactSize);
                        }
                    }
                }


                if (info.GetEdgeInfo(confluences[i].GetPrePoint()).HasRightContact || Mathf.Abs(info.GetEdgeInfo(confluences[i].GetPrePoint()).GetAngle()) < 5)
                {
                    ControllerPoint rightContactpoint = info.GetEdgeInfo(confluences[i].GetPrePoint()).GetRight();
                    bool moveStartVertices = false;
                    float contactSize = 0;
                    if (info.GetEdgeInfo(rightContactpoint).GetIsPreEdge())
                    {
                        if (rightContactpoint.GetID() > 0)
                            contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[rightContactpoint.GetID() - 1].GetWidth();
                        else
                            contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftWidth();
                    }
                    else
                    {
                        if (rightContactpoint.GetID() < rightContactpoint.GetManager().GetControllerPoints().Count - 1)
                            contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[rightContactpoint.GetID()].GetWidth();
                    }
                    float angle = Mathf.Abs(info.GetEdgeInfo(confluences[i].GetPrePoint()).GetAngle());
                    float moveDistance = contactSize + (angleMultyplayer * (contactSize / 90) * angle);
                    ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(confluences[i].GetPrePoint(), info.GetEdgeInfo(confluences[i].GetPrePoint()).GetRight(), true);
                    AddCrossHelper(crossHelper);
                    if (confluences[i].GetPrePoint().GetID() > 0)
                        confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[confluences[i].GetPrePoint().GetID() - 1].RecalculateSideWalks(moveStartVertices, moveDistance);
                }
                else if ((info.GetEdgeInfo(confluences[i].GetPrePoint()).IsRightOfMainStreet() && !isRightOfMainEmpty) || (!info.GetEdgeInfo(confluences[i].GetPrePoint()).IsRightOfMainStreet() && !isLeftOfMainEmpty))
                {
                    bool moveStartVertices = false;
                    bool mustMove = CalculateMustMove(confluences[i].GetPrePoint(), info.GetEdgeInfo(confluences[i].GetPrePoint()).GetRight(), controller.transform);
                    if (!mustMove)
                    {
                        ConfluenceSideWalkHelper helper = CreateHelper(confluences[i].GetPrePoint(), info.GetEdgeInfo(confluences[i].GetPrePoint()).GetRight());
                        AddHelper(helper);
                        ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(confluences[i].GetPrePoint(), info.GetEdgeInfo(confluences[i].GetPrePoint()).GetRight(), false);
                        AddCrossHelper(crossHelper);

                        if (confluences[i].GetPrePoint().GetID() > 0)
                            confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[confluences[i].GetPrePoint().GetID() - 1].ResetForConfluence(moveStartVertices);
                    }
                    else
                    {
                        if (confluences[i].GetPrePoint().GetID() > 0)
                        {
                            float contactSize = confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[confluences[i].GetPrePoint().GetID() - 1].GetWidth();
                            confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[confluences[i].GetPrePoint().GetID() - 1].RecalculateSideWalks(moveStartVertices, contactSize);
                        }
                    }
                }
            }

            if (confluences[i].GetPostPoint() != null)
            {
                if (info.GetEdgeInfo(confluences[i].GetPostPoint()).HasLeftContact || Mathf.Abs(info.GetEdgeInfo(confluences[i].GetPostPoint()).GetAngle())<5)
                {
                    ControllerPoint leftContactpoint = info.GetEdgeInfo(confluences[i].GetPostPoint()).GetLeft();
                    bool moveStartVertices = true;
                    float contactSize = 0;
                    if (info.GetEdgeInfo(leftContactpoint).GetIsPreEdge())
                    {
                        if (leftContactpoint.GetID() > 0)
                            contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[leftContactpoint.GetID() - 1].GetWidth();
                        else
                            contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightWidth();
                    }
                    else
                    {
                        if (leftContactpoint.GetID() < leftContactpoint.GetManager().GetControllerPoints().Count - 1)
                            contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[leftContactpoint.GetID()].GetWidth();
                    }
                    float angle = Mathf.Abs(info.GetEdgeInfo(confluences[i].GetPostPoint()).GetAngle());
                    float moveDistance = contactSize + (angleMultyplayer * (contactSize / 90) * angle);
                    ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(confluences[i].GetPostPoint(), info.GetEdgeInfo(confluences[i].GetPostPoint()).GetLeft(), true);
                    AddCrossHelper(crossHelper);
                    if (confluences[i].GetPostPoint().GetID() < confluences[i].GetPoint().GetManager().GetControllerPoints().Count - 1)
                        confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[confluences[i].GetPostPoint().GetID()].RecalculateSideWalks(moveStartVertices, moveDistance);
                }
                else if ((info.GetEdgeInfo(confluences[i].GetPostPoint()).IsRightOfMainStreet() && !isRightOfMainEmpty) || (!info.GetEdgeInfo(confluences[i].GetPostPoint()).IsRightOfMainStreet() && !isLeftOfMainEmpty))
                {
                    bool moveStartVertices = true;
                    bool mustMove = CalculateMustMove(confluences[i].GetPostPoint(), info.GetEdgeInfo(confluences[i].GetPostPoint()).GetLeft(), controller.transform);
                    if (!mustMove)
                    {
                        ConfluenceSideWalkHelper helper = CreateHelper(confluences[i].GetPostPoint(), info.GetEdgeInfo(confluences[i].GetPostPoint()).GetLeft());
                        AddHelper(helper);
                        ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(confluences[i].GetPostPoint(), info.GetEdgeInfo(confluences[i].GetPostPoint()).GetLeft(), false);
                        AddCrossHelper(crossHelper);
                        if (confluences[i].GetPostPoint().GetID() < confluences[i].GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                            confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[confluences[i].GetPostPoint().GetID()].ResetForConfluence(moveStartVertices);
                    }
                    else
                    {
                        if (confluences[i].GetPostPoint().GetID() < confluences[i].GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                        {
                            float contactSize = confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[confluences[i].GetPostPoint().GetID()].GetWidth();
                            confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[confluences[i].GetPostPoint().GetID()].RecalculateSideWalks(moveStartVertices, contactSize);
                        }
                    }
                }

                if (info.GetEdgeInfo(confluences[i].GetPostPoint()).HasRightContact || Mathf.Abs(info.GetEdgeInfo(confluences[i].GetPostPoint()).GetAngle()) < 5)
                {
                    ControllerPoint rightContactpoint = info.GetEdgeInfo(confluences[i].GetPostPoint()).GetRight();
                    bool moveStartVertices = true;
                    float contactSize = 0;
                    if (info.GetEdgeInfo(rightContactpoint).GetIsPreEdge())
                    {
                        if (rightContactpoint.GetID() > 0)
                            contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[rightContactpoint.GetID() - 1].GetWidth();
                        else
                            contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftWidth();

                    }
                    else
                    {
                        if (rightContactpoint.GetID() < rightContactpoint.GetManager().GetControllerPoints().Count - 1)
                            contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[rightContactpoint.GetID()].GetWidth();
                    }
                    float angle = Mathf.Abs(info.GetEdgeInfo(confluences[i].GetPostPoint()).GetAngle());
                    float moveDistance = contactSize + (angleMultyplayer * ((contactSize / 90) * angle));
                    ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(confluences[i].GetPostPoint(), info.GetEdgeInfo(confluences[i].GetPostPoint()).GetRight(), true);
                    AddCrossHelper(crossHelper);
                    if (confluences[i].GetPostPoint().GetID() < confluences[i].GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                        confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[confluences[i].GetPostPoint().GetID()].RecalculateSideWalks(moveStartVertices, moveDistance);
                }
                else if ((info.GetEdgeInfo(confluences[i].GetPostPoint()).IsRightOfMainStreet() && !isRightOfMainEmpty) || (!info.GetEdgeInfo(confluences[i].GetPostPoint()).IsRightOfMainStreet() && !isLeftOfMainEmpty))
                {
                    bool moveStartVertices = true;
                    bool mustMove = CalculateMustMove(confluences[i].GetPostPoint(), info.GetEdgeInfo(confluences[i].GetPostPoint()).GetRight(), controller.transform);
                    if (!mustMove)
                    {
                        ConfluenceSideWalkHelper helper = CreateHelper(confluences[i].GetPostPoint(), info.GetEdgeInfo(confluences[i].GetPostPoint()).GetRight());
                        AddHelper(helper);
                        ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(confluences[i].GetPostPoint(), info.GetEdgeInfo(confluences[i].GetPostPoint()).GetRight(), false);
                        AddCrossHelper(crossHelper);

                        if (confluences[i].GetPostPoint().GetID() < confluences[i].GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                            confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[confluences[i].GetPostPoint().GetID()].ResetForConfluence(moveStartVertices);
                    }
                    else
                    {
                        if (confluences[i].GetPostPoint().GetID() < confluences[i].GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                        {
                            float contactSize = confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[confluences[i].GetPostPoint().GetID()].GetWidth();
                            confluences[i].GetPoint().GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[confluences[i].GetPostPoint().GetID()].RecalculateSideWalks(moveStartVertices, contactSize);
                        }
                    }
                }
            }
        }
    }

    private Vector3 CalculateEdgePoint(Transform point,Transform otherpoint)
    {
        float width = 0;
        if(point.GetComponent<Line>()!=null)
            width = point.GetComponent<Line>().GetWidth();
        else
            width = point.GetComponent<ControllerPoint>().GetManager().GetController().GetStreetWidth();
        if(point.InverseTransformPoint(otherpoint.position).x > 0)
        {
            return point.TransformPoint(new Vector3(width/2, 0, 0));
        }
        else
        {
            return point.TransformPoint(new Vector3(-width / 2, 0, 0));
        }
    }
    private bool CalculateMustMove(ControllerPoint point, ControllerPoint otherPoint, Transform baseRotation)
    {
        float angle = info.GetEdgeInfo(point).GetAngle();
        Vector3 EdgePoint = CalculateEdgePoint(point.transform, otherPoint.transform);
        Vector3 EdgeOtherPoint = CalculateEdgePoint(otherPoint.transform, point.transform);
        float otherAngle = info.GetEdgeInfo(otherPoint).GetAngle();
        Quaternion rotation = Quaternion.LookRotation(EdgePoint - EdgeOtherPoint, Vector3.up);
        //Debug.Log(rotation);
        float checkAngle = Mathf.Abs(Quaternion.Angle(rotation, baseRotation.rotation)-90);
        //Debug.Log(angle+"<=angle -- other=>"+otherAngle+" |-- checkAngle=>"+checkAngle+ " --EdgePoint= "+ EdgePoint+ " EdgeOtherPoint= "+ EdgeOtherPoint);
        //Debug.Log("Point = " + point.transform.position + "  -- otherPoint = " + otherPoint.transform.position);
        //Debug.Log(name);
        return CheckIsNearAngle(angle, otherAngle, checkAngle);
    }
    public static bool CheckIsNearAngle(float angle, float otherAngle, float checkNearAngle)
    {
        bool item1zero = angle == 0;
        bool item2zero = otherAngle == 0;

        float item1Angle = Mathf.Abs(angle);
        float item1Angle2 = Mathf.Abs(angle);
        if (item1zero)
            item1Angle2 = 90;

        float item2Angle = Mathf.Abs(otherAngle);
        float item2Angle2 = Mathf.Abs(otherAngle);
        if (item2zero)
            item2Angle2 = 90;

        float checkAngle = checkNearAngle;


        float diff1 = Mathf.Abs(item1Angle - checkAngle);
        float diff12 = Mathf.Abs(item1Angle2 - checkAngle);
        float diff2 = Mathf.Abs(item2Angle - checkAngle);
        float diff22 = Mathf.Abs(item2Angle2 - checkAngle);
        if(diff1 >= diff12)
            diff1 = diff12;

        if(diff2 >= diff22)
            diff2 = diff22;

        if (diff1 <= diff2)
            return false;
        else
            return true;
    }
    public ConfluenceController GetController() => controller;
    public ConfluenceSideWalkHelper CreateHelper(ControllerPoint point,ControllerPoint other)//,List<Vector3> startVecrtices,List<Vector3> endVertices)
    {
        ConfluenceSideWalkHelper helper = new ConfluenceSideWalkHelper();
        helper.SetPoint(point);
        helper.SetOtherPoint(other);
        //helper.SetStartVertices(startVecrtices);
        //helper.SetEndVertices(endVertices);
        return helper;
    }
    public ConfluenceSideWalkCrossHelper CreateCrossHelper(ControllerPoint point, ControllerPoint other, bool DefaultSideWalk)
    {
        ConfluenceSideWalkCrossHelper crossHelper = new ConfluenceSideWalkCrossHelper();
        crossHelper.SetPoint(point);
        crossHelper.SetOtherPoint(other);
        crossHelper.SetIsDefault(DefaultSideWalk);
        return crossHelper;
    }
    private bool GetInfo()
    {
        if (GetComponent<MultyRoadConfluence>() == null)
            return false;

        info = GetComponent<MultyRoadConfluence>();
        return true;
    }
    private void RecalculateMainStreetSideWalks(float angleMultyplayer)
    {
        if (controller.GetPrePoint() != null)
        {
            if (info.GetEdgeInfo(controller.GetPrePoint()).HasLeftContact ||(info.GetEdgeInfo(controller.GetPrePoint()).GetLeft() != null &&  Mathf.Abs(info.GetEdgeInfo(info.GetEdgeInfo(controller.GetPrePoint()).GetLeft()).GetAngle())<5) )
            {
                ControllerPoint leftContactpoint = info.GetEdgeInfo(controller.GetPrePoint()).GetLeft();
                bool moveStartVertices = false;
                float contactSize = 0;
                if (info.GetEdgeInfo(leftContactpoint).GetIsPreEdge())
                {
                    if (leftContactpoint.GetID() > 0)
                        contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[leftContactpoint.GetID() - 1].GetWidth();
                    else
                        contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightWidth();
                }
                else
                {
                    if (leftContactpoint.GetID() < leftContactpoint.GetManager().GetControllerPoints().Count - 1)
                        contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[leftContactpoint.GetID()].GetWidth();
                }
                float angle = Mathf.Abs(info.GetEdgeInfo(leftContactpoint).GetAngle());
                float moveDistance = contactSize + (angleMultyplayer * (contactSize / 90) * angle);
                ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(controller.GetPrePoint(), info.GetEdgeInfo(controller.GetPrePoint()).GetLeft(), true);
                AddCrossHelper(crossHelper);
                if (controller.GetPrePoint().GetID() > 0)
                    controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID() - 1].RecalculateSideWalks(moveStartVertices, moveDistance);
                isLeftOfMainEmpty = false;
            }
            else if (controller.GetPostPoint()!=null && info.GetEdgeInfo(controller.GetPostPoint()).HasRightContact)//must check **postpoint** for empty side
            {
                bool moveStartVertices = false;
                bool mustMove = CalculateMustMove(controller.GetPrePoint(), info.GetEdgeInfo(controller.GetPrePoint()).GetLeft(), controller.transform);
                //Debug.Log("Must Move ? " + mustMove);
                if (!mustMove)
                {
                    ConfluenceSideWalkHelper helper = CreateHelper(controller.GetPrePoint(), info.GetEdgeInfo(controller.GetPrePoint()).GetLeft());
                    AddHelper(helper);
                    ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(controller.GetPrePoint(), info.GetEdgeInfo(controller.GetPrePoint()).GetLeft(), false);
                    AddCrossHelper(crossHelper);
                    if (controller.GetPrePoint().GetID() > 0)
                        controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID() - 1].ResetForConfluence(moveStartVertices);
                }
                else
                {
                    float contactSize = 0;
                    if (controller.GetPrePoint().GetID() > 0)
                    {
                        contactSize = controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID() - 1].GetWidth();
                        controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPrePoint().GetID() - 1].RecalculateSideWalks(moveStartVertices, contactSize);
                    }
                    //else
                    //     controller.GetStreet().GetComponent<SideWalkManager>().GetLeftWidth();
                }
                isLeftOfMainEmpty = false;
            }
            else
            {
                isLeftOfMainEmpty = true;
            }


            if (info.GetEdgeInfo(controller.GetPrePoint()).HasRightContact || (info.GetEdgeInfo(controller.GetPrePoint()).GetRight() != null && Mathf.Abs(info.GetEdgeInfo(info.GetEdgeInfo(controller.GetPrePoint()).GetRight()).GetAngle()) < 5))
            {
                ControllerPoint rightContactpoint = info.GetEdgeInfo(controller.GetPrePoint()).GetRight();
                bool moveStartVertices = false;
                float contactSize = 0;
                if (info.GetEdgeInfo(rightContactpoint).GetIsPreEdge())
                {
                    if (rightContactpoint.GetID() > 0)
                        contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[rightContactpoint.GetID() - 1].GetWidth();
                    else
                        contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftWidth();

                }
                else
                {
                    if (rightContactpoint.GetID() < rightContactpoint.GetManager().GetControllerPoints().Count - 1)
                        contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[rightContactpoint.GetID()].GetWidth();
                }
                float angle = Mathf.Abs(info.GetEdgeInfo(rightContactpoint).GetAngle());

                float moveDistance = contactSize + (angleMultyplayer * (contactSize / 90) * angle);
                ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(controller.GetPrePoint(), info.GetEdgeInfo(controller.GetPrePoint()).GetRight(), true);
                AddCrossHelper(crossHelper);
                if (controller.GetPrePoint().GetID() > 0)
                    controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() - 1].RecalculateSideWalks(moveStartVertices, moveDistance);
                isRightOfMainEmpty = false;
            }
            else if (info.GetEdgeInfo(controller.GetPostPoint()).HasLeftContact)
            {
                bool moveStartVertices = false;
                bool mustMove = CalculateMustMove(controller.GetPrePoint(), info.GetEdgeInfo(controller.GetPrePoint()).GetRight(), controller.transform);
                if (!mustMove)
                {
                    ConfluenceSideWalkHelper helper = CreateHelper(controller.GetPrePoint(), info.GetEdgeInfo(controller.GetPrePoint()).GetRight());
                    AddHelper(helper);
                    ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(controller.GetPrePoint(), info.GetEdgeInfo(controller.GetPrePoint()).GetRight(), false);
                    AddCrossHelper(crossHelper);
                    if (controller.GetPrePoint().GetID() > 0)
                        controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() - 1].ResetForConfluence(moveStartVertices);
                }
                else
                {
                    float contactSize = 0;
                    if (controller.GetPrePoint().GetID() > 0)
                    {
                        contactSize = controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() - 1].GetWidth();
                        controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPrePoint().GetID() - 1].RecalculateSideWalks(moveStartVertices, contactSize);

                    }
                    else
                        contactSize = controller.GetStreet().GetComponent<SideWalkManager>().GetRightWidth();
                }
                isRightOfMainEmpty = false;
            }
            else
                isRightOfMainEmpty = true;


            isBackOfMainEmpty = false;
        }
        else
        {
            isBackOfMainEmpty = true;
        }


        if (controller.GetPostPoint() != null)
        {
            if (!isRightOfMainEmpty && ( info.GetEdgeInfo(controller.GetPostPoint()).HasLeftContact || Mathf.Abs(info.GetEdgeInfo(info.GetEdgeInfo(controller.GetPostPoint()).GetLeft()).GetAngle()) < 5 )  )
            {
                ControllerPoint leftContactpoint = info.GetEdgeInfo(controller.GetPostPoint()).GetLeft();
                bool moveStartVertices = true;
                float contactSize = 0;
                if (info.GetEdgeInfo(leftContactpoint).GetIsPreEdge())
                {
                    if (leftContactpoint.GetID() > 0)
                        contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[leftContactpoint.GetID() - 1].GetWidth();
                    else
                        contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightWidth();

                }
                else
                {
                    if (leftContactpoint.GetID() < leftContactpoint.GetManager().GetControllerPoints().Count - 1)
                        contactSize = leftContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[leftContactpoint.GetID()].GetWidth();
                }
                float angle = Mathf.Abs(info.GetEdgeInfo(leftContactpoint).GetAngle());
                float moveDistance = contactSize + (angleMultyplayer * (contactSize / 90) * angle);
                ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(controller.GetPostPoint(), info.GetEdgeInfo(controller.GetPostPoint()).GetLeft(), true);
                AddCrossHelper(crossHelper);
                if (controller.GetPostPoint().GetID() < controller.GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                    controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPostPoint().GetID()].RecalculateSideWalks(moveStartVertices, moveDistance);
                //isLeftOfMainEmpty = false;

                //controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPostPoint().GetID()].RecalculateSideWalks();
            }
            else if (!isRightOfMainEmpty)
            {
                bool moveStartVertices = true;
                bool mustMove = CalculateMustMove(controller.GetPostPoint(), info.GetEdgeInfo(controller.GetPostPoint()).GetLeft(), controller.transform);
                if (!mustMove)
                {
                    ConfluenceSideWalkHelper helper = CreateHelper(controller.GetPostPoint(), info.GetEdgeInfo(controller.GetPostPoint()).GetLeft());
                    AddHelper(helper);
                    ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(controller.GetPostPoint(), info.GetEdgeInfo(controller.GetPostPoint()).GetLeft(), false);
                    AddCrossHelper(crossHelper);
                    if (controller.GetPostPoint().GetID() < controller.GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                        controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPostPoint().GetID()].ResetForConfluence(moveStartVertices);
                }
                else
                {
                    if (controller.GetPostPoint().GetID() < controller.GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                    {
                        float contactSize = controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPostPoint().GetID()].GetWidth();
                        controller.GetStreet().GetComponent<SideWalkManager>().GetRightSideWalks()[controller.GetPostPoint().GetID()].RecalculateSideWalks(moveStartVertices, contactSize);
                    }
                }
            }

            if (!isLeftOfMainEmpty &&( info.GetEdgeInfo(controller.GetPostPoint()).HasRightContact || Mathf.Abs(info.GetEdgeInfo(info.GetEdgeInfo(controller.GetPostPoint()).GetRight()).GetAngle()) < 5))
            {
                ControllerPoint rightContactpoint = info.GetEdgeInfo(controller.GetPostPoint()).GetRight();
                bool moveStartVertices = true;
                float contactSize = 0;
                if (info.GetEdgeInfo(rightContactpoint).GetIsPreEdge())
                {
                    if (rightContactpoint.GetID() > 0)
                        contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[rightContactpoint.GetID() - 1].GetWidth();
                    else
                        contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftWidth();

                }
                else
                {
                    if (rightContactpoint.GetID() < rightContactpoint.GetManager().GetControllerPoints().Count - 1)
                        contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[rightContactpoint.GetID()].GetWidth();
                    else
                        contactSize = rightContactpoint.GetManager().GetController().GetComponent<SideWalkManager>().GetRightWidth();
                }
                float angle = Mathf.Abs(info.GetEdgeInfo(rightContactpoint).GetAngle());
                float moveDistance = contactSize + (angleMultyplayer * (contactSize / 90) * angle);
                ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(controller.GetPostPoint(), info.GetEdgeInfo(controller.GetPostPoint()).GetRight(), true);
                AddCrossHelper(crossHelper);
                if (controller.GetPostPoint().GetID() < controller.GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                    controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPostPoint().GetID()].RecalculateSideWalks(moveStartVertices, moveDistance);
                //isRightOfMainEmpty = false;

                //controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPostPoint().GetID()].RecalculateSideWalks();
            }
            else if (!isLeftOfMainEmpty)
            {
                bool moveStartVertices = true;
                bool mustMove = CalculateMustMove(controller.GetPostPoint(), info.GetEdgeInfo(controller.GetPostPoint()).GetRight(), controller.transform);
                if (!mustMove)
                {
                    ConfluenceSideWalkHelper helper = CreateHelper(controller.GetPostPoint(), info.GetEdgeInfo(controller.GetPostPoint()).GetRight());
                    AddHelper(helper);
                    ConfluenceSideWalkCrossHelper crossHelper = CreateCrossHelper(controller.GetPostPoint(), info.GetEdgeInfo(controller.GetPostPoint()).GetRight(), false);
                    AddCrossHelper(crossHelper);

                    if (controller.GetPostPoint().GetID() < controller.GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                        controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPostPoint().GetID()].ResetForConfluence(moveStartVertices);
                }
                else
                {

                    if (controller.GetPostPoint().GetID() < controller.GetPostPoint().GetManager().GetControllerPoints().Count - 1)
                    {
                        float contactSize = controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPostPoint().GetID()].GetWidth();
                        controller.GetStreet().GetComponent<SideWalkManager>().GetLeftSideWalks()[controller.GetPostPoint().GetID()].RecalculateSideWalks(moveStartVertices, contactSize);
                    }
                }
            }
            isFrontOfMainEmpty = false;
        }
        else
        {
            isFrontOfMainEmpty = true;
        }
    }
}
