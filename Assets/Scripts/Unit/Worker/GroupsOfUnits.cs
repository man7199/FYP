using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupsOfUnits : MonoBehaviour
    //stores all units, group them
{
    public static GroupsOfUnits Instance;

    private List<ResourceCollectionPoint> baseList = new List<ResourceCollectionPoint>();
    private List<RedBloodCell> workerList = new List<RedBloodCell>();
    private List<ResourcePoint> resourcePointList = new List<ResourcePoint>();


    private void Awake()
    {
        Debug.Log("GroupsOfUnits awake called");
        if(Instance != null)
        {
            Debug.LogError("Only one GroupsOfUnits should exist");
            return;
        }

        Instance = this;


    }
    private float counter;
    private bool printing = true;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GroupsOfUnits awake called");
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        //     if (counter >= 1 && printing == true)
        // {
        //     Debug.Log(getBaseList().Count);
        //     Debug.Log(getWorkerList().Count);
        //     Debug.Log(getResourcePointList().Count);
        //     //Debug.Log(getWorkerList().ToArray().ToString());
        //     //Debug.Log(getResourcePointList().ToArray().ToString());
        //
        //     printing = false;
        // }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log(getBaseList().Count);
            Debug.Log(getWorkerList().Count);
            Debug.Log(getResourcePointList().Count);
        }
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    foreach(Base q in getBaseList())
        //    {
        //        Debug.Log("base: " + q);
        //    }
        //}
        //
    }

    public void addBase(ResourceCollectionPoint input)
    {
        baseList.Add(input);
    }
    public void RemoveBase(ResourceCollectionPoint input)
    {
        baseList.Remove(input);
    }

    public List<ResourceCollectionPoint> getBaseList()
    {
        return baseList;
    }

    public ResourceCollectionPoint getBase()
    {
        return baseList[0];
    }

    public void addWorker(RedBloodCell input)
    {
        workerList.Add(input);
    }
    public void RemoveWorker(RedBloodCell input)
    {
        workerList.Remove(input);
    }
    public List<RedBloodCell> getWorkerList()
    {
        return workerList;
    }

    public void addResourcePoint(ResourcePoint input)
    {
        resourcePointList.Add(input);
    }
    public void RemoveResourcePoint(ResourcePoint input)
    {
        resourcePointList.Remove(input);
    }
    public List<ResourcePoint> getResourcePointList()
    {
        return resourcePointList;
    }



}
