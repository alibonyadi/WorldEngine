using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField]
    private ControllerPoint startPos;
    [SerializeField]
    private ControllerPoint endPos;
    [SerializeField]
    private bool isInitialized = false;
    [SerializeField]
    private LineManager lineManager;
    [SerializeField]
    private List<Vector3> mStartVertices = new List<Vector3>();
    [SerializeField]
    private List<Vector3> mEndVertices = new List<Vector3>();
    //private StreetController streetController;
    [SerializeField]
    private float width = 3.0f;
    private void Update()
    {
        //Draw();
    }
    public void OnDrawGizmos()
    {
        if(!isInitialized)
        {
            isInitialized = true;
            mStartVertices = new List<Vector3>();
            mEndVertices = new List<Vector3>();
        }
        Draw();
    }
    public ControllerPoint GetStartPoint() => startPos;
    public ControllerPoint GetEndPoint() => endPos;

    public void SetWidth(float w) => width = w;
    public float GetWidth() => width;
    public List<Vector3> GetEndVertices() => mEndVertices;
    public List<Vector3> GetStartVertices() => mStartVertices;
    public Line(ControllerPoint startPoint, ControllerPoint endPoint)
    {
        SetPoints(startPoint, endPoint);
    }
    public void SetManager(LineManager m)=> lineManager = m;
    public void SetPoints(ControllerPoint startPoint, ControllerPoint endPoint)
    {
        startPos = startPoint;
        endPos = endPoint;
    }
    public void GenerateBaseMesh()
    {
        if(gameObject.GetComponent<MeshFilter>()==null)
            gameObject.AddComponent<MeshFilter>();

        if(gameObject.GetComponent<MeshRenderer>()==null)
            gameObject.AddComponent<MeshRenderer>();
        float height = Vector3.Magnitude(startPos.transform.position - endPos.transform.position);
        Mesh mesh = CreateBasePlane(width, height,new List<Vector3>());
        mesh.RecalculateBounds();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }
    public void GenerateBaseMesh(List<Vector3> startList)
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();

        if (gameObject.GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        float height = Vector3.Magnitude(startPos.transform.position - endPos.transform.position);
        Mesh mesh = CreateBasePlane(width, height, startList);
        mesh.RecalculateBounds();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void GenerateBaseMesh(List<Vector3> startList,List<Vector3> endList)
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();

        if (gameObject.GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        float height = Vector3.Magnitude(startPos.transform.position - endPos.transform.position);
        Mesh mesh = CreateBasePlane(width, height, startList, endList);
        mesh.RecalculateBounds();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void ReBuildByEndMeshVertices(Vector3 vertex1,Vector3 vertex2)
    {
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(vertex2);
        vertices.Add(vertex1);
        GenerateBaseMesh(GetStartVertices(), vertices);
    }

    public Mesh CreateBasePlane(float width, float height, List<Vector3> startVertices,List<Vector3> endVertices )
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        if(endVertices.Count<= 0)
        {
            vertices[0] = new Vector3(-width / 2, 0, height);
            vertices[1] = new Vector3(width / 2, 0, height);
        }
        else
        {
            vertices[0] = transform.InverseTransformPoint(endVertices[0]);
            vertices[1] = transform.InverseTransformPoint(endVertices[1]);
        }
        

        if (startVertices.Count <= 0)
        {
            vertices[2] = new Vector3(-width / 2, 0, 0);//-height / 2);
            vertices[3] = new Vector3(width / 2, 0, 0);// -height / 2);
        }
        else
        {
            vertices[2] = transform.InverseTransformPoint(startVertices[0]);
            vertices[3] = transform.InverseTransformPoint(startVertices[1]);
        }

        mStartVertices.Clear();
        mEndVertices.Clear();
        mStartVertices.Add(vertices[0]);
        mStartVertices.Add(vertices[1]);
        mEndVertices.Add(vertices[2]);
        mEndVertices.Add(vertices[3]);

        Vector3 worldPosition1 = transform.TransformPoint(vertices[0]);
        Vector3 worldPosition2 = transform.TransformPoint(vertices[1]);
        //Debug.Log(worldPosition1 + " --- " + worldPosition2);

        lineManager.SetTempFrontVertices(worldPosition1, worldPosition2);

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

    public Mesh CreateBasePlane(float width, float height, List<Vector3> startVertices)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];


        vertices[0] = new Vector3(-width / 2, 0, height);
        vertices[1] = new Vector3(width / 2, 0, height);

        if (startVertices.Count <= 0)
        {
            vertices[2] = new Vector3(-width / 2, 0, 0);//-height / 2);
            vertices[3] = new Vector3(width / 2, 0, 0);// -height / 2);
        }
        else
        {
            vertices[2] = transform.InverseTransformPoint(startVertices[0]);
            vertices[3] = transform.InverseTransformPoint(startVertices[1]);
        }
        
        //if(mStartVertices.Count>0) 
            mStartVertices.Clear();
        //if(mEndVertices.Count>0)
            mEndVertices.Clear();
        mStartVertices.Add(vertices[0]);
        mStartVertices.Add(vertices[1]);
        mEndVertices.Add(vertices[2]);
        mEndVertices.Add(vertices[3]);

        Vector3 worldPosition1 = transform.TransformPoint(vertices[0]);
        Vector3 worldPosition2 = transform.TransformPoint(vertices[1]);
        //Debug.Log(worldPosition1 + " --- " + worldPosition2);

        lineManager.SetTempFrontVertices(worldPosition1, worldPosition2);

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

    public void Draw()
    {
        Gizmos.DrawLine(startPos.transform.position, endPos.transform.position);
    }
}