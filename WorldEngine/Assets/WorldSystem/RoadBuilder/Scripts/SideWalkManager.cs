using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SideWalkManager : MonoBehaviour
{
    [SerializeField]
    private float leftWidth=5;
    [SerializeField]
    private float rightWidth=5;
    [SerializeField]
    private float height = 0.3f;
    [SerializeField]
    private bool hasLeft = true;
    [SerializeField]
    private bool hasRight = true;
    [SerializeField]
    private List<SideWalk> leftSideWalks = new List<SideWalk>();
    [SerializeField]
    private List<SideWalk> rightSideWalks = new List<SideWalk>();
    [SerializeField]
    private StreetController streetController;
    [SerializeField]
    private UnityEngine.Object procedure;
    [SerializeField]
    private List<Vector3> tempLeftFrontVertices = new List<Vector3>();
    [SerializeField]
    private List<Vector3> tempRightFrontVertices = new List<Vector3>();

    public float GetLeftWidth()=> leftWidth;
    public float GetRightWidth()=> rightWidth;
    public List<SideWalk> GetLeftSideWalks() => leftSideWalks;
    public List<SideWalk> GetRightSideWalks() => rightSideWalks;
    public void SetPrucedure(UnityEngine.Object p)
    {
        for(int i=0;i<leftSideWalks.Count;i++)
        {
            leftSideWalks[i].SetPrucedure(p);
            if (leftSideWalks[i].GetComponent<PruceduralRoad>())
                leftSideWalks[i].GetComponent<PruceduralRoad>().SetProcedure(p);
        }

        for (int i = 0; i < rightSideWalks.Count; i++)
        {
            rightSideWalks[i].SetPrucedure(p);
            if (rightSideWalks[i].GetComponent<PruceduralRoad>())
                rightSideWalks[i].GetComponent<PruceduralRoad>().SetProcedure(p);
        }
    }
    public void SetTempFrontVertices(Vector3 v1, Vector3 v2,bool isRight)
    {
        if(isRight)
        {
            tempRightFrontVertices.Clear();
            tempRightFrontVertices.Add(v1);
            tempRightFrontVertices.Add(v2);
        }
        else
        {
            tempLeftFrontVertices.Clear();
            tempLeftFrontVertices.Add(v1);
            tempLeftFrontVertices.Add(v2);
        }
    }
    public void GeneratePrucedure()
    {
        for (int i = 0; i < leftSideWalks.Count; i++)
        {
            if (leftSideWalks[i].GetComponent<PruceduralRoad>())
                leftSideWalks[i].GetComponent<PruceduralRoad>().GenerateMeshes();
        }

        for (int i = 0; i < rightSideWalks.Count; i++)
        {
            if (rightSideWalks[i].GetComponent<PruceduralRoad>())
                rightSideWalks[i].GetComponent<PruceduralRoad>().GenerateMeshes();
        }
    }

    public void GenerateBaseMesh(int index,float length, bool isFirst,bool createLeft,bool createRight)
    {
        if (isFirst)
        {
            if(createLeft)
                leftSideWalks[index].GenerateBaseMesh(length);
            if(createRight)
                rightSideWalks[index].GenerateBaseMesh(length);
        }
        else
        {
            if(createLeft)
                leftSideWalks[index].GenerateBaseMesh(tempLeftFrontVertices, length);
            if(createRight)
                rightSideWalks[index].GenerateBaseMesh(tempRightFrontVertices, length);
        }
    }

    public void RemoveMeshProcedure(int index)
    {
        if (!leftSideWalks[index].GetEmptySide())
        {
            if (leftSideWalks[index].GetComponent<MeshFilter>())
                leftSideWalks[index].GetComponent<MeshFilter>().mesh = null;
            if (leftSideWalks[index].GetComponent<PruceduralRoad>())
                DestroyImmediate(leftSideWalks[index].GetComponent<PruceduralRoad>());
        }

        if (!rightSideWalks[index].GetEmptySide())
        {
            if (rightSideWalks[index].GetComponent<MeshFilter>())
                rightSideWalks[index].GetComponent<MeshFilter>().mesh = null;
            if (rightSideWalks[index].GetComponent<PruceduralRoad>())
                DestroyImmediate(rightSideWalks[index].GetComponent<PruceduralRoad>());
        }
    }

    public void ClearLists()
    {
        leftSideWalks.Clear();
        rightSideWalks.Clear();
    }

    public void RecalculateSideWalks()
    {

    }

    public void CreateSideWalkObjects(Line myline)
    {
        if (myline.gameObject.GetComponentInChildren<SideWalk>() != null)
        {
            List<SideWalk> sideWalks = myline.gameObject.GetComponentsInChildren<SideWalk>().ToList();
            for(int i = 0; i < sideWalks.Count;i++)
            {
                if (sideWalks[i].GetIsRight())
                    rightSideWalks.Add(sideWalks[i]);
                else
                    leftSideWalks.Add(sideWalks[i]);
            }
        }
        else
        {
            GameObject leftsidewalk = new GameObject("LeftSideWalk");
            leftsidewalk.transform.parent = myline.transform;
            leftsidewalk.transform.position = myline.transform.position;
            leftsidewalk.transform.rotation = myline.transform.rotation;
            leftsidewalk.transform.Translate(-(myline.GetWidth() / 2 + leftWidth / 2), 0, 0);
            SideWalk leftSW = leftsidewalk.AddComponent<SideWalk>();
            PruceduralRoad lPR = leftsidewalk.AddComponent<PruceduralRoad>();
            leftSW.SetLine(myline);
            leftSW.SetWidth(leftWidth);
            leftSW.SetManager(this);
            leftSW.SetHeight(height);
            leftSW.SetIsRight(false);
            leftSideWalks.Add(leftSW);

            GameObject rightsidewalk = new GameObject("RightSideWalk");
            rightsidewalk.transform.parent = myline.transform;
            rightsidewalk.transform.position = myline.transform.position;
            rightsidewalk.transform.rotation = myline.transform.rotation;
            rightsidewalk.transform.Translate(myline.GetWidth() / 2 + rightWidth / 2, 0, 0);
            SideWalk rightSW = rightsidewalk.AddComponent<SideWalk>();
            PruceduralRoad rPR = rightsidewalk.AddComponent<PruceduralRoad>();
            rightSW.SetLine(myline);
            rightSW.SetWidth(rightWidth);
            rightSW.SetManager(this);
            rightSW.SetHeight(height);
            rightSW.SetIsRight(true);
            rightSideWalks.Add(rightSW);
        }

        
    }
}
