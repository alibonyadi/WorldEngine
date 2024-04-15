using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWalk : MonoBehaviour
{
    [SerializeField]
    private float width = 3;
    [SerializeField]
    private float height = 0.3f;
    [SerializeField]
    private bool isRight = true;
    [SerializeField]
    private bool isActive = true;
    [SerializeField]
    private SideWalkManager manager;
    [SerializeField]
    private Line SideWalkLine;
    [SerializeField]
    private UnityEngine.Object procedure;
    [SerializeField]
    private List<Vector3> mStartVertices = new List<Vector3>();
    [SerializeField]
    private List<Vector3> mEndVertices = new List<Vector3>();
    [SerializeField]
    private float VertexStartMoveDistance;
    [SerializeField]
    private float VertexEndMoveDistance;
    [SerializeField]
    private bool moveStartVertex;
    [SerializeField]
    private bool moveEndVertex;
    [SerializeField]
    private bool isStartOnConfluence;
    [SerializeField]
    private bool isEndOnConfluence;
    [SerializeField]
    private bool emptySide;

    public bool GetEmptySide() => emptySide;
    public void SetEmptySide(bool b)
    {
        emptySide = b;
        if(b)
        {
            isEndOnConfluence = false; 
            isStartOnConfluence = false;
            moveEndVertex = false;
            moveStartVertex = false;
        }
    }
    public void SetWidth(float w) => width = w;
    public void SetHeight(float h) => height = h;
    public void SetLine(Line line) => SideWalkLine = line;
    public void SetIsRight(bool b) => isRight = b;
    public bool GetIsRight() => isRight;
    public float GetWidth() => width;
    public List<Vector3> GetEndVertices() => mEndVertices;
    public List<Vector3> GetStartVertices() => mStartVertices;
    public void SetManager(SideWalkManager m) => manager = m;
    public void SetPrucedure(UnityEngine.Object p) => procedure = p;
    public void GenerateBaseMesh(float length)
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();

        if (gameObject.GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();

        //float height = Vector3.Magnitude(startPos.transform.position - endPos.transform.position);
        Mesh mesh = CreateBasePlane(width, length, new List<Vector3>());
        mesh.RecalculateBounds();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }
    public void GenerateBaseMesh(List<Vector3> startList, float length)
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();

        if (gameObject.GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = CreateBasePlane(width, length, startList);
        mesh.RecalculateBounds();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }


    //for test
    public void RecalculateSideWalks(bool changeStartVertices,float moveDist)
    {
        if (changeStartVertices)
        {
            moveStartVertex = true;
            VertexStartMoveDistance = moveDist;
            isStartOnConfluence = true;
        }
        else
        {
            moveEndVertex = true;
            VertexEndMoveDistance = moveDist;
            isEndOnConfluence = true;
        }
    }

    public void ResetForConfluence()
    {
        isStartOnConfluence = false;
        isEndOnConfluence = false;
        moveStartVertex = false;
        moveEndVertex = false;
        VertexStartMoveDistance = 0;
        VertexEndMoveDistance = 0;
    }

    public void ResetForConfluence(bool startVertexReset)
    {
        if (startVertexReset)
        {
            VertexStartMoveDistance = 0;
            moveStartVertex = false;
            isStartOnConfluence = false;
        }
        else
        {
            VertexEndMoveDistance = 0;
            moveEndVertex = false;
            isEndOnConfluence = false;
        }
    }

    public void GenerateBaseMesh(List<Vector3> startList, List<Vector3> endList,float length)
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
            gameObject.AddComponent<MeshFilter>();

        if (gameObject.GetComponent<MeshRenderer>() == null)
            gameObject.AddComponent<MeshRenderer>();
        //float height = Vector3.Magnitude(startPos.transform.position - endPos.transform.position);
        Mesh mesh = CreateBasePlane(width, length, startList, endList);
        mesh.RecalculateBounds();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void ReBuildByEndMeshVertices(Vector3 vertex1, Vector3 vertex2)
    {
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(vertex2);
        vertices.Add(vertex1);
        Debug.LogWarning("this is InComplete!!!!!!!");
        //GenerateBaseMesh(GetStartVertices(), vertices);
    }
    public Mesh CreateBasePlane(float width, float height, List<Vector3> startVertices, List<Vector3> endVertices)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        if (endVertices.Count <= 0)
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


        if(isStartOnConfluence && moveStartVertex)
        {
            vertices[2].z += VertexStartMoveDistance;
            vertices[3].z += VertexStartMoveDistance;
        }

        if(isEndOnConfluence && moveEndVertex)
        {
            vertices[0].z -= VertexEndMoveDistance;
            vertices[1].z -= VertexEndMoveDistance;    
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

        manager.SetTempFrontVertices(worldPosition1, worldPosition2, isRight);

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

        if (isStartOnConfluence && moveStartVertex)
        {
            vertices[2].z += VertexStartMoveDistance;
            vertices[3].z += VertexStartMoveDistance;
        }
        if (isEndOnConfluence && moveEndVertex)
        {
            vertices[0].z -= VertexEndMoveDistance;
            vertices[1].z -= VertexEndMoveDistance;
        }


        if (mStartVertices.Count > 0)
            mStartVertices.Clear();
        if (mEndVertices.Count > 0)
            mEndVertices.Clear();
        mStartVertices.Add(vertices[0]);
        mStartVertices.Add(vertices[1]);
        mEndVertices.Add(vertices[2]);
        mEndVertices.Add(vertices[3]);

        Vector3 worldPosition1 = transform.TransformPoint(vertices[0]);
        Vector3 worldPosition2 = transform.TransformPoint(vertices[1]);
        //Debug.Log(worldPosition1 + " --- " + worldPosition2 +" -- isRight = "+isRight);
        
        manager.SetTempFrontVertices(worldPosition1, worldPosition2, isRight);

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
