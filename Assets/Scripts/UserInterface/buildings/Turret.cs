using System.Collections;
using UnityEngine;
using Fusion;
public class Turret : RobotBuilding
{

    int[] requireresource1 = { 250, 2000, 0, 10, 10, 0 };
    int[] requiretime1 = { 3, 3, 0, 1, 1, 3 };   
    [SerializeField] public float attackRange = 50;
    [SerializeField] public float attackFreq = 1;
    [SerializeField] public int ATK = 20;
    [SerializeField] private GatlingGun self;
    [SerializeField] private int attackTimes = 1;
    [SerializeField] private BuildingUI ui;
    private bool _StopAttacking = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        if (GetComponent<Transform>().parent == null)
        {
            icon = Resources.Load<Sprite>("Icons/RobotBuilding/turret");
            name = "Turret(under construct)";
            self = GetComponent<GatlingGun>();
            base.Start();
        }
    }
    protected override void ActivateGameObject()
    {
        base.ActivateGameObject();
        self.canAttack = true;
        hp = 100;
        currenthp = hp;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        icon = Resources.Load<Sprite>("Icons/RobotBuilding/turret");
        sprites[0] = Resources.Load<Sprite>("Arts/UI/atkUp");
        sprites[1] = Resources.Load<Sprite>("Arts/UI/turretUp");
        sprites[2] = Resources.Load<Sprite>("Arts/UI/stopui");
        description[0] = "Increase the ATK of this building by 10(Current Atk:"+ATK+")";
        description[1] = "Increase attack times per attack by 1(Current AtkTimes:" + attackTimes+ ")";
        description[2] = "Stop attacking";
        name = "Turret";
        ui = GameObject.Find("ProgressUI").GetComponent<BuildingUI>();
        StartCoroutine(Attack());
    }

    private string Info
 = "This is turret, which attacks enemy.";
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
        if (self.canFire) {
            if (self.go_target.GetComponent<Unit>()!=null) {
                for (int i = 0; i < attackTimes; i++)
                {
                    self.go_target.GetComponent<Unit>().TakeDamage(ATK, Owner, self.go_target.GetComponent<Unit>());
                }
                RPCShowEffect( transform.position);
            }
        }
        yield return new WaitForSeconds(1/attackFreq);
        StartCoroutine(Attack());
    }
    
    public override void Effect1()
    {
        ATK += 10;
        description[0] = "Increase the ATK of this building by 10(Current Atk:" + ATK + ")";
        description[1] = "Increase attack times per attack by 1(Current AtkTimes:" + attackTimes + ")";
        ui.Refresh(this);
    }

    public override void Effect2()
    {
        attackTimes += 1;
        self.barrelRotationSpeed *= 2;
        description[0] = "Increase the ATK of this building by 10(Current Atk:" + ATK + ")";
        description[1] = "Increase attack times per attack by 1(Current AtkTimes:" + attackTimes + ")";

        ui.Refresh(this);
    }
    public override void Effect3()
    {
        if (!_StopAttacking)
        {
            self.canFire = false;
            self.canAttack = false;
            _StopAttacking = true;
            RPCStop();
            description[2] = "Restore Attacking";
            ui.Refresh(this);
        }
        else
        {
            self.canFire = false;
            self.canAttack = false;
            _StopAttacking = false;
            RPCContinue();
            description[2] = "Stop Attacking";
            ui.Refresh(this);
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCShowEffect(Vector3 position)
    {        
        AudioManager.Play("turret", AudioManager.MixerTarget.UI, position);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCStop()
    {
        self.canFire = false;
        self.canAttack = false;
        _StopAttacking = true;
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCContinue()
    {
        self.canFire = false;
        self.canAttack = false;
        _StopAttacking = false;
    }
}