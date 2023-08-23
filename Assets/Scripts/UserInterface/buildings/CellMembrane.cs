using UnityEngine;
public class CellMembrane : Building
{
    int[] requireresource1 = { 50, 100, 200, 0, 0, 0 };
    int[] requiretime1 = { 3, 3, 3, 0, 0, 3 };
    // Start is called before the first frame update
    void Start()
    {
        cost = 100;
        hp = 2000;
        currenthp = hp;
        defensive = 2;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        sprites[0] = Resources.Load<Sprite>("Arts/UI/hpUp");
        sprites[1] = Resources.Load<Sprite>("Arts/UI/hpUp2");
        sprites[2] = Resources.Load<Sprite>("Arts/UI/hpUp3");
        description[0] = "Increase the HP of this building by 50";
        description[1] = "Increase the HP of this building by 100";
        description[2] = "Increase the HP of this building by 200";
        name = "Cell Membrane";
        icon =  Resources.Load<Sprite>("Arts/UI/Building/cellmembrane");

    }

    private string Info
= "This is cell membrane, which can block enemies.";

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
        hp += 50;
        currenthp += 50;

    }
    public override void Effect2()
    {
        hp += 100;
        currenthp += 100;
    }
    public override void Effect3()
    {
        hp += 200;
        currenthp += 200;
    }
    /* public override void Effect2()
     {
         defensive = defensive + 0.1;
         requireresource[0] = requireresource[0] * 2;
         setup(hp, requireresource, requiretime);
         setDefensive(defensive);
     }*/
}