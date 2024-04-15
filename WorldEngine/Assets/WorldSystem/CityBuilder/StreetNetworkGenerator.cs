using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class StreetNetworkGenerator : MonoBehaviour
{
    [SerializeField]
    private bool isInitialized = false;
    [SerializeField]
    private CityManager cityManager;
    [SerializeField]
    private StreetNetworkManager networkManager;
    [SerializeField]
    private List<StreetNetworkHelper> helpers;
    [SerializeField]
    private int numberOfStreets = 30;
    [SerializeField]
    private float minStreetSize = 6.0f;
    [SerializeField]
    private float maxStreetSize = 24.0f;
    [SerializeField]
    private float minStreetLength = 100;
    [SerializeField]
    private float maxStreetLength = 750;
    [SerializeField]
    private float cityRadius = 3000;

    [SerializeField]
    private bool generateStreetNetwork = false;
    [SerializeField]
    private bool calculateStreets = false;
    [SerializeField]
    private bool findConfluencesOnStreet = false;


    public void GenerateStreets()
    {
        cityManager.ClearAll();
        helpers.Clear();
        for (int i=0;i<numberOfStreets;i++)
        {
            StreetNetworkHelper helper = new StreetNetworkHelper();
            StreetController sc = networkManager.AddStreet();
            helper.SetObject(sc.gameObject);
            float width = UnityEngine.Random.Range(minStreetSize, maxStreetSize);
            sc.SetStreetWidth(width);
            helper.SetWidth(width);

            ControllerPoint cp1 = sc.GetComponent<AddLinePoint>().GeneratePoint();
            helper.SetPoint(cp1);
            cp1.transform.position = transform.position + new Vector3 (UnityEngine.Random.Range(-cityRadius,cityRadius), 0, UnityEngine.Random.Range(-cityRadius, cityRadius));

            float length = maxStreetLength*( maxStreetSize/width) + UnityEngine.Random.Range(-minStreetLength, minStreetLength);
            if(length>maxStreetLength)
                length = maxStreetLength;

            
            //if(length > cityRadius*2)
            //    length = cityRadius*2;

            length = UnityEngine.Random.value > 0.5f ? length : -length;
            helper.SetLength(length);

            ControllerPoint cp2 = sc.GetComponent<AddLinePoint>().GeneratePoint();
            helper.SetPoint2(cp2);
            if (UnityEngine.Random.value > 0.5f)
            {
                cp2.transform.position = cp1.transform.position + new Vector3(0, 0, length);
                helper.SetIsVertical(true);
            }
            else
            {
                cp2.transform.position = cp1.transform.position + new Vector3(length, 0, 0);
                helper.SetIsVertical(false);
            }
            helpers.Add(helper);
        }
        //CalculateStreets();
    }

    public void CalculateStreets()
    {
        //Remove near streets
        for(int i=0;i<helpers.Count;i++)
        {
            for(int j=0;j<helpers.Count;j++)
            {

                if (i >= helpers.Count || helpers[i] == null || helpers[j] == null)
                    continue;

                bool sameObject = helpers[i].GetObject() == helpers[j].GetObject();
                bool sameDirction = helpers[i].IsVertical() == helpers[j].IsVertical();
                bool isVertical = helpers[i].IsVertical();
                if (!sameObject && sameDirction)
                {
                    if(isVertical && Mathf.Abs( helpers[i].GetPoint().transform.position.x - helpers[j].GetPoint().transform.position.x) < maxStreetSize * 3)
                    {
                        networkManager.RemoveStreet(helpers[j].GetObject().GetComponent<StreetController>());
                        DestroyImmediate(helpers[j].GetObject());
                        helpers.Remove(helpers[j]);
                    }
                    else if(!isVertical && Mathf.Abs(helpers[i].GetPoint().transform.position.z - helpers[j].GetPoint().transform.position.z) < maxStreetSize * 3)
                    {
                        networkManager.RemoveStreet(helpers[j].GetObject().GetComponent<StreetController>());
                        DestroyImmediate(helpers[j].GetObject());
                        helpers.Remove(helpers[j]);
                    }
                }
            }
        }

        for (int i = 0; i < helpers.Count; i++)
        {
            for (int j = 0; j < helpers.Count; j++)
            {

                if (i >= helpers.Count || helpers[i] == null || helpers[j] == null)
                    continue;

                bool sameObject = helpers[i].GetObject() == helpers[j].GetObject();
                bool sameDirction = helpers[i].IsVertical() == helpers[j].IsVertical();
                bool isVertical = helpers[i].IsVertical();
                if (!sameObject && sameDirction)
                {
                    if (isVertical && Mathf.Abs(helpers[i].GetPoint().transform.position.x - helpers[j].GetPoint().transform.position.x) < maxStreetSize * 3)
                    {
                        networkManager.RemoveStreet(helpers[j].GetObject().GetComponent<StreetController>());
                        DestroyImmediate(helpers[j].GetObject());
                        helpers.Remove(helpers[j]);
                    }
                    else if (!isVertical && Mathf.Abs(helpers[i].GetPoint().transform.position.z - helpers[j].GetPoint().transform.position.z) < maxStreetSize * 3)
                    {
                        networkManager.RemoveStreet(helpers[j].GetObject().GetComponent<StreetController>());
                        DestroyImmediate(helpers[j].GetObject());
                        helpers.Remove(helpers[j]);
                    }
                }
            }
        }

        //check to create default place confluences
        for (int i=0;i<helpers.Count;i++)
        {
            helpers[i].GetPoint().Changed();
            helpers[i].GetPoint().MoveFinished();

            helpers[i].GetPoint2().Changed();
            helpers[i].GetPoint2().MoveFinished();
        }

        //Find and Create Point On Confluences
        for(int i=0;i<helpers.Count;i++)
        {
            for(int j=0;j<helpers.Count;j++)
            {
                bool sameObject = helpers[i].GetObject() == helpers[j].GetObject();
                bool sameDirction = helpers[i].IsVertical() == helpers[j].IsVertical();
                if (!sameObject && !sameDirction)// && helpers[i].IsVertical())
                {
                    FindConfluencesOnStreet(helpers[i].GetObject().GetComponent<StreetController>(), helpers[j].GetObject().GetComponent<StreetController>(), helpers[i].IsVertical());
                }
            }
        }
    }

    public void FindConfluencesOnStreet(StreetController st1,StreetController st2,bool isPoint1Vertical)
    {
        
        if (st1 == null || st2 == null || st1 == st2)
            return;

        //Check All Points not Confluence
        List<ControllerPoint> points1 = st1.GetPointManager().GetControllerPoints();
        List<ControllerPoint> points2 = st2.GetPointManager().GetControllerPoints();
        for(int i=0;i<points1.Count;i++)
        {
            for(int j=0;j<points2.Count;j++)
            {
                if (points1[i].transform.position == points2[j].transform.position)
                {
                    points1[i].Changed();
                    points1[i].MoveFinished();
                    return;
                }
            }

            if (isPoint1Vertical && points1[i].transform.position.x == points2[0].transform.position.x)
            {
                points1[i].Changed();
                points1[i].MoveFinished();
                return;
            }
            else if (!isPoint1Vertical && points1[i].transform.position.z == points2[0].transform.position.z)
            {
                points1[i].Changed();
                points1[i].MoveFinished();
                return;
            }
        }

        //Check Lines
        bool isXcollide = false;
        
        bool isZcollide = false;
        
        if(isPoint1Vertical)
        {
            if (points1[0].transform.position.z < points1[points1.Count-1].transform.position.z)
            {
                if (points2[0].transform.position.z> points1[0].transform.position.z && points2[0].transform.position.z < points1[points1.Count - 1].transform.position.z)
                    isZcollide = true;
            }
            else
            {
                if (points2[0].transform.position.z < points1[0].transform.position.z && points2[0].transform.position.z > points1[points1.Count - 1].transform.position.z)
                    isZcollide = true;
            }

            if (points2[0].transform.position.x < points2[points2.Count - 1].transform.position.x)
            {
                if (points1[0].transform.position.x > points2[0].transform.position.x && points1[0].transform.position.x < points2[points2.Count - 1].transform.position.x)
                    isXcollide = true;
            }
            else
            {
                if (points1[0].transform.position.x < points2[0].transform.position.x && points1[0].transform.position.x > points2[points2.Count - 1].transform.position.x)
                    isXcollide = true;
            }
        }
        else
        {
            if (points1[0].transform.position.x < points1[points1.Count - 1].transform.position.x)
            {
                if (points2[0].transform.position.x > points1[0].transform.position.x && points2[0].transform.position.x < points1[points1.Count - 1].transform.position.x)
                    isXcollide = true;
            }
            else
            {
                if (points2[0].transform.position.x < points1[0].transform.position.x && points2[0].transform.position.x > points1[points1.Count - 1].transform.position.x)
                    isXcollide = true;
            }

            if (points2[0].transform.position.z < points2[points2.Count - 1].transform.position.z)
            {
                if (points1[0].transform.position.z > points2[0].transform.position.z && points1[0].transform.position.z < points2[points2.Count - 1].transform.position.z)
                    isZcollide = true;
            }
            else
            {
                if (points1[0].transform.position.z < points2[0].transform.position.z && points1[0].transform.position.z > points2[points2.Count - 1].transform.position.z)
                    isZcollide = true;
            }
        }

        if (!isXcollide || !isZcollide)
            return;

        float Xpos = isPoint1Vertical?points1[0].transform.position.x:points2[0].transform.position.x;
        float Zpos = isPoint1Vertical?points2[0].transform.position.z:points1[0].transform.position.z;
        int index = 0;
        for(int i=0;i<points1.Count-1;i++)
        {
            if(!isPoint1Vertical)
            {
                if(points1[i].transform.position.x < points1[i+1].transform.position.x)
                {
                    if(Xpos > points1[i].transform.position.x && Xpos < points1[i + 1].transform.position.x)
                    {
                        index = i + 1;
                        break;
                    }
                }
                else
                {
                    if (Xpos < points1[i].transform.position.x && Xpos > points1[i + 1].transform.position.x)
                    {
                        index = i + 1;
                        break;
                    }
                }
                
            }
            else if(isPoint1Vertical)
            {
                if (points1[i].transform.position.z < points1[i + 1].transform.position.z)
                {
                    if (Zpos > points1[i].transform.position.z && Zpos < points1[i + 1].transform.position.z)
                    {
                        index = i + 1;
                        break;
                    }
                }
                else
                {
                    if (Zpos < points1[i].transform.position.z && Zpos > points1[i + 1].transform.position.z)
                    {
                        index = i + 1;
                        break;
                    }
                }
            }
        }
        //Debug.Log("index " + index);
        float width = st2.GetStreetWidth();

        for (int i = 0; i < st2.GetPointManager().GetControllerPoints().Count; i++)
        {
            float dist = Vector3.Distance(st2.GetPointManager().GetControllerPoints()[i].transform.position, new Vector3(Xpos, 0, Zpos));
            if (dist < width)
            {
                st2.GetPointManager().GetControllerPoints()[i].transform.position = new Vector3(Xpos, 0, Zpos);
                //st2.GetPointManager().GetControllerPoints()[i].transform.position += (st2.GetPointManager().GetControllerPoints()[i].transform.position - new Vector3(Xpos, 0, Zpos))*3;
                break;
            }
        }

        if(Vector3.Distance(st1.GetPointManager().GetControllerPoints()[index-1].transform.position,new Vector3(Xpos,0,Zpos)) < width)
        {
            st1.GetPointManager().GetControllerPoints()[index - 1].transform.position = new Vector3(Xpos, 0, Zpos);
            st1.GetPointManager().GetControllerPoints()[index-1].Changed();
            st1.GetPointManager().GetControllerPoints()[index-1].MoveFinished();
        }
        else if(Vector3.Distance(st1.GetPointManager().GetControllerPoints()[index].transform.position, new Vector3(Xpos, 0, Zpos)) < width)
        {
            st1.GetPointManager().GetControllerPoints()[index].transform.position = new Vector3(Xpos, 0, Zpos);
            st1.GetPointManager().GetControllerPoints()[index].Changed();
            st1.GetPointManager().GetControllerPoints()[index].MoveFinished();
        }
        else
        {
            st1.GetPointManager().CreatePointOnIndex(index);
            st1.GetPointManager().GetControllerPoints()[index].transform.position = new Vector3(Xpos, st1.GetPointManager().GetControllerPoints()[index].transform.position.y, Zpos);
            //st1.GetPointManager().GetControllerPoints()[index + 1].transform.position += new Vector3(UnityEngine.Random.Range(-0.01f, 0.01f), 0, UnityEngine.Random.Range(-0.01f, 0.01f));
            //st1.GetPointManager().GetControllerPoints()[index - 1].transform.position += new Vector3(UnityEngine.Random.Range(-0.01f, 0.01f), 0, UnityEngine.Random.Range(-0.01f, 0.01f));
            st1.GetPointManager().GetControllerPoints()[index].Changed();
            st1.GetPointManager().GetControllerPoints()[index].MoveFinished();
        }

        
        /*index++;
        st1.GetPointManager().GetControllerPoints()[index+2].Changed();
        st1.GetPointManager().GetControllerPoints()[index+2].MoveFinished();
        st1.GetPointManager().GetControllerPoints()[index-2].Changed();
        st1.GetPointManager().GetControllerPoints()[index-2].MoveFinished();*/
    }

    public void OnDrawGizmos()
    {
        if(!isInitialized)
        {
            isInitialized = true;
            cityManager = GetComponent<CityManager>();
            networkManager = GetComponent<StreetNetworkManager>();
        }


        if(generateStreetNetwork)
        {
            generateStreetNetwork = false;
            GenerateStreets();
        }

        if(calculateStreets)
        {
            calculateStreets = false;
            CalculateStreets();
        }

        if(findConfluencesOnStreet)
        {
            findConfluencesOnStreet = false;
            //FindConfluencesOnStreet();
        }

    }
}

[Serializable]
public class StreetNetworkHelper
{
    [SerializeField]
    private GameObject obj = null;
    [SerializeField]
    private bool isVertical = false;
    [SerializeField]
    private float length = 0;
    [SerializeField]
    private float width = 0;
    [SerializeField]
    private ControllerPoint point = null;
    [SerializeField]
    private ControllerPoint point2 = null;

    public void SetObject(GameObject g) => obj = g;
    public void SetWidth(float w) => width = w;
    public void SetLength(float l) => length = l;
    public void SetIsVertical(bool b) => isVertical = b;
    public void SetPoint(ControllerPoint cp) => point = cp;
    public void SetPoint2(ControllerPoint cp) => point2 = cp;
    public GameObject GetObject() => obj;
    public float GetWidth() => width;
    public float GetLength() => length;
    public bool IsVertical() => isVertical;
    public ControllerPoint GetPoint() => point;
    public ControllerPoint GetPoint2() => point2;
    
}
