using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    [SerializeField]
    private bool initialized = false;
    [SerializeField]
    private StreetNetworkManager roadNetworkManager;
    [SerializeField]
    private LotManager lotManager;

    [SerializeField]
    private bool GenerateFullCity = false;
    [SerializeField]
    private bool GenerateRoads = false;
    [SerializeField]
    private bool GenerateLots = false;
    [SerializeField]
    private bool clearAll = false;

    public StreetNetworkManager GetRoadNetwork() => roadNetworkManager;

    private void OnDrawGizmos()
    {
        if (!initialized)
        {
            initialized = true;
            if(GetComponent<LotManager>()!=null)
                lotManager = GetComponent<LotManager>();
            else
                lotManager = gameObject.AddComponent<LotManager>();

            if(GetComponent<StreetNetworkManager>()!=null)
                roadNetworkManager = GetComponent<StreetNetworkManager>();
            else
                roadNetworkManager = gameObject.AddComponent<StreetNetworkManager>();

            if(GetComponent<StreetNetworkGenerator>()==null)
                gameObject.AddComponent<StreetNetworkGenerator>();
        }

        if(GenerateFullCity)
        {
            GenerateFullCity = false;
            if (GetComponent<StreetNetworkGenerator>() != null)
                GetComponent<StreetNetworkGenerator>().GenerateStreets();
            //roadNetworkManager.GenerateAllRoads();
            GenerateRoads = true;
            GenerateLots = true;
            //lotManager.GenerateAllLots();
        }

        if (GenerateRoads)
        {
            GenerateRoads  = false;
            roadNetworkManager.GenerateAllRoads();
        }

        if(GenerateLots)
        {
            GenerateLots = false;
            lotManager.GenerateAllLots();
        }

        if(clearAll)
        {
            clearAll = false;
            ClearAll();
        }
    }

    public void ClearAll()
    {
        lotManager.ClearAll();
        roadNetworkManager.ClearAll();
        Transform[] allObjects = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform obj in allObjects)
        {
            if (obj != gameObject.transform && obj != null)
            {
                DestroyImmediate(obj.gameObject);
            }
        }
    }

    public ConfluenceController FindCunfluenceByPoint(ControllerPoint point)
    {
        List<ConfluenceController> confluenceControllers = roadNetworkManager.GetAllConfluences();
        for (int i = 0; i < confluenceControllers.Count; i++)
        {
            if (confluenceControllers[i].ContainPoint(point))
            {
                return confluenceControllers[i];
            }
        }

        return null;
    }

}
