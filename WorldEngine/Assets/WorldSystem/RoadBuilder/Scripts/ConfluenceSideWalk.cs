using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfluenceSideWalk:MonoBehaviour
{
    Vector3 EdgeMain;
    Vector3 EdgeMain2;
    Vector3 EdgeOther;
    bool isRight = false;
    float sideWalkWidth = 3.0f;
    ControllerPoint point;
    [SerializeField]
    ControllerPoint other;
    ConfluenceSideWalkController controller;
    List<Vector3> startVertices = new List<Vector3>();
    List<Vector3> endVertices = new List<Vector3>();

    public void Initialize(ControllerPoint mainPoint, ControllerPoint otherPoint, Vector3 edgePoint,Vector3 edgeOther,ConfluenceSideWalkController c)
    {
        EdgeMain = edgePoint; 
        EdgeOther = edgeOther;
        point = mainPoint;
        other = otherPoint;
        controller = c;
    }

    public List<Vector3> GetStartVertices() => startVertices;
    public List<Vector3> GetEndVertices() => endVertices;

    public void CreateBase()
    {
        startVertices.Clear();
        //endVertices.Clear();
        bool isOtherPre = false;
        if (other.GetPostPoint()!=null)
            isOtherPre = other.GetPostPoint().IsOnConfluence();
        else
            isOtherPre = !other.GetPrePoint().IsOnConfluence();
        //Debug.Log("Other pre ? = " + isOtherPre);
        float width = 0;
        if (point.GetID()< point.GetManager().GetControllerPoints().Count-1)
            width = point.GetComponent<Line>().GetWidth();
        else
            width = point.GetManager().GetController().GetStreetWidth();
        sideWalkWidth = point.GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[point.GetID()].GetWidth();
        ControllerPoint p = isOtherPre ? other.GetPrePoint() : other;
        bool isOtherSidewalkLeft = p.transform.InverseTransformPoint(EdgeMain).x<0;
        //Debug.Log("is left side walk ? = " + isOtherSidewalkLeft);
        Transform sidewalk = !isOtherSidewalkLeft? other.GetManager().GetController().GetComponent<SideWalkManager>().GetRightSideWalks()[p.GetID()].transform: other.GetManager().GetController().GetComponent<SideWalkManager>().GetLeftSideWalks()[p.GetID()].transform;
        isRight = transform.InverseTransformPoint(sidewalk.transform.position).x > 0;
        if ( point.transform.InverseTransformPoint(other.transform.position).x > 0)
        {
            EdgeMain2 = point.transform.TransformPoint(new Vector3((width / 2)+sideWalkWidth, 0, 0));
            //isRight = true; 
        }
        else
        {
            EdgeMain2 = point.transform.TransformPoint(new Vector3((-width / 2)-sideWalkWidth, 0, 0));
            //isRight = false;
        }
        startVertices.Add(EdgeMain); 
        startVertices.Add(EdgeMain2);
        GenerateBaseMesh(startVertices,Vector3.Magnitude(EdgeMain-EdgeOther));
    }

    public void GenerateBaseMesh(List<Vector3> startList, float length)
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();

        if (gameObject.GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = CreateBasePlane(sideWalkWidth, length, startList);
        mesh.RecalculateBounds();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    public Mesh CreateBasePlane(float width, float height, List<Vector3> startVertices)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        if (isRight)
        {
            vertices[0] = new Vector3(0, 0, height - width);
            vertices[1] = new Vector3(width, 0, height - width);
        }
        else
        {
            vertices[1] = new Vector3(0, 0, height - width);
            vertices[0] = new Vector3(-width, 0, height - width);
        }
        endVertices.Clear();
        endVertices.Add(transform.TransformPoint(vertices[1]));
        endVertices.Add(transform.TransformPoint(vertices[0]));
        /*}
        else 
        {
            vertices[0] = new Vector3(width , 0, height);
            vertices[1] = new Vector3(0, 0, height);
        }*/

        if (startVertices.Count <= 0)
        {
            if (isRight)
            {
                vertices[2] = new Vector3(0, 0, 0);//-height / 2);
                vertices[3] = new Vector3(width, 0, 0);// -height / 2);
            }
            else
            {
                vertices[2] = new Vector3(-width, 0, 0);//-height / 2);
                vertices[3] = new Vector3(0, 0, 0);// -height / 2);
            }
        }
        else
        {
            if (isRight)
            {
                vertices[2] = transform.InverseTransformPoint(startVertices[0]);
                vertices[3] = transform.InverseTransformPoint(startVertices[1]);
            }
            else
            {
                vertices[3] = transform.InverseTransformPoint(startVertices[0]);
                vertices[2] = transform.InverseTransformPoint(startVertices[1]);
            }
        }


        /*if (isStartOnConfluence && moveStartVertex)
        {
            vertices[2].z += VertexStartMoveDistance;
            vertices[3].z += VertexStartMoveDistance;
        }
        if (isEndOnConfluence && moveEndVertex)
        {
            vertices[0].z -= VertexEndMoveDistance;
            vertices[1].z -= VertexEndMoveDistance;
        }*/

        /*if (mStartVertices.Count > 0)
            mStartVertices.Clear();
        if (mEndVertices.Count > 0)
            mEndVertices.Clear();
        mStartVertices.Add(vertices[0]);
        mStartVertices.Add(vertices[1]);
        mEndVertices.Add(vertices[2]);
        mEndVertices.Add(vertices[3]);*/

        //Vector3 worldPosition1 = transform.TransformPoint(vertices[0]);
        //Vector3 worldPosition2 = transform.TransformPoint(vertices[1]);
        //Debug.Log(worldPosition1 + " --- " + worldPosition2 +" -- isRight = "+isRight);

        //manager.SetTempFrontVertices(worldPosition1, worldPosition2, isRight);

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