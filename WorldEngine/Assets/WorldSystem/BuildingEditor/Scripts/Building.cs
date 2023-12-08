using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WallDesigner;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class Building : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.Object prucedure;

    List<FunctionItem> functions = new List<FunctionItem>();
    private string path;
    private Mesh startMeshTemp;
    private Mesh startMeshInput;
    private bool isBaseMeshLoaded;
    private WallPartItem outputMesh;
    private FunctionItem endItem;
    private FunctionItem InputItem;

    [SerializeField]
    private bool canGenerate = false;

    Building()
    {
        //startMeshInput = GetComponent<MeshFilter>().mesh;
    }

    private void Awake()
    {
        GetItemPath();
    }

    private void GetItemPath()
    {
        path = AssetDatabase.GetAssetPath(prucedure);
    }

    private void GenerateBuilding()
    {
        GetItemPath();
        
        if(!isBaseMeshLoaded)
        {
            isBaseMeshLoaded = true;
            startMeshInput = startMeshTemp = GetComponent<MeshFilter>().sharedMesh;
        }
        else
        {
            startMeshInput = startMeshTemp;
        }

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
                    FunctionItem fitem = (FunctionItem)Activator.CreateInstance(type,item2.getnodeItems.Count,item2.getnodeItems.Count);
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
                    gIM.inputMesh.Add(wallPartItem);
                    gIM.havemesh = true;
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
                return;

            //Debug.Log("There is EndItem!!!");

            if (endItem.GetNodes[0].ConnectedNode == null)
                return;

            //Debug.Log("EndItem Connected!!!");

            WallItem item = new WallItem();
            item = (WallItem)endItem.myFunction(item, 0);

            GetComponent<MeshFilter>().mesh = item.wallPartItems[0].mesh;
            GetComponent<MeshRenderer>().materials = item.wallPartItems[0].material.ToArray();
            Debug.Log("Generate Complete!!!");
        }
    }

    public void OnDrawGizmos()
    {
        if (canGenerate)
        {
            canGenerate = false;
            GenerateBuilding();
        }
    }

    private void Start()
    {
        
        Debug.Log(path);
    }

}
