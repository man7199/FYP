using Fusion;
using UnityEngine;

public class HealthCenter : RobotBuilding
{
    [Networked] public NetworkPrefabRef prefab { get; set; }
    int[] requireresource1 = { 100, 200, 350, 500, 1000, 2000 };
    int[] requiretime1 = { 30, 30, 30, 30, 30, 30 };
    HealthSystem health;

    // Start is called before the first frame update
    protected override void Start()
    {
        if (GetComponent<Transform>().parent == null)
        {
            hp = 10;
            icon = Resources.Load<Sprite>("Icons/RobotBuilding/healthcenter");
            name = "Health Center(under construct)";
            base.Start();
        }
    }
    protected override void ActivateGameObject()
    {
        base.ActivateGameObject();
        hp = 10;
        currenthp = hp;
        defensive = 1.0;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        description[0] = "Wash hand regularly (+5Health Index)";
        description[1] = "Avoid smoking (+10Health Index)";
        description[2] = "Maintain healthy weight (+15Health Index)";
        description[3] = "Get enough sleep (+20Health Index)";
        description[4] = "Exercise regularly (+25Health Index)";
        description[5] = "Eat healthy diet (+30Health Index)";
        sprites[0] = Resources.Load<Sprite>("Sprites/healthyaction1");
        sprites[1] = Resources.Load<Sprite>("Sprites/healthyaction2");
        sprites[2] = Resources.Load<Sprite>("Sprites/healthyaction3");
        sprites[3] = Resources.Load<Sprite>("Sprites/healthyaction4");
        sprites[4] = Resources.Load<Sprite>("Sprites/healthyaction5");
        sprites[5] = Resources.Load<Sprite>("Sprites/healthyaction6");
        name = "Health Center";
        health = GameObject.Find("HealthSystem").GetComponent<HealthSystem>();

    }

    private string Info
        = "This is Health Center, which you can consume resources to boost your units.";
    public override string getInfo()
    {
        return Info;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Effect1()
    {
        health.Change(5);
    }
    public override void Effect2()
    {
        health.Change(10);
    }
    public override void Effect3()
    {
        health.Change(15);
    }
    public override void Effect4()
    {
        health.Change(20);
    }
    public override void Effect5()
    {
        health.Change(25);
    }
    public override void Effect6()
    {
        health.Change(30);
    }
}