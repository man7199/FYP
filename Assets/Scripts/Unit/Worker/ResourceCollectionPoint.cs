using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollectionPoint : Building
{
    public int resource = 0;
    public static float submitDistance = 3f;

    int sufficientResourcesCapacity = 2000;
    [SerializeField] HealthBar1 storageBar;
    private ResourceManager resourceMangager;

    bool built = false; //if the building is successfully built

    // Start is called before the first frame update
    public override void Spawned()
    {
        base.Spawned();
    }

    private string Info
= "This is resource collection point, where workers submit resoruces to it.";

    public override string getInfo()
    {
        return Info;
    }

    private void Awake()
    {
        //Debug.Log("base Awake called");

        icon = Resources.Load<Sprite>("Arts/UI/Building/resourcecollection");
    }
    protected override void Update()
    {
        base.Update();
        if (!built)
        {
            if (GetComponent<Transform>().parent == null) //building constructed
            {
                built = true;
                GroupsOfUnits.Instance.addBase(this);
                resourceMangager = GameObject.Find("Network Game Manager")?.GetComponent<ResourceManager>();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        RedBloodCell worker = other.GetComponent<RedBloodCell>();
        if(worker != null)
        {
            worker.SendResourcesToBase();
            Debug.Log("to base:" + worker.CheckWorkerDistanceToBase());
        }
    }

    public void ManagerResourceAdd(int numOfResource) //resource added through resourceManager
    {
        resource += numOfResource;
        storageBar.UpdateHealthBarWithoutSlider((resource / (float)sufficientResourcesCapacity));
        Debug.Log(resource);
    }

    public void addResource(int numOfResource) //resource added through workers
    {
        resource += numOfResource;
        storageBar.UpdateHealthBarWithoutSlider((resource / (float)sufficientResourcesCapacity));
        resourceMangager.AddCellResources(numOfResource);
    }

    public override void OnDestroy()
    {
        GroupsOfUnits.Instance.RemoveBase(this);
    }


}
