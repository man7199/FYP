using UnityEngine;

public class Template : Building
{

    // set the hp, defensive, and requiresource , time , description as below
    //  override Effect 1,2,3 for the button from top left to right
    //  4,5,6 bottom left to right
    // without descrption = empty
    // put the template script to a prefab building 
    // go to spirite to choose the image for the button
    // done.
    public GameObject prefab;
    int[] requireresource1 = { 100, 10, 10, 10, 10, 10 };
    int[] requiretime1 = { 5, 1, 1, 1, 1, 1 };
    private int count = 2;

    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        currenthp = hp;
        defensive = 1.0;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        description[0] = "Produce a unit";
        description[1] = "Increase Attack damage of the unit";
        description[2] = "Increase Defensive of the unit";
        description[3] = "Increase Attack speed of the unit";
        description[4] = "Increase health point of the unit";
      
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Effect1()
    {
        Instantiate(prefab,
            new Vector3(GetComponent<Transform>().localPosition.x + count, GetComponent<Transform>().localPosition.y,
                GetComponent<Transform>().localPosition.z), Quaternion.identity);
        count += 3;
    }

    public override void Effect2()
    {
    }
}