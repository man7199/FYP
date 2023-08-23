using Fusion;
using UnityEngine;

public class HeroFactory : RobotBuilding
{
    [Networked] public NetworkPrefabRef prefab1 { get; set; }
    [Networked] public NetworkPrefabRef prefab2 { get; set; }
    [Networked] public NetworkPrefabRef prefab3 { get; set; }
    int[] requireresource1 = { 750, 750, 750, 0, 0, 0 };
    int[] requiretime1 = { 0, 0, 0, 5, 5, 5 };
    private int count = 2;
    private GameLevel gamelevel;
    // Start is called before the first frame update
    protected override void Start()
    {
        if (GetComponent<Transform>().parent == null)
        {
            name = "Hero Factory(under construct)";
            icon = Resources.Load<Sprite>("Icons/RobotBuilding/herofactory");
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
        sprites[0] = Resources.Load<Sprite>("Arts/Robot/cannondrone");
        sprites[1] = Resources.Load<Sprite>("Arts/Robot/utility_robot");
        sprites[2] = Resources.Load<Sprite>("Arts/Robot/tankdrone");
        description[0] = "produce a Cannon Hero";
        description[1] = "produce a Support Hero";
        description[2] = "produce a Tank Hero";
        name = "Hero Factory";
        gamelevel = GameLevel.Instance;
        Refresh();

    }
    // Update is called once per frame

    private string Info
= "This is Hero Factory which produces hero robots.";
    public override string getInfo()
    {
        return Info;
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void Effect1()
    {
        if (!gamelevel.isCannonHeroProduced)
        {
            Buildingprogress(5, 0);
            count += 2;
            gamelevel.RpcCannon(true);
        }
        else {
            RpcSpawnUnit(prefab1, Runner.LocalPlayer);
        }
        Invoke("Refresh",0.1f);
    }
    public override void Effect2()
    {
        if (!gamelevel.isSupportHeroProduced)
        {
            Buildingprogress(5, 1);
            count += 2;
            gamelevel.RpcSupport(true);
        }
        else {
            RpcSpawnUnit(prefab2, Runner.LocalPlayer);
        }
        Invoke("Refresh", 0.1f);
    }  
    
    public override void Effect3()
    {
        if (!gamelevel.isTankHeroProduced)
        {
            Buildingprogress(5, 2);
            count += 2;
            gamelevel.RpcTank(true);
        }
        else {
            RpcSpawnUnit(prefab3, Runner.LocalPlayer);
        }
        Invoke("Refresh", 0.1f);
    }
   
    void Refresh() {
        if (gamelevel.isCannonHeroProduced)
        {
            description[0] = null;
        }
        if (gamelevel.isSupportHeroProduced)
        {
            description[1] = null;
        }
            if (gamelevel.isTankHeroProduced)
        {
            description[2] = null;
        }
        GameObject.Find("ProgressUI").GetComponent<BuildingUI>().Refresh(this);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RpcSpawnUnit(NetworkPrefabRef prefabRef, PlayerRef playerRef)
    {
        Vector3 position = new Vector3(GetComponent<Transform>().localPosition.x + count,
            GetComponent<Transform>().localPosition.y, GetComponent<Transform>().localPosition.z);
        NetworkObject newObject = Runner.Spawn(prefabRef, position, Quaternion.identity);

        //todo use network input to spawn unit
        Unit unit = newObject.GetComponent<Unit>();
        unit.Owner = playerRef;
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RpcCannon()
    {
        gamelevel.isCannonHeroProduced = true;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RpcSupport()
    {
        gamelevel.isSupportHeroProduced = true;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RpcTank()
    {
        gamelevel.isTankHeroProduced = true;
    }

}