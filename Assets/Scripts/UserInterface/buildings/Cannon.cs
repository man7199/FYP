using System.Collections;
using UnityEngine;
using Fusion;
public class Cannon : RobotBuilding
{

    [SerializeField] public int[] requireresource1 = { 100, 200, 0, 10, 10, 0 };
    [SerializeField] public int[] requiretime1 = { 5, 5, 0, 1, 1, 3 };
    [SerializeField] public float attackRange = 30;
    [SerializeField] public float aoeRange = 1.5f;
    [SerializeField] public float attackFreq = 1f;
    [SerializeField] public int ATK = 50;
    [SerializeField] private CannonModel self;
    private int _unitLayer;
    [SerializeField]  private bool _Attacking = false;
    [SerializeField] private bool _StopAttacking = false;
    [SerializeField] private BuildingUI ui;
    public NetworkPrefabRef explosion;
    // Start is called before the first frame update
    protected override void Start()
    {
        if (GetComponent<Transform>().parent == null) //building constructed
        {
            self = GetComponent<CannonModel>();
            hp = 100;
            currenthp = hp;
            icon = Resources.Load<Sprite>("Icons/RobotBuilding/cannon");
            name = "Cannon(under construct)";
            base.Start();
        }
    }

    private string Info
    = "This is cannon, which attacks enemy.";
    public override string getInfo()
    {
        return Info;
    }
    protected override void ActivateGameObject()
    {
        base.ActivateGameObject();
        self.CanAttack = true;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        icon = Resources.Load<Sprite>("Icons/RobotBuilding/cannon");
        sprites[0] = Resources.Load<Sprite>("Arts/UI/atkUp");
        sprites[1] = Resources.Load<Sprite>("Arts/UI/atkUp");
        sprites[2] = Resources.Load<Sprite>("Arts/UI/stopUI");
        ATK = 50;
        description[0] = "Increase the ATK of this building by 10(Current ATK:" + ATK + ")";
        description[1] = "Increase the ATK of this building by 20(Current ATK:"+ATK+")";
        description[2] = "Stop Attacking";
        name = "Cannon";
        _unitLayer = Global.UNIT_MASK;
        ui = GameObject.Find("ProgressUI").GetComponent<BuildingUI>();
    }
        public void EnemyFound()
    {
        StartCoroutine(Attack());
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    private IEnumerator Attack()
    {
        if (self.canFire && !_Attacking) {
            if (self.go_target.GetComponent<Unit>()!=null) {
                RpcSmash();
                self.explosion.transform.position = self.go_target.position;
                _Attacking = true;
                RPCShowEffect(self.go_target.position);
            }
            yield return new WaitForSeconds(1 / attackFreq);
            _Attacking = false;
            StartCoroutine(Attack());
        }        
    }
    
    public override void Effect1()
    {
        ATK += 10;
        description[0] = "Increase the ATK of this building by 10(Current ATK:" + ATK + ")";
        description[1] = "Increase the ATK of this building by 20(Current ATK:" + ATK + ")";
        ui.Refresh(this);
    }

    public override void Effect2()
    {
        ATK += 20;
        description[0] = "Increase the ATK of this building by 10(Current ATK:" + ATK + ")";
        description[1] = "Increase the ATK of this building by 20(Current ATK:" + ATK + ")";
        ui.Refresh(this);
    }
    public override void Effect3()
    {
        if (!_StopAttacking)
        {
            _Attacking = true;
            self.CanAttack = false;
            description[2] = "Restore Attacking";            
            _StopAttacking = true;
            RPCStop();
            ui.Refresh(this);
        }
        else {
            _Attacking = false;
            self.CanAttack = true;
            description[2] = "Stop Attacking";
            _StopAttacking = false;
            RPCContinue();
            ui.Refresh(this);
        }

    }

    public void RpcSmash()
    {
        Collider[] colliders = Physics.OverlapSphere(self.go_target.transform.position, aoeRange, _unitLayer);
        foreach (Collider col in colliders)
        {
            Unit unit = col.GetComponent<Unit>();
            if (unit.Owner != Owner)
            {
                unit.TakeDamage(ATK, Owner, unit);
            }
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCShowEffect(Vector3 position)
    {
        Runner.Spawn(explosion, position, Quaternion.identity);
        AudioManager.Play("explode", AudioManager.MixerTarget.UI, position);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCStop()
    {
        self.canFire = false;
        _Attacking = true;
        _StopAttacking = true;
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    public void RPCContinue()
    {
        self.canFire = false;
        _Attacking = false;
        _StopAttacking = false;
    }

}