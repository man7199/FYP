using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoint : Building
{
    [SerializeField] HealthBar1 progressBar;
    [SerializeField] HealthBar1 storageBar;
    int resources = 900000000;           //the available resources in the point
    int resourcesCapacity = 900000000; //the max no. of resources the point can store

    public static float refillDistance = 8f;

    //[SerializeField] int resources = 10;           //the available resources in the point
    //[SerializeField] int resourcesCapacity = 100; //the max no. of resources the point can store


    float collectTime = 1f;
    int resourcePerCollectTime = 95;

    bool occupied = false;
    bool mining = false;
    RedBloodCell occupyWorker;
    private float counter;
    private float secondsCounter;

    void Start()
    {
        GroupsOfUnits.Instance.addResourcePoint(this);
        progressBar.UpdateHealthBarWithoutSlider(0f);
        storageBar.UpdateHealthBarWithoutSlider((resources / (float)resourcesCapacity));
        icon= Resources.Load<Sprite>("Arts/UI/Building/resourcecollection");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        counter += Time.deltaTime;
        secondsCounter+= Time.deltaTime;

        if (mining == true)
        {

            progressBar.UpdateHealthBarWithoutSlider((counter / (float)collectTime));
            if (counter >= collectTime)
            {
                Debug.Log("mining x1");

                counter = 0;
                AddResourceToWorker();
                progressBar.UpdateHealthBarWithoutSlider(0f);
            }
        }



        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(resources + " Added resources");
            resources += 5;
            Debug.Log(resources + " after Added resources");
        }

    }

    public bool CheckOccupyState()
    {
        return occupied;
    }

    public bool CheckEmpty()
    {
        return resources == 0;
    }

    public float GetRefillDistance()
    {
        return refillDistance;
    }
    public Vector3 Occupy(RedBloodCell worker)
    {
        if (!occupied && resources>0)
        {
            Debug.Log(worker.ToString() + " occupied! " + Time.frameCount +""+ this.name + "resources: " + resources);
            occupied = true;
            occupyWorker = worker;
            return transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void SetUnoccupied()
        //called when the occupy worker is killed, 
        //or when worker no longer occupy the point as other job is assigned to the worker 
    {
            Debug.Log("unoccupied");
            occupied = false;
            mining = false;
            occupyWorker = null;
            counter = 0;
            progressBar.UpdateHealthBarWithoutSlider(0f);

    }

    public void Mine()
    {
        Debug.Log("calling StartMine");
        if (mining == false && resources > 0)
        {
            counter = 0;
            mining = true;
        }
    }

    public void StopMine()
    {
        Debug.Log("calling StopMine");
        if (mining == true)
        {
            counter = 0;
            mining = false;
            progressBar.UpdateHealthBarWithoutSlider(0f);
        }
    }

    private void AddResourceToWorker()
    {
        //var affectedObjects = Physics.OverlapSphere(transform.position, resourceRadius);
        //foreach (var col in affectedObjects)
        //{
        //    if (GameObject.ReferenceEquals(col.GetComponent<Worker>(), occupyWorker))
        //    {
        if (resources > resourcePerCollectTime)
        {
            resources -= resourcePerCollectTime;

            int returnedResources= occupyWorker.GetResources(resourcePerCollectTime);
            resources += returnedResources;
            //the resources not obtained by the worker will be turned back

            storageBar.UpdateHealthBarWithoutSlider((resources / (float)resourcesCapacity));

        }
        else if(resources <= resourcePerCollectTime)
        {
            //all the resource remain will be tried to be given to the worker
            resources = occupyWorker.GetResources(resources);

            storageBar.UpdateHealthBarWithoutSlider((resources / (float)resourcesCapacity));

            Debug.Log("remaining " + resources);

            if (resources == 0)
            {
                mining = false;
                occupied = false;
                occupyWorker.UpdateResourcePoint(); //ask the worker to update resource point as this point is already empty
            }

        }
        //        
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other) //start mine when worker enter mining range
    {
        Debug.Log("enter resourcePoint:" + other.name);
        RedBloodCell worker = other.GetComponent<RedBloodCell>();
        if (worker != null) 
        {
            Debug.Log("enter resourcePoint:" + worker.name);
            if (GameObject.ReferenceEquals(worker, occupyWorker)) //same worker
            {
                worker.StopMoving();
                Mine();
            }
        }
    }

    private void OnTriggerExit(Collider other) //stop mine when worker exit mining range
    {
        RedBloodCell worker = other.GetComponent<RedBloodCell>();
        if (worker != null)
        {
            if (GameObject.ReferenceEquals(worker, occupyWorker)) //same worker
            {
                StopMine();
            }
        }

    }


}
