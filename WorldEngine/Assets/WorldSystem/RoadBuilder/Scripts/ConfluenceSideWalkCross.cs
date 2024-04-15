using System.Collections.Generic;
using UnityEngine;

public class ConfluenceSideWalkCross : MonoBehaviour
{
    [SerializeField]
    private GameObject junctionObj;
    [SerializeField]
    Vector3 EdgeJunction;
    [SerializeField]
    ControllerPoint point;
    [SerializeField]
    ControllerPoint other;
    [SerializeField]
    bool isDefault = false;
    [SerializeField]
    bool isRight = false;
    [SerializeField]
    ConfluenceSideWalkController controller;
    [SerializeField]
    List<Vector3> startVertices = new List<Vector3>();
    [SerializeField]
    List<Vector3> endVertices = new List<Vector3>();

    public void Initialize(ControllerPoint mainPoint, ControllerPoint otherPoint, Vector3 edgejunction, bool isDefault, ConfluenceSideWalkController c)
    {
        EdgeJunction = edgejunction;
        point = mainPoint;
        other = otherPoint;
        this.isDefault = isDefault;
        controller = c;
    }

    public void SetJunctionObject(GameObject j)=> junctionObj = j;
    public GameObject GetJunctionObject() => junctionObj;

    public bool CheckIsRightOfPoint(Transform point,Transform other)
    {
        if (point.InverseTransformPoint(other.position).x > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GeneratePruceduralMesh()
    {
        GetComponent<PruceduralRoad>().GenerateMeshes();
        junctionObj.GetComponent<PruceduralRoad>().GenerateMeshes();
    }

    public void SetJunctionPrucedure(UnityEngine.Object p)
    {
        junctionObj.GetComponent<PruceduralRoad>().SetProcedure(p);
    }

    public static List<Vector3> ConvertToWorldPoint(List<Vector3> list,Transform local)
    {
        List<Vector3> reslt = new List<Vector3>();
        for(int i=0;i<list.Count;i++)
        {
            reslt.Add(local.TransformPoint(list[i]));
        }
        return reslt;
    }
    public void CreateBase()
    {
        startVertices.Clear();
        //Debug.Log("CC id = "+ controller.name+" --- cross name = "+name);
        bool isMainPre = false;
        if (point.GetPostPoint()!=null)
            isMainPre = point.GetPostPoint().IsOnConfluence();//f
        else
            isMainPre = !point.GetPrePoint().IsOnConfluence();

        bool isOtherPre = false;
        if (other.GetPostPoint()!=null)
            isOtherPre = other.GetPostPoint().IsOnConfluence();//t
        else
            isOtherPre = !other.GetPrePoint().IsOnConfluence();
        isRight = CheckIsRightOfPoint(isMainPre ? point.GetPrePoint().transform : point.transform, other.transform);//t
        bool isOtherRight = CheckIsRightOfPoint(isOtherPre ? other.GetPrePoint().transform : other.transform, point.transform);//
        //bool isEndRight = CheckIsRightOfPoint();
        List<Vector3> LocalStartVertices = new List<Vector3>();
        List<Vector3> LocalEndVertices = new List<Vector3>();

        //Start Vertices World
        if (!isDefault)
        {
            List<Vector3> vList = controller.GetConfluenceSideWalk(point).GetEndVertices();
            List<Vector3> tempList = new List<Vector3>();
            for (int i = 0; i < vList.Count; i++)
                tempList.Add(vList[i]);
            tempList = SortOnDistance(tempList[0], tempList[1], controller.transform);
            if(isRight)
            {
                if(isMainPre)//t
                {
                    startVertices.Add(tempList[1]);
                    startVertices.Add(tempList[0]);
                }
                else//t
                {
                    startVertices.Add(tempList[0]);
                    startVertices.Add(tempList[1]);
                }
            }
            else
            {
                if (isMainPre)//t
                {
                    startVertices.Add(tempList[0]);
                    startVertices.Add(tempList[1]);
                }
                else//t
                {
                    startVertices.Add(tempList[1]);
                    startVertices.Add(tempList[0]);
                }
            }
            
        }
        else
        {
            int pointId = isMainPre ? point.GetID() - 1 : point.GetID();
            SideWalkManager SWA = point.GetManager().GetController().GetSideWalkManager();
            SideWalk SW = isRight ? SWA.GetRightSideWalks()[pointId] : SWA.GetLeftSideWalks()[pointId];
            List<Vector3> vetices = ConvertToWorldPoint(isMainPre ? SW.GetStartVertices() : SW.GetEndVertices(), SW.transform);
            //Debug.Log("parent = "+transform.parent.name+" ------ "+name+" Vertices count = " + vetices.Count);
            //Debug.Log("isMainPre=" + isMainPre + " --- pointID= " + pointId + " --- isRight = " + isRight);
            vetices = SortOnDistance(vetices[0], vetices[1], point.transform);
            startVertices.Clear();
            if (isRight)
            {
                if(isMainPre)
                {
                    startVertices.Add(vetices[1]);
                    startVertices.Add(vetices[0]);
                }
                else
                {
                    startVertices.Add(vetices[0]);
                    startVertices.Add(vetices[1]);
                }
            }
            else
            {
                if (isMainPre)
                {
                    startVertices.Add(vetices[0]);
                    startVertices.Add(vetices[1]);
                }
                else
                {
                    startVertices.Add(vetices[1]);
                    startVertices.Add(vetices[0]);
                }
            }
        }

        //End Vertices
        if (!isDefault)
        {
            int otherId = isOtherPre ? other.GetID() - 1 : other.GetID();
            SideWalkManager oSWA = other.GetManager().GetController().GetSideWalkManager();
            SideWalk oSW = isOtherRight ? oSWA.GetRightSideWalks()[otherId] : oSWA.GetLeftSideWalks()[otherId];
            List<Vector3> vv = ConvertToWorldPoint(isOtherPre ? oSW.GetStartVertices() : oSW.GetEndVertices(), oSW.transform);
            vv = SortOnDistance(vv[0], vv[1], other.transform);
            if(isRight)
            {
                if(isMainPre)//t
                {
                    endVertices.Add(vv[1]);
                    endVertices.Add(vv[0]);
                }
                else//t
                {
                    endVertices.Add(vv[0]);
                    endVertices.Add(vv[1]);
                }
            }
            else
            {
                if (isMainPre)//t
                {
                    endVertices.Add(vv[0]);
                    endVertices.Add(vv[1]);
                }
                else//t
                {
                    endVertices.Add(vv[1]);
                    endVertices.Add(vv[0]);
                }
            }
            
        }
        else
        {
            int otherId = isOtherPre ? other.GetID() - 1 : other.GetID();
            SideWalkManager oSWA = other.GetManager().GetController().GetSideWalkManager();
            SideWalk oSW = isOtherRight ? oSWA.GetRightSideWalks()[otherId] : oSWA.GetLeftSideWalks()[otherId];
            List<Vector3> vv = ConvertToWorldPoint(isOtherPre ? oSW.GetStartVertices() : oSW.GetEndVertices(), oSW.transform);
            vv = SortOnDistance(vv[0], vv[1], other.transform);
            if(isRight)
            {
                if(isMainPre)//T
                {
                    endVertices.Add(vv[1]);
                    endVertices.Add(vv[0]);
                }
                else
                {
                    endVertices.Add(vv[0]);
                    endVertices.Add(vv[1]);
                }
            }
            else
            {
                if (isMainPre)
                {
                    endVertices.Add(vv[0]);
                    endVertices.Add(vv[1]);
                }
                else
                {
                    endVertices.Add(vv[1]);
                    endVertices.Add(vv[0]);
                }
            }
        }
        GenerateBaseMesh(startVertices, endVertices);
        CreateJunction();
    }

    private List<Vector3> SortOnDistance(Vector3 v1,Vector3 v2,Transform point)
    {
        List<Vector3> v= new List<Vector3>();
        float dist1 = Vector3.Distance(v1, point.position);
        float dist2 = Vector3.Distance(v2, point.position);
        if(dist1<dist2)
        {
            v.Add(v1);
            v.Add(v2);
        }
        else
        {
            v.Add(v2);
            v.Add(v1);
        }

        return v;
    }

    private void CreateJunction()
    {
        bool isMainPre = point.GetPostPoint().IsOnConfluence();//f
        bool isOtherPre = other.GetPostPoint().IsOnConfluence();

        List<Vector3> startjunctionVertices = new List<Vector3>();
        List<Vector3> tempstartjunction = SortOnDistance(startVertices[0], startVertices[1], point.transform);
        //if (isDefault)
        //{
            if (isRight)
            {
                if (isMainPre)
                {
                    startjunctionVertices.Add(tempstartjunction[0]);
                    startjunctionVertices.Add(EdgeJunction);
                }
                else
                {
                    startjunctionVertices.Add(EdgeJunction);
                    startjunctionVertices.Add(tempstartjunction[0]);
                }
            }
            else
            {
                if(isMainPre)
                {
                    startjunctionVertices.Add(EdgeJunction);
                    startjunctionVertices.Add(tempstartjunction[0]);
                }
                else
                {
                    startjunctionVertices.Add(tempstartjunction[0]);
                    startjunctionVertices.Add(EdgeJunction);
                }
                
            }
        /*}
        else
        {
            if (isRight)
            {
                startjunctionVertices.Add(EdgeJunction);
                startjunctionVertices.Add(tempstartjunction[0]);
            }
            else
            {
                startjunctionVertices.Add(tempstartjunction[0]);
                startjunctionVertices.Add(EdgeJunction);
            }
        }*/
        
        List<Vector3> endJunctionVertices = new List<Vector3>();
        List<Vector3> tempEndVertices = SortOnDistance(endVertices[0], endVertices[1], other.transform);

        endJunctionVertices.Add(tempEndVertices[0]);
        endJunctionVertices.Add(EdgeJunction);

        
        

        if(junctionObj == null) 
            junctionObj = new GameObject("junction");

        junctionObj.transform.parent = transform;
        junctionObj.transform.position = transform.position;
        if(junctionObj.GetComponent<Junction>()==null)
            junctionObj.AddComponent<Junction>();
        if(junctionObj.GetComponent<PruceduralRoad>()==null)
            junctionObj.AddComponent<PruceduralRoad>();

        Junction j = junctionObj.GetComponent<Junction>();
        j.CreateBase(startjunctionVertices, endJunctionVertices);
    }

    public void GenerateBaseMesh(List<Vector3> startList, List<Vector3> endlist)
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();

        if (gameObject.GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = CreateBasePlane(startList, endlist);
        /*List<Vector3> allVertices = new List<Vector3>();
        allVertices.AddRange(startList);
        allVertices.AddRange(endlist);
        Debug.Log(allVertices.Count);
        for(int i=0;i<allVertices.Count;i++)
        {
            allVertices[i] = transform.InverseTransformPoint(allVertices[i]);
        }
        Mesh mesh = MeshUtility.GenerateFlatMeshOnVertices(allVertices);*/
        mesh.RecalculateBounds();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    public Mesh CreateBasePlane(List<Vector3> startList, List<Vector3> endList)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        /*if(isDefault && !isRight)
        {
            vertices[0] = transform.InverseTransformPoint(startList[1]);
            vertices[1] = transform.InverseTransformPoint(startList[0]);
            vertices[2] = transform.InverseTransformPoint(endList[0]);
            vertices[3] = transform.InverseTransformPoint(endList[1]);
        }
        else
        {*/
            vertices[0] = transform.InverseTransformPoint(startList[0]);
            vertices[1] = transform.InverseTransformPoint(startList[1]);
            vertices[2] = transform.InverseTransformPoint(endList[0]);
            vertices[3] = transform.InverseTransformPoint(endList[1]);
        //}
        

        normals[0] = new Vector3(0, 1, 0);
        normals[1] = new Vector3(0, 1, 0);
        normals[2] = new Vector3(0, 1, 0);
        normals[3] = new Vector3(0, 1, 0);

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 3;
        triangles[4] = 2;
        triangles[5] = 1;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.normals = normals;
        mesh.triangles = triangles;

        return mesh;

    }
}
