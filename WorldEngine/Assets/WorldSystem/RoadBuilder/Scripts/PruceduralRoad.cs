using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using WallDesigner;

public class PruceduralRoad : MonoBehaviour
{
    [SerializeField]
    private bool canGenerate = false;
    private string path;
    List<FunctionItem> functions = new List<FunctionItem>();
    [SerializeField]
    [HideInInspector]
    private FunctionItem endItem;
    [SerializeField]
    [HideInInspector]
    private FunctionItem InputItem;
    //private Mesh startMeshTemp;
    private Mesh startMeshInput;
    private bool isBaseMeshLoaded;
    [SerializeField]
    private UnityEngine.Object prucedure;
    [SerializeField]
    private Mesh baseMesh;
    [SerializeField]
    private GameObject meshHolderObj;
    public void SetProcedure(UnityEngine.Object p) => prucedure = p;
    public void SetBaseMesh(Mesh mesh) => baseMesh = mesh;
    public Mesh GetBaseMesh() => baseMesh;
    public void SetMeshHolder(GameObject go) => meshHolderObj = go;
    public GameObject GetMeshHolderObj() => meshHolderObj;
    private void GetItemPath()
    {
        if(prucedure!=null)
            path = AssetDatabase.GetAssetPath(prucedure);
    }

    public void OnDrawGizmos()
    {
        if (canGenerate)
        {
            canGenerate = false;
            GenerateMeshes();
        }
    }

    public void GenerateMeshes()
    {
        //if (canGenerate)
        //{
        //canGenerate = false;
        GetItemPath();
        //Thread tr = new Thread(new ThreadStart(GenerateRoad));
        //tr.Start();
        //}
        GenerateRoad();
        /*try
        {
            GenerateRoad();
        }
        catch (Exception e)
        {
            //Debug.LogWarning("Can't Create " + path);
            Debug.LogWarning(e);
        }*/
    }

    public void SetPath(string p) => path = p;


    private void GenerateRoad()
    {
        if (meshHolderObj == null)
        {
            if (GetComponent<MeshFilter>() != null)
                meshHolderObj = GetComponent<MeshFilter>().gameObject;
            else
            {
                Debug.LogWarning("No GameObject To Get Mesh From!!!");
                return;
            }
        }

        //if (!isBaseMeshLoaded)
        //{
        //    isBaseMeshLoaded = true;
            startMeshInput = meshHolderObj.GetComponent<MeshFilter>().sharedMesh;
        //}

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
                    gIM.inputMesh.Caller = gameObject;
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
                functions[i].LoadProperty(functionItems[i],functions);
            }

            if (endItem == null)
                return;

            //Debug.Log("There is EndItem!!!");

            if (endItem.GetNodes[0].ConnectedNode == null)
                return;

            //Debug.Log("EndItem Connected!!!");

            WallItem item = new WallItem();
            item = (WallItem)endItem.myFunction(item, 0);

            meshHolderObj.GetComponent<MeshFilter>().mesh = item.wallPartItems[0].mesh;
            meshHolderObj.GetComponent<MeshRenderer>().materials = item.wallPartItems[0].material.ToArray();
            //Debug.Log("Generate Complete!!!");
        }
    }
}