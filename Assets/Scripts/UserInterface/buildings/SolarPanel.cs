using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SolarPanel : RobotBuilding
{
    private float delay = 15;
    int[] requireresource1 = { 500, 500, 10, 10, 10, 0 };
    int[] requiretime1 = { 5, 5, 1, 1, 1, 3 };
    private int resourceGen = 50;
    private ResourceManager resourceman;
    // Start is called before the first frame update
    protected override void Start()
    {
        if (GetComponent<Transform>().parent == null)
        {
            
            name = "Solar Panel(under construct)";
            icon = Resources.Load<Sprite>("Icons/RobotBuilding/solarpanel");
            base.Start();            
        }
        RobotBase.solarPanelCount++;
    }

    protected override void ActivateGameObject()
    { 
        base.ActivateGameObject();
        resourceman = GameObject.Find("Game Manager")?.GetComponent<ResourceManager>();
        hp = 10;
        currenthp = hp;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        description[0] = "Increase the resource Generation of this building by 50";
        description[1] = "Increase resource Generation speed of this building by 0.1second";
        StartCoroutine(Gen());
        name = "Solar Panel";

}

    private string Info
= "This is a solar panel which generate resources";
    public override string getInfo()
    {
        return Info;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    private void OnDestroy()
    {
        base.OnDestroy();
        RobotBase.solarPanelCount--;
    }
    public override void Effect1()
    {
        resourceGen += 50;
    }

    public override void Effect2()
    {
        delay -= 0.1f;
    }

    private IEnumerator Gen()
    {
       
        yield return new WaitForSeconds(delay);
        ResourceManager.Instance.AddRobotResources(resourceGen);
        StartCoroutine(Gen());
    }

}