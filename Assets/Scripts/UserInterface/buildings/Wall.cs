public class Wall : Building
{
    int[] requireresource1 = { 100, 10, 0, 0, 0, 0 };
    int[] requiretime1 = { 3, 3, 0, 0, 0, 0 };
    // Start is called before the first frame update
    void Start()
    {
        hp = 1000;
        currenthp = hp;
        defensive = 2;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        description[0] = "Increase the health point of this building by 1000";
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Effect1()
    {
        hp += 1000;
      
    }
    /* public override void Effect2()
     {
         defensive = defensive + 0.1;
         requireresource[0] = requireresource[0] * 2;
         setup(hp, requireresource, requiretime);
         setDefensive(defensive);
     }*/
}