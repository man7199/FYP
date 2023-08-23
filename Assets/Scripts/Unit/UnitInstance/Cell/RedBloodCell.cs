using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class RedBloodCell : Cell
{

    public ResourceCollectionPoint playerBase; //might be more than 1
    Vector3 baseCoordinate;

    private Building targetBuilding;
    private Vector3 buildingCoordinate;
    
    ResourcePoint occupyingResourcePoint;
    Vector3 resourceCoordinate;

    protected WorkType working; //also use target to show if it is working, target == null means not working
    
    int carryingResource = 0;

    public enum WorkType
    {
        Mining,
        Building,
        Idle
    }

    [SerializeField] ResourceBar resourceBar;
     int maxResource = 100;

    protected float timer=0;
    protected bool aiming = false;
    protected bool moving = false;

    protected Vector3 aimingCoordinate= Vector3.zero;

    protected override Node SetupBehaviorTree()
    {
        return WorkerSubtree_2.WorkerTree(this);
    }


    protected override void Awake()
    {
        base.Awake();
        description[3] = "Start mining";
        description[4] = "Stop mining";
        working = WorkType.Idle;
        icon = Resources.Load<Sprite>("Icons/Cell/redbloodcell");
    }


    public override void Spawned()
    {
        base.Spawned();
        SetWorkTypeMining();
        //StartCoroutine(LateStart(0.5f));
    }
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //UpdateBasePoint();
    }
    protected override void Update()
    {
        base.Update();

        if (aiming) //aiming means the worker aims at the position, possibly base or resourcepoint
        {
            TurnTowardsCoordinate(aimingCoordinate);
        }
        if (moving) //works with aiming, moving means the worker is moving forward, possibly towards base or resourcepoint
        {
            MoveForward();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetNotWorking();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (playerBase != null)
            {
                Debug.Log("base: " +playerBase.name);
            }
            else
            {
                Debug.Log("no base");
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(CheckWorkerDistanceToBase());
            Debug.Log(baseCoordinate);
        }
    }

    public bool UpdateBasePoint()
    {
        List<ResourceCollectionPoint> tempList = GroupsOfUnits.Instance.getBaseList();
        if (tempList.Count > 0) //has base
        {
            float minDistance = float.MaxValue;
            ResourceCollectionPoint tempBase = null;
            Vector3 tempBaseCoordinate;

            foreach (ResourceCollectionPoint temp in tempList)
            {
                if (temp != null)
                {
                    float distance = float.MaxValue;
                    if (occupyingResourcePoint == null)
                    {
                        distance = Vector3.Distance(this.transform.position, temp.transform.position);
                    }
                    else
                    {
                        distance = Vector3.Distance(occupyingResourcePoint.transform.position, temp.transform.position);
                    }
                    if (distance < minDistance)
                    {
                        tempBase = temp;
                        tempBaseCoordinate = tempBase.transform.position;
                        minDistance = distance;
                    }
                }
            }
            if (tempBase != null) //base is updated
            {
                playerBase = tempBase;
                baseCoordinate = playerBase.transform.position;
                return true;
            }
        }
        return false;
    }
    public bool UpdateResourcePoint() //return true when found one
    {
        ResourcePoint[] resourcePoints = GroupsOfUnits.Instance.getResourcePointList().ToArray();

        float minDistance = float.MaxValue;

        foreach (ResourcePoint resourcePoint in resourcePoints)
        {
            float distance = float.MaxValue;
            Vector3 position = resourcePoint.Occupy(this); //try to occupy resourcePoint
            if (position != Vector3.zero) //already occupy new resourcePoint
            {
                distance = Vector3.Distance(position, transform.position); //distance between worker and resourcePoint
                if (distance < minDistance)
                {
                    if (occupyingResourcePoint != null)
                    {
                        occupyingResourcePoint.SetUnoccupied(); //release previously the occupying resourcePoint
                    }
                    minDistance = distance;
                    occupyingResourcePoint = resourcePoint; //save occupied resourcePoint
                    resourceCoordinate = position;
                }
                else
                {
                    resourcePoint.SetUnoccupied();
                }
            }
        }
        if (occupyingResourcePoint != null)
        {
            MoveTowardsResourcePoint();
            UpdateBasePoint();
            return true;
        }

        occupyingResourcePoint = null;

        if (playerBase != null)
        {
            if (CheckWorkerDistanceToBase() > ResourceCollectionPoint.submitDistance)
            {
                MoveTowardsBase();
            }
        }
        else
        {
            Debug.Log("null base_123");
            bool success = UpdateBasePoint(); //if cannot update base, means the worker should stop working
            if (!success)
            {
                SetNotWorking();
            }
        }
        return false;
    }

    public void MoveToMineOrBase()
    {
        //performance issue
        //add time restriction here so code can be only executed every e.g.0.1s
            if(playerBase == null)
            {
                bool success = UpdateBasePoint(); 
                if(!success) //no base to go, goes idle
                {
                SetNotWorking();
                }
            }
            else if (!occupyingResourcePoint.CheckEmpty() && carryingResource < maxResource && occupyingResourcePoint != null)
            {
                if (CheckWorkerDistanceToResourcePoint() > occupyingResourcePoint.GetRefillDistance()-2f)
                {
                MoveTowardsResourcePoint();
                }
                else
                {
                    StopMoving();
                    aimingCoordinate = baseCoordinate;
                }
            }
            else
            {
                if (CheckWorkerDistanceToBase() > ResourceCollectionPoint.submitDistance) //no resource point, move to base
                {
                MoveTowardsBase();
                Debug.Log("MoveTowardsBase();");
                }
                else //
                {
                Debug.Log("StopMoving(); " + (CheckWorkerDistanceToBase() > ResourceCollectionPoint.submitDistance - 1f) + "?" + CheckWorkerDistanceToBase() + "??" + (ResourceCollectionPoint.submitDistance - 1f));
                StopMoving();
                aimingCoordinate = resourceCoordinate;
            }
        }
    }
    public int GetResources(int num)
    {
        int remainResource = 0;
        AudioManager.Play("mining", AudioManager.MixerTarget.SFX, transform.position);
        if (carryingResource < maxResource)
        //not full
        {
            if (carryingResource + num >= maxResource)
            {
                remainResource = carryingResource + num - maxResource;
                carryingResource = maxResource;
                resourceBar.UpdateHealthBar((carryingResource / (float)maxResource));

                if(occupyingResourcePoint != null)
                {
                    occupyingResourcePoint.StopMine();
                }

                //moving towards base
                MoveTowardsBase();
            }
            else
            {
                carryingResource += num;
                resourceBar.UpdateHealthBar((carryingResource / (float)maxResource));
            }
            return remainResource;
        }
        return num;
    }

    public void MoveTowardsBase()
    {
        if (playerBase != null) //there exist a base
        {
            aimingCoordinate = baseCoordinate;
            StartAiming();
            StartMoving();
        }
        else
        {
            bool success = UpdateBasePoint();
            if(!success)
            {
                SetNotWorking();
            }
            else
            {
                MoveTowardsBase();
            }
        }
    }

    public void MoveTowardsResourcePoint()
    {
        aimingCoordinate = resourceCoordinate;
        StartAiming();
        StartMoving();
    }

    public void MoveTowardsBuildingCoordinate()
    {
        aimingCoordinate = buildingCoordinate;
        StartAiming();
        StartMoving();
    }

    public bool MineResourcePoint()
    {
        if(occupyingResourcePoint != null)
        {
            occupyingResourcePoint.Mine();
            return true;
        }
        return false;
    }

    public bool StopMineResourcePoint()
    {
        if (occupyingResourcePoint != null)
        {
            occupyingResourcePoint.StopMine();
            return true;
        }
        return false;
    }
    public bool CheckFull()
    {
        if( carryingResource == maxResource)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public WorkType CheckState()
    {
        return working;
    }
    public void SetNotWorking() //unlink the resource point
    {
        Debug.Log("SetNotWorking");
        StopAiming();
        StopMoving();
        if (occupyingResourcePoint != null)
        {
            occupyingResourcePoint.SetUnoccupied();
            occupyingResourcePoint = null;
            working = WorkType.Idle;
        }
        working = WorkType.Idle;
    }
    public void SetWorkTypeMining()
    {
        if (playerBase == null) //no basepoint
        {
            bool success = UpdateBasePoint();
            if (!success)
            {
                SetNotWorking(); //cannot work as no basepoint
            }
            else
            {
                SetWorkTypeMining();
            }
        }
        else
        {
            if (working != WorkType.Mining)
            {
                SetNotWorking();
                working = WorkType.Mining;
            }
        }
    }
    public void SetWorkTypeBuilding()
    {
        if (working != WorkType.Building)
        {
            SetNotWorking();
            working = WorkType.Building;
        }
    }
    public bool OccupyingResourcePoint()
    {
        return occupyingResourcePoint != null;
    }
    public ResourcePoint GetOccupyingResourcePoint()
    {
        return occupyingResourcePoint;
    }
    public Vector3 GetBaseCoordinate()
    {
        return baseCoordinate;
    }
    public float CheckWorkerDistanceToBase()
    {
        return Vector3.Distance(transform.position, baseCoordinate);
    }
    public float CheckWorkerDistanceToResourcePoint()
    {
        return Vector3.Distance(transform.position, resourceCoordinate);
    }
    public void StartAnimation()
    {
        Debug.Log("start mining anime");
    }
    public void StopAnimation()
    {
        Debug.Log("stop mining anime");
    }
    public void StartAiming()
    {
        aiming= true;
    }
    public void StopAiming()
    {
        aiming= false;
    }
    public void StartMoving()
    {
        moving = true;
    }
    public void StopMoving()
    {
        moving= false;
    }
    public void SendResourcesToBase()
    {
        if (playerBase != null) //has base
        {
            if (carryingResource > 0)
            {
                AudioManager.Play("sendingResource", AudioManager.MixerTarget.SFX, transform.position);
                int submitResource = carryingResource;
                carryingResource = 0;
                playerBase.addResource(submitResource);

                resourceBar.UpdateHealthBar((carryingResource / (float)maxResource));
                if (occupyingResourcePoint != null)
                {
                    MoveTowardsResourcePoint();
                }
                else
                {
                    SetNotWorking(); //let the worker rest as it can do nothing now
                }
            }
            else
            {
                if (occupyingResourcePoint == null) //avoid mis-set unworking if workers accidently enter base when working
                {
                    SetNotWorking();
                }
            }
        }
        else //no base
        {
            bool success = UpdateBasePoint();
            if (success)
            {
                SendResourcesToBase();
            }
            else
            {
                SetNotWorking();
            }
        }
    }
    public void ManualSendResourcesToBase()
    {
        if (playerBase != null)
        {
            if (CheckWorkerDistanceToBase() <= ResourceCollectionPoint.submitDistance)
            {
                playerBase.addResource(carryingResource);
                carryingResource = 0;
                resourceBar.UpdateHealthBar((carryingResource / (float)maxResource));
            }
            else
            {
                MoveTowardsBase();
            }
        }
        else
        {
            bool success = UpdateBasePoint();
            if (success)
            {
                ManualSendResourcesToBase();
            }
        }
    }
    public static float minBuildDistance = 20f;
    public static float maxBuildDistance = 35f;
    private bool HasBuildTask = false;
    
    public Vector3 GetbuildingCoordinate()
    {
        return buildingCoordinate;
    }
    public bool GetHasBuildTask()
    {
        return HasBuildTask;
    }
    public bool Build()
    {
        if (buildingCoordinate != Vector3.zero)
        {
            float distance = Vector3.Distance(buildingCoordinate, transform.position);
            if ( distance >= minBuildDistance && distance <= maxBuildDistance)
            {
                StopMoving();
                StopAiming();
                var result = (bool)BuildingController.Instance?.PlacingBuildingCommand(buildingCoordinate, targetBuilding);
                if (!result)
                {
                    UtilsClass.CreateWorldTextPopup("You cannot place here", buildingCoordinate, color: Color.red);
                }
                else
                {
                    HasBuildTask = false;
                }
                    return result;
            }
        }
        return false; //building not success
    }
    public void SetBuildAtPosition(Vector3 position, Building building)
    {
        HasBuildTask = true;
        buildingCoordinate = position;
        targetBuilding = building;
    }
    public void SetBuildAtMousePosition(Building building)
    {
        HasBuildTask = true;
        buildingCoordinate = UtilsClass.GetMouseWorldPosition();
        targetBuilding = building;
    }
    protected override void Death()
    {
        if (occupyingResourcePoint != null)
        {
            occupyingResourcePoint.SetUnoccupied();
        }
        GroupsOfUnits.Instance.RemoveWorker(this);
        base.Death();
    }
    public override void Skill4()
    {
        SetWorkTypeMining();
    }
    public override void Skill5()
    {
        SetNotWorking();
    }
    //Setup Unit's Info

    private string UnitInfo = "Hello! I'm Red Blood Cell, which is a type of cell. I'm a friendly unit who automatically mines resources at resource points for friendly cell faction, and drops resources at Resource Collection Points.";
    public override string getInfo()
    {
        return UnitInfo;
    }
}