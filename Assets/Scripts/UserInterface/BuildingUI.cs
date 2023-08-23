using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class BuildingUI : UserInterface
{
    [SerializeField] protected UIProgress uiprogress;
    [SerializeField] protected UnitUI unitUI;
    [SerializeField] protected Building building;
    private Mission mission;
    public TMP_Text guiText;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        mission = GameObject.Find("Network Game Manager")?.GetComponent<Mission>();
        hpText = GameObject.Find("hpUI2").GetComponent<TMP_Text>();
        hpText.SetText("");
        hpText.GetComponentInParent<Image>().enabled = false;
        uiprogress = GameObject.Find("Progress").GetComponent<UIProgress>();
        unitUI = GameObject.Find("UnitUI").GetComponent<UnitUI>();
        guiText = GameObject.Find("Error Message").GetComponent<TMP_Text>();
        Buildingicon.enabled = false;
    }
    

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public virtual bool Skill1()
    {
        if (mission.localPlayer == mission.cellPlayerRef)
        {
            
            if (ResourceManager.Instance.ReduceCellResources(building.getrequireresource(0)))
            {
                if (building.getrequiretime(0) == 0)              
                {
                    building.Buildingprogress(0, 0);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(0), 0);
                return true;
            }
            else {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        else
        {
            if (ResourceManager.Instance.ReduceRobotResources(building.getrequireresource(0)))
            {
                if (building.getrequiretime(0) == 0)
                {

                    building.Buildingprogress(0, 0);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(0), 0);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        return false;
    }


    public virtual bool Skill2()
    {
        if (mission.localPlayer == mission.cellPlayerRef)
        { 
            if (ResourceManager.Instance.ReduceCellResources(building.getrequireresource(1)))
            {
                if (building.getrequiretime(1) == 0)
                {
                    building.Buildingprogress(0, 1);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(1), 1);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        else
        {
            if (ResourceManager.Instance.ReduceRobotResources(building.getrequireresource(1)))
            {
                if (building.getrequiretime(1) == 0)
                {
                    building.Buildingprogress(0, 1);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(1), 1);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        return false;
    }


    public virtual bool Skill3()
    {
        if (mission.localPlayer == mission.cellPlayerRef)
        {
            
             if  (ResourceManager.Instance.ReduceCellResources(building.getrequireresource(2)))
            {
                if (building.getrequiretime(2) == 0)
                {
                    building.Buildingprogress(0, 2);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(2), 2);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        else
        {
            
            if (ResourceManager.Instance.ReduceRobotResources(building.getrequireresource(2)))
            {
                if (building.getrequiretime(2) == 0)
                {
                    building.Buildingprogress(0, 2);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(2), 2);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        return false;
    }


    public virtual bool Skill4()
    {
        if (mission.localPlayer == mission.cellPlayerRef)
        {
            
            if (ResourceManager.Instance.ReduceCellResources(building.getrequireresource(3)))
            {
                if (building.getrequiretime(3) == 0)
                {
                    building.Buildingprogress(0, 3);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(3), 3);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        else
        {
            
            if (ResourceManager.Instance.ReduceRobotResources(building.getrequireresource(3)))
            {
                if (building.getrequiretime(3) == 0)
                {
                    building.Buildingprogress(0, 3);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(3), 3);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        return false;
    }

    public virtual bool Skill5()
    {
         if (mission.localPlayer == mission.cellPlayerRef)
        {
              if (ResourceManager.Instance.ReduceCellResources(building.getrequireresource(4)))
            {
                if (building.getrequiretime(4) == 0)
                {
                    building.Buildingprogress(0, 4);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(4), 4);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        else
        {
            
            if (ResourceManager.Instance.ReduceRobotResources(building.getrequireresource(4)))
            {
                if (building.getrequiretime(4) == 0)
                {
                    building.Buildingprogress(0, 4);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(4), 4);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        return false;
    }

    public virtual bool Skill6()
    {
        if (mission.localPlayer == mission.cellPlayerRef)
        {
             if (ResourceManager.Instance.ReduceCellResources(building.getrequireresource(5)))
            {
                if (building.getrequiretime(5) == 0)
                {
                    building.Buildingprogress(0, 5);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(5), 5);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        else
        {
            if (ResourceManager.Instance.ReduceRobotResources(building.getrequireresource(5)))
            {
                if (building.getrequiretime(5) == 0)
                {
                    building.Buildingprogress(0, 5);
                    return false;
                }
                building.Buildingprogress(building.getrequiretime(5), 5);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }
        }
        return false;
    }
    public virtual bool Skill7()
    {
        if (ResourceManager.Instance.ReduceRobotResources(building.getrequireresource(0)))
            {
                building.Buildingprogress(building.getrequiretime(0), 6);
                return true;
            }
            else
            {
                StartCoroutine(ShowMessage("not enough resource", 2));
            }        
        return false;
    }
    public virtual bool Skill8()
    {
        if (ResourceManager.Instance.ReduceRobotResources(building.getrequireresource(1)))
        {
            building.Buildingprogress(building.getrequiretime(1), 7);
            return true;
        }
        else
        {
            StartCoroutine(ShowMessage("not enough resource", 2));
        }
        return false;
    }
    public override string hpUI()
    {
        return "hp:" + building.getCurHP() + "/" + building.getHP();
    }

    public override void changeUI(Building build)
    {
        unitUI.changeUI();
        base.changeUI(build);
        building = build;
        if (build.Owner == Player.Instance.MyPlayerRef())
        {
            button.setbuilding(build);
            uiprogress.setbuilding(build);
            build.setUI(uiprogress);
        }
        else
        {
            uiprogress.setbuilding();
            button.setbuilding();
            uiprogress.ChangeBuildingName(build);
        }
    }

    public override void changeUI()
    {
        base.changeUI();
        if (building != null)
        {
            building.setUI();
            building = null;
        }

        button.setbuilding();
        Buildingicon.enabled = false;
        uiprogress.setbuilding();
    }

    public override void Refresh(Building b)
    {
        if (building == b)
        {
            changeUI();
            changeUI(b);
        }
    }

    public Building selected()
    {
        return building;
    }
    private IEnumerator ShowMessage(string message, float delay)
    {
        guiText.text = message;
        guiText.enabled = true;
        yield return new WaitForSeconds(delay);
        guiText.enabled = false;
    }
}