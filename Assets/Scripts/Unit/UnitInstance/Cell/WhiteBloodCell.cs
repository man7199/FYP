using UnityEngine;

public class WhiteBloodCell : Cell, IMelee
{

    private bool isBuilding = false;
    private BuildingGhost bg;
    protected override Node SetupBehaviorTree()
    {
        return Subtree.MeleeSubtree(this);
    }

    public void MeleeAttack(Transform target)
    {
        target.GetComponent<Unit>()?.TakeDamage(ATK, Owner, this);
    }
    protected override void Awake()
    {
        base.Awake();
        
        //description[3] = "Build thing";
        //sprite[3] = Resources.Load<Sprite>("Arts/UI/testing");
        bg = GameObject.Find("Game Manager").GetComponentInChildren<BuildingGhost>();
        icon = Resources.Load<Sprite>("Icons/Cell/whitebloodcell");
    }
    public override void Skill1()
    {
        if (!isBuilding)
        {
            base.Skill1();
        }
        else {
            UI.setClicked(false);
            bg.SetBuildingGhost();

        }
    }
    public override void Skill2()
    {
        if (!isBuilding)
        {
            base.Skill2();
        }
        else
        {
            UI.setClicked(false);
            bg.SetBuildingGhost();
        }
    }
    public override void Skill4() {
        description[0] = "Build a BoneMarrow II";
        description[1] = "Build a BoneMarrow III";
        description[2] = null;
        description[3] = null;
        description[5] = "Cancel";
        sprite[0] = Resources.Load<Sprite>("Arts/UI/testing");
        sprite[1] = Resources.Load<Sprite>("Arts/UI/testing");
        sprite[5] = Resources.Load<Sprite>("Arts/UI/testing");
        isBuilding = true;
        UI.changeUI(this);        
    }
    public override void Skill6()
    {
        if (!isBuilding)
        {
            base.Skill6();
        }
        else
        {
            ResetUI();
            UI.changeUI(this);

        }
    }
    public override void ResetUI()
    {
        Awake();
        isBuilding = false;
        description[5] = null;
    }
    
    private string UnitInfo = "Hello! I'm White Blood Cell, which is a type of cell. I'm a friendly unit who performs melee attacks to a nearby enemy.";
    public override string getInfo()
    {
        return UnitInfo;
    }
}
