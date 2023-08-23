using System.Collections;
using UnityEngine;

public class RepairFactory : RobotBuilding
{

    [SerializeField] public int[] requireresource1 = { 200, 1000, 10, 10, 10, 0 };
    [SerializeField] public int[] requiretime1 = { 5, 5, 1, 1, 1, 3 };
    [SerializeField] public float attackRange = 10;
    [SerializeField] public float attackFreq = 1;
    [SerializeField] public int ATK = 50;
    private int _unitLayer;
    // Start is called before the first frame update
    protected override void Start()
    {
        if (GetComponent<Transform>().parent == null)
        {
            name = "Repair Factory(under construct)";
            icon = Resources.Load<Sprite>("Icons/RobotBuilding/repairfactory");
            base.Start();
        }
    }
    protected override void ActivateGameObject()
    {
        base.ActivateGameObject();
        hp = 100;
        currenthp = hp;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        sprites[0] = Resources.Load<Sprite>("Arts/UI/hpUp");
        sprites[1] = Resources.Load<Sprite>("Arts/UI/resourceSpeedup");
        description[0] = "Increase the Healing of this building by 10";
        description[1] = "Increase the Healing speed of this building by 0.1second";
        name = "Repair Factory";
        StartCoroutine(Attack());
        _unitLayer = Global.UNIT_MASK;

    }

    private string Info
 = "This is Repair Factory which can heal friendly units.";
    public override string getInfo()
    {
        return Info;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    private IEnumerator Attack()
    {
       
                RpcSmash();
        yield return new WaitForSeconds(1/attackFreq);
        StartCoroutine(Attack());
    }
    
    public override void Effect1()
    {
        ATK += 10;
    }

    public override void Effect2()
    {
        attackFreq += 0.1f;
    }

    public void RpcSmash()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, _unitLayer);
        foreach (Collider col in colliders)
        {
            Unit unit = col.GetComponent<Unit>();
            if (unit.Owner == Owner)
            {
                unit.regenHP(ATK);
            }
        }
    }
}