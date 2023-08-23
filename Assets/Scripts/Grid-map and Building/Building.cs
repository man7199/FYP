using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(NetworkObject), typeof(NetworkTransform))]
public class Building : NetworkBehaviour
{
    [SerializeField] private bool cannotRemove;
    [SerializeField] public int hp = 100;

    public enum BuildingType
    {
        Default,
        AllyProducer,
        ResourceCollector,
        Tower,
        Wall,
        Base
    }

    public NetworkPrefabRef prefabRef;
    [Networked] public PlayerRef Owner { get; set; }
    public bool isMembrane;
    public BuildingController.Direction direction;
    public int x;
    public int y;
    public int width, height;
    public string nameString;
    public BuildingType buildingType;
    public int cost;
    public int buildTime = 0;
    public string info = "Building's info should be displayed here";


    private readonly float[] buildingTime = { 0, 0, 0, 0, 0 };
    public int currentTask;
    public readonly float[] currentTime = { 0, 0, 0, 0, 0 };
    private readonly bool[] isBuilding = { false, false, false, false, false };
    protected readonly int[] whichfunc = { 0, 0, 0, 0, 0 };


    [SerializeField] protected double defensive = 1.0;
    [SerializeField] protected int[] requireresource = { 0, 0, 0, 0, 0, 0 };
    [SerializeField] protected int[] requiretime = { 0, 0, 0, 0, 0, 0 };
    public int currenthp = 100;
    public Sprite[] sprites = { null, null, null, null, null, null };
    public string[] description = { null, null, null, null, null, null };

    [SerializeField] protected UIProgress uiprogress = null;
    public Sprite icon;

    public override void Spawned()
    {
        base.Spawned();
        sprites[5] = Resources.Load<Sprite>("Arts/UI/stopui");
        description[5] = "Destroy building";
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isBuilding[0])
        {
            currentTime[0] -= Time.deltaTime;

            if (currentTime[0] <= 0)
            {
                switch (whichfunc[0])
                {
                    case 0:
                        Effect1();
                        break;
                    case 1:
                        Effect2();
                        break;
                    case 2:
                        Effect3();
                        break;
                    case 3:
                        Effect4();
                        break;
                    case 4:
                        Effect5();
                        break;
                    case 5:
                        Effect6();
                        break;
                }

                for (var i = 0; i < currentTask - 1; i++)
                {
                    currentTime[i] = currentTime[i + 1];
                    buildingTime[i] = buildingTime[i + 1];
                    whichfunc[i] = whichfunc[i + 1];
                    isBuilding[i] = isBuilding[i + 1];
                }

                currentTask--;
                isBuilding[currentTask] = false;

                if (uiprogress != null)
                {
                    uiprogress.deQueue(GetComponent<Building>());
                }
            }
        }
    }

    public void Buildingprogress(int time, int which)
    {
        if (time == 0) { 
                switch (which)
            {
                case 0:
                    Effect1();
                    break;
                case 1:
                    Effect2();
                    break;
                case 2:
                    Effect3();
                    break;
                case 3:
                    Effect4();
                    break;
                case 4:
                    Effect5();
                    break;
                case 5:
                    Effect6();
                    break;
                case 6:
                    Effect1();
                    break;
            }

        }
        else
        {
            whichfunc[currentTask] = which;
            buildingTime[currentTask] = time;
            currentTime[currentTask] = time;
            isBuilding[currentTask] = true;
            currentTask++;
        }
    }

    public bool fullprogress()
    {
        if (currentTask >= 5)
            return true;
        return false;
    }

    public float progressUI()
    {
        if (isBuilding[0])
            return currentTime[0] / buildingTime[0];
        return 0;
    }

    public int getCurrent()
    {
        return currentTask;
    }

    public virtual void setUI(UIProgress x)
    {
        uiprogress = x;
        for (var i = 0; i < currentTask; i++) uiprogress.addQueue(sprites[whichfunc[i]]);
    }

    public void setUI()
    {
        uiprogress = null;
    }

    public Sprite[] getSprites()
    {
        return sprites;
    }

    public string getDescription(int x)
    {
        return description[x];
    }

    public string getName()
    {
        return GetType().Name;
    }

    public int getHP()
    {
        return hp;
    }

    public int getCurHP()
    {
        return currenthp;
    }

    public int getrequireresource(int x)
    {
        return requireresource[x];
    }

    public int getrequiretime(int x)
    {
        return requiretime[x];
    }

    public void setrequireresource(int[] x)
    {
        requireresource = x;
    }

    public void setrequiretime(int[] x)
    {
        requiretime = x;
    }

    public int getWidth()
    {
        return width;
    }

    public int getHeight()
    {
        return height;
    }

    public int getCost()
    {
        return cost;
    }

    public int getBuildTime()
    {
        return buildTime;
    }

    public virtual string getInfo()
    {
        return info;
    }

    public virtual void Effect1()
    {
    }

    public virtual void Effect2()
    {
    }

    public virtual void Effect3()
    {
    }

    public virtual void Effect4()
    {
    }

    public virtual void Effect5()
    {
    }

    public virtual void Effect6()
    {
        if (!cannotRemove)
        {
            BuildingController.Instance.DestroyBuilding(this);
        }
    }

    public BuildingType getType()
    {
        return buildingType;
    }

    public void StopButton(int x)
    {
    }

    public void ReloadButton(int x)
    {
    }
    public virtual void OnDestroy()
    {
        if (GameObject.Find("ProgressUI")== true)
        {
            if(GameObject.Find("ProgressUI").GetComponent<BuildingUI>().selected() == this)
            {
                GameObject.Find("ProgressUI").GetComponent<BuildingUI>().changeUI();
            }
        }

    }

}