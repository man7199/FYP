using UnityEngine;
public class CellWall : Building
{
    int[] requireresource1 = { 500, 1000, 2000, 0, 0, 0 };
    int[] requiretime1 = { 3, 3, 3, 0, 0, 3 };
    // Start is called before the first frame update
    void Start()
    {
        cost = 1000;
        hp = 5000;
        currenthp = hp;
        defensive = 2;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        sprites[0] = Resources.Load<Sprite>("Arts/UI/hpUp");
        sprites[1] = Resources.Load<Sprite>("Arts/UI/hpUp2");
        sprites[2] = Resources.Load<Sprite>("Arts/UI/hpUp3");
        description[0] = "Increase the HP of this building by 500";
        description[1] = "Increase the HP of this building by 1000";
        description[2] = "Increase the HP of this building by 2000";
        name = "Cell Wall";
        icon = sprites[4] = Resources.Load<Sprite>("Arts/UI/Building/cellWall");

    }

    private string Info
       = "This is cell wall, which can block enemies.";
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
        hp += 500;
        currenthp += 500;

    }
    public override void Effect2()
    {
        hp += 1000;
        currenthp += 1000;
    }
    public override void Effect3()
    {
        hp += 2000;
        currenthp += 2000;
    }
    /* public override void Effect2()
     {
         defensive = defensive + 0.1;
         requireresource[0] = requireresource[0] * 2;
         setup(hp, requireresource, requiretime);
         setDefensive(defensive);
     }*/
}