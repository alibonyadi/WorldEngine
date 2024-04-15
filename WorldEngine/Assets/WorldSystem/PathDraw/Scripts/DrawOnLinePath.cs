using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WallDesigner;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class DrawOnLinePath : MonoBehaviour
{
    [SerializeField]
    LinePath linePath = new LinePath();
    [SerializeField]
    private bool canGenerate = false;
    private string path;
    [SerializeField]
    private float width = 10f;

    private List<Vector3> tempFrontVertex = new List<Vector3>();
    private List<Vector3> tempBackVertex = new List<Vector3>();

    List<FunctionItem> functions = new List<FunctionItem>();
    [SerializeField]
    [HideInInspector]
    private FunctionItem endItem;
    [SerializeField]
    [HideInInspector]
    private FunctionItem InputItem;

    [SerializeField]
    private UnityEngine.Object prucedure;

    private void Awake()
    {
        GetItemPath();
    }

    private void GetItemPath()
    {
        path = AssetDatabase.GetAssetPath(prucedure);
    }

    public void OnDrawGizmos()
    {
        if (canGenerate)
        {
            canGenerate = false;
            GetPoints();
            GeneratePath();
        }
    }

    public void GeneratePath()
    {
        GetItemPath();
        tempFrontVertex.Clear();
        Material mat = new Material(Shader.Find("Standard"));
        for (int i = 0; i < linePath.pointPosition.Count; i++)
        {
            if (i <= 0)
                continue;

            float height = Vector3.Magnitude(linePath.pointPosition[i - 1].transform.position - linePath.pointPosition[i].transform.position);
            Mesh mesh = CreateBasePlane(width, height, linePath.pointPosition[i - 1]);
            if (linePath.pointPosition[i - 1].GetComponent<MeshFilter>() == null)
            {
                linePath.pointPosition[i - 1].AddComponent<MeshFilter>().mesh = mesh;// CreateBasePlane(width, height);
            }
            else
            {
                linePath.pointPosition[i - 1].GetComponent<MeshFilter>().mesh = mesh;// CreateBasePlane(width, height);
            }

            if (linePath.pointPosition[i - 1].GetComponent<MeshRenderer>() == null)
                linePath.pointPosition[i - 1].AddComponent<MeshRenderer>();


            linePath.pointPosition[i - 1].GetComponent<MeshRenderer>().material = mat;
            WallItem item = (WallItem)GeneratePrucedure(mesh, linePath.pointPosition[i - 1]);
            if (item != null)
            {
                linePath.pointPosition[i - 1].GetComponent<MeshFilter>().mesh = item.wallPartItems[0].mesh;
                linePath.pointPosition[i - 1].GetComponent<MeshRenderer>().materials = item.wallPartItems[0].material.ToArray();
            }

        }
        Debug.Log("Folow Path Generate Complete!!!");
    }

    private object GeneratePrucedure(Mesh startMeshInput,GameObject caller)
    {
        if (path != "")
        {
            //Debug.Log("Is Generating Building by " + prucedure.name);
            functions.Clear();
            //string path = Application.dataPath + "/WorldSystem/WallDesigner/CreatedFunctions";
            List<SerializedFunctionItem> functionItems = SaveLoadManager.LoadSerializedFunctionItemList(path);
            //Debug.Log("number of "+functionItems.Count+" function loaded!");
            //List<FunctionItem> functions = new List<FunctionItem>();
            foreach (SerializedFunctionItem item2 in functionItems)
            {
                Type type = Type.GetType(item2.ClassName);
                if (type != null && type.IsSubclassOf(typeof(FunctionItem)))
                {
                    FunctionItem fitem = (FunctionItem)Activator.CreateInstance(type, item2.getnodeItems.Count, item2.getnodeItems.Count);
                    fitem.LoadSerializedAttributes(item2);
                    fitem.position = item2.Position;
                    functions.Add(fitem);
                }
                if (functions[functions.Count - 1].GetType() == typeof(EndCalculate))
                {
                    //EndItemIndex = functions.Count - 1;
                    endItem = functions[functions.Count - 1];
                    //CreateAction(EndItemIndex);
                    //Debug.Log("EndItem founded!! " + endItem.Name);
                }
                if (functions[functions.Count - 1].GetType() == typeof(GetInputMesh))
                {
                    InputItem = functions[functions.Count - 1];
                    GetInputMesh gIM = InputItem as GetInputMesh;
                    WallPartItem wallPartItem = new WallPartItem();
                    wallPartItem.mesh = startMeshInput;
                    gIM.inputMesh.wallPartItems.Add(wallPartItem);
                    gIM.havemesh = true;
                    gIM.inputMesh.Caller = caller;
                    gIM.inputMesh.isInEditMode = false;
                    functions[functions.Count - 1] = gIM;
                    //CreateAction(EndItemIndex);
                    //Debug.Log("GetInput founded!! " + InputItem.Name);
                }
            }
            //WallEditorController.Instance.SetAllCreatedItems(functions);
            for (int i = 0; i < functionItems.Count; i++)
            {
                functions[i].LoadNodeConnections(functionItems[i], functions);
            }

            if (endItem == null)
                return null;

            //Debug.Log("There is EndItem!!!");

            if (endItem.GetNodes[0].ConnectedNode == null)
                return null;

            //Debug.Log("EndItem Connected!!!");

            WallItem item = new WallItem();
            item = (WallItem)endItem.myFunction(item, 0);

            //GetComponent<MeshFilter>().mesh = item.wallPartItems[0].mesh;
            //GetComponent<MeshRenderer>().materials = item.wallPartItems[0].material.ToArray();
            return item;
            
        }

        return null;
    }

    private void GetPoints()
    {
        linePath.pointPosition.Clear();
        List<Transform> objects = gameObject.GetComponentsInChildren<Transform>().ToList();
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].gameObject != gameObject)
            {
                linePath.pointPosition.Add(objects[i].gameObject);
                if (linePath.pointPosition.Count > 1)
                {
                    linePath.pointPosition[linePath.pointPosition.Count - 2].transform.LookAt(linePath.pointPosition[linePath.pointPosition.Count - 1].transform);
                }
            }
        }
        //Debug.Log(objects.Count);
        //linePath.pointPosition = gameObject.GetComponentsInChildren<Transform>().ToList();
    }

    public Mesh CreateBasePlane(float width, float height,GameObject obj)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(-width / 2, 0, height );
        vertices[1] = new Vector3(width / 2, 0, height );

        if (tempFrontVertex.Count <= 0)
        {
            vertices[2] = new Vector3(-width / 2, 0, 0);//-height / 2);
            vertices[3] = new Vector3(width / 2, 0, 0);// -height / 2);
        }
        else
        {
            Vector3 localPos1 = obj.transform.InverseTransformPoint(tempFrontVertex[0]);
            Vector3 localPos2 = obj.transform.InverseTransformPoint(tempFrontVertex[1]);

            vertices[2] = localPos1;
            vertices[3] = localPos2;
        }

        tempFrontVertex.Clear();
        Vector3 worldPosition1 = obj.transform.TransformPoint(vertices[0]);
        Vector3 worldPosition2 = obj.transform.TransformPoint(vertices[1]);
        //Debug.Log(worldPosition1 + " --- " + worldPosition2);
        tempFrontVertex.Add(worldPosition1);
        tempFrontVertex.Add(worldPosition2);

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

public class LinePath
{
    public List<GameObject> pointPosition = new List<GameObject>();
}

public class LinePathPoint
{
    public float width = 7;
}
