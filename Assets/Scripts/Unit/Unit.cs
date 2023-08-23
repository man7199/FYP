using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NetworkRigidbody), typeof(NetworkObject), typeof(Rigidbody))]
[RequireComponent(typeof(BuffableEntity))]
public abstract class Unit : NetworkBehaviour
{
    [Networked] [UnitySerializeField] public PlayerRef Owner { get; set; }

    [SerializeField] public bool canPassWall;


    [SerializeField] public float scoutRange = 10;
    [SerializeField] public float scoutMaxRange = 10;
    [SerializeField] public float attackRange = 10;
    [SerializeField] public float minAttackRange = 0;
    [SerializeField] public float attackFreq = 1;
    [Networked] [UnitySerializeField] public int ATK { get; set; } //attackDamage
    [Networked] [UnitySerializeField] protected int defense { get; set; }
    [Networked] [UnitySerializeField] public float moveSpeed { get; set; }
    [SerializeField] public int ATK_Dict = 10;
    [SerializeField] public int Denfense_Dict = 10;
    [SerializeField] public int MoveSpeed_Dict = 10;
    [SerializeField] public int maxHP;
    [SerializeField] protected int InitialMaxHP = 100;
    [SerializeField] protected float AOEmaxRange = 30;
    [SerializeField] public TeamType teamType;

    public float explodeRange;

    [Networked(OnChanged = nameof(OnHPChanged))]
    [UnitySerializeField]
    protected int HP { get; set; } = 100; //current HP

    [SerializeField] protected UnitType unitType = UnitType.None;
    [SerializeField] HealthBar1 healthBar;

    // status
    public bool IsStunned { get; private set; } = false;
    public float stunTime;

    public BuffableEntity buffableEntity;
    public Transform target;
    public UnitGroup unitGroup; // for pathfinding
    public int unitTeamNum = -1; // for forming team

    public Sprite[] sprite = { null, null, null, null, null, null };
    public string[] description = { null, null, null, null, null, null };
    public UnitUI UI;
    public PlayerControl playerControl;
    public bool isForcedMovement = false;


    private Node _behaviorTree;
    private Rigidbody _rb;
    private GameObject _selectionCircle;
    private bool _selected;
    protected bool _isAoe = false;
    protected bool _isBuild = false;
    public Sprite icon;

    public virtual bool isAoe()
    {
        return _isAoe;
    }

    public virtual Canvas AOEcircle()
    {
        return null;
    }

    public virtual Canvas AOEfixed()
    {
        return null;
    }

    public virtual float AOEoutsidelimit()
    {
        return AOEmaxRange;
    }

    // set up the behavior tree
    protected abstract Node SetupBehaviorTree();

    protected virtual void Update()
    {
        if (_behaviorTree != null)
        {
            _behaviorTree.Evaluate();
        }
    }

    protected virtual void Awake()
    {
        buffableEntity = GetComponent<BuffableEntity>();
        _rb = GetComponent<Rigidbody>();
        _selectionCircle = transform.Find("model/Selection Circle")?.gameObject;
        healthBar = GetComponentInChildren<HealthBar1>();
        SetSelection(false);
        UI = GameObject.Find("UnitUI").GetComponent<UnitUI>();
        playerControl = GameObject.Find("Game Manager").GetComponent<PlayerControl>();

        description[0] = "Command: move";
        description[1] = "Command: stop moving";
        description[2] = "Command: patrol";
        sprite[0] = Resources.Load<Sprite>("Arts/UI/moveui");
        sprite[1] = Resources.Load<Sprite>("Arts/UI/stopui");
        sprite[2] = Resources.Load<Sprite>("Arts/UI/patrolui");
    }


    public override void Spawned()
    {
        maxHP = InitialMaxHP;
        _behaviorTree = SetupBehaviorTree();
    }

    public bool IsSelected
    {
        get => _selected;
    }


    public Sprite[] SkillImage()
    {
        return sprite;
    }

    public string getDescription(int x)
    {
        return description[x];
    }

    public enum UnitType
    {
        None,
        ExplodeEnemy,
        MeleeEnemy,
        Solder1,
        Corona,
        Bacteria,
        KillerCell,
        NKCell,
    }

    [SerializeField]
    public enum TeamType
    {
        Friendly, //player team
        Enemy //enemy team
    }


    public void Move(PathGrid pathGrid)
    {
        var cell = pathGrid.GetCell(transform.position);
        Vector3 moveDirection = new Vector3(cell.bestDirection.vector.x, 0, cell.bestDirection.vector.y);
        if (moveDirection.magnitude != 0)
        {
            Quaternion quaternion = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion, 200 * Time.deltaTime);
        }

        if (pathGrid.GetCostGridArray()[cell.x, cell.y] == 2)
        {
            _rb.velocity = moveDirection.normalized * moveSpeed / 3;
        }
        else
        {
            _rb.velocity = moveDirection.normalized * moveSpeed;
        }
    }

    public void TurnTowardsCoordinate(Vector3 position)
    {
        Vector3 positionDirection = (position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(positionDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 2000);
    }

    public static void OnHPChanged(Changed<Unit> changed)
    {
        changed.Behaviour.OnHPChanged();
    }

    private void OnHPChanged()
    {
        if (healthBar != null)
            healthBar.UpdateHealthBar((HP / (float)maxHP));
    }

    // return true if it is dead
    // TakeDamage need the playerRef of the player who deal the damage. We can use it to trace back who kill the unit. 
    public virtual bool TakeDamage
        (int damage, PlayerRef playerRef, Unit unit)
    {
        var damageTaken = damage * (100 - defense) / 100;
        HP = HP - damageTaken;
        if (target == null&&unit!=null)
        {
            target = unit.transform;
        }

        if (HP <= 0) //unit dead
        {
            Invoke(nameof(Death), 0.1f);
            return true; //target dead
        }

        return false; //target not dead
    }

    public virtual bool TakeDamageWithoutPlayerRef
        (int damage)
    {
        HP = HP - damage;
        if (HP <= 0) //unit dead
        {
            Invoke(nameof(Death), 0.1f);
            return true; //target dead
        }

        return false; //target not dead
    }

    public bool TakePoisonDamage
        (int damage, PlayerRef playerRef, Unit unit)
    {
        HP = HP - damage;
        if (HP <= 0) //unit dead
        {
            Invoke(nameof(Death), 0.1f);
            return true;
        }

        return false;
    }

    public void TakePoison(int poisonDamage, float interval, int poisonNumber, PlayerRef playerRef, Unit unit)
    {
        StartCoroutine(Poison(poisonDamage, interval, poisonNumber, playerRef, unit));
    }

    IEnumerator Poison(int poisonDamage, float interval, int poisonNumber, PlayerRef playerRef, Unit unit)
    {
        for (int i = 0; i < poisonNumber; i++)
        {
            TakePoisonDamage(poisonDamage, playerRef, unit);
            yield return new WaitForSeconds(interval);
        }
    }

    public UnitType GetUnitType()
    {
        return unitType;
    }

    public void SetSelection(bool isSelect)
    {
        _selected = isSelect;
        _selectionCircle?.SetActive(isSelect);
    }

    // Killed by player
    protected virtual void Death(PlayerRef playerRef)
    {
        // record the kill
        // StatisticRecorder.Instance.AddKillRecord(playerRef, GetType());
        Death();
    }

    // Killed by computer or self-destruction
    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (UI.selected() == this && UI != null)
        {
            UI.changeUI();
        }

        DestroyHandle();
    }

    protected void DestroyHandle()
    {
        if (unitGroup != null) unitGroup.RemoveUnit(this);
        UnitSelection.Instance.DeselectUnit(this);
    }

    public void MoveBackward()
    {
        this.GetComponent<Rigidbody>().velocity = -transform.forward * moveSpeed;
    }

    public void MoveForward()
    {
        this.GetComponent<Rigidbody>().velocity = transform.forward * moveSpeed;
    }

    public int getHP()
    {
        return HP;
    }

    public int getInitialHP()
    {
        return InitialMaxHP;
    }

    public void regenHP(int amount)
    {
        if (amount < 0)
        {
            Debug.Log("regen HP receiving negtive value");
        }
        else
        {
            if (HP + amount > maxHP)
            {
                HP = maxHP;
            }
            else
            {
                HP += amount;
            }
        }
    }

    public int getATK()
    {
        return ATK;
    }

    public int getATK_Dict()
    {
        return ATK_Dict;
    }

    public float getATKSpeed()
    {
        return attackFreq;
    }

    public float getATKRange()
    {
        return attackRange;
    }

    public float getMinATKRange()
    {
        return minAttackRange;
    }

    public int getDefensive()
    {
        return defense;
    }

    public int getDef_Dict()
    {
        return Denfense_Dict;
    }

    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    public int getMovement_Dict()
    {
        return MoveSpeed_Dict;
    }

    public string getName()
    {
        return GetType().Name;
    }

    public abstract string getInfo();

    public virtual void WakeUpFromStun()
    {
    }

    public virtual void Stunning()
    {
        _rb.velocity = new Vector3(0, 0, 0);
    }

    public virtual void ResetUI()
    {
    }

    public virtual void Skill1()
    {
        UI.cursorAim();
        UI.setClicked(false);

        IEnumerator Effect()
        {
            while (!UI.checkClicked())
            {
                yield return new WaitForSeconds(.1f);
            }

            playerControl.Move();
            UI.cursorDef();

            if (UI.checkCancel())
            {
                UI.resetCancel();
            }
            else

            {
                playerControl.Move();
            }
        }

        StartCoroutine(Effect());
    }

    public virtual void Skill2()
    {
        UnitController.Instance.StopUnit(this);
    }

    public virtual void Skill3()
    {
        UI.cursorAim();
        UI.setClicked(false);

        IEnumerator Effect()
        {
            while (!UI.checkClicked())
            {
                yield return new WaitForSeconds(.1f);
            }

            playerControl.Patrol();

            UI.cursorDef();

            if (UI.checkCancel())
            {
                UI.resetCancel();
            }
            else
            {
                playerControl.Patrol();
            }
        }

        StartCoroutine(Effect());
    }

    public virtual void Skill4()
    {
    }

    public virtual void Skill5()
    {
    }

    public virtual void Skill6()
    {
    }

    private Building building;

    public virtual void Build(Vector3 position, Building building)
    {
        this.building = building;
        if (_isBuild)
        {
            StopAllCoroutines();
            StartCoroutine(MoveAndBuild(position));
        }
        else
        {
            _isBuild = true;
            StartCoroutine(MoveAndBuild(position));
        }
    }

    public virtual void StopBuild()
    {
        StopAllCoroutines();
        _isBuild = false;
    }

    private IEnumerator MoveAndBuild(Vector3 pos)
    {
        if (Vector3.Distance(transform.position, pos) < 10)
        {
            BuildingController.Instance.PlacingBuildingCommand(pos, building, this);
            UnitController.Instance.StopUnit(this);
            _isBuild = false;
        }
        else
        {
            yield return new WaitForSeconds(0.03f);
            StartCoroutine(MoveAndBuild(pos));
        }
    }
}