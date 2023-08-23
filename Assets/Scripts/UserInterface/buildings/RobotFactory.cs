using Fusion;
using UnityEngine;

public class RobotFactory : RobotBuilding
{
    [Networked] public NetworkPrefabRef prefab1 { get; set; }
    [Networked] public NetworkPrefabRef prefab2 { get; set; }
    [Networked] public NetworkPrefabRef prefab3 { get; set; }
    int[] requireresource1 = { 100, 100, 250, 10, 10, 0 };
    int[] requiretime1 = { 5, 5, 5, 1, 1, 3 };
    private int count = 2;

    // Start is called before the first frame update
    protected override void Start()
    {
        if (GetComponent<Transform>().parent == null)
        {
            hp = 10;
            currenthp = hp;
            defensive = 1.0;
            icon = Resources.Load<Sprite>("Icons/RobotBuilding/robotfactory");
            name = "Robot Factory(under construct)";
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
        icon = Resources.Load<Sprite>("Icons/RobotBuilding/robotfactory");
        sprites[0] = Resources.Load<Sprite>("Icons/Robot/minerobot");
        sprites[1] = Resources.Load<Sprite>("Icons/Robot/eye_robot");
        sprites[2] = Resources.Load<Sprite>("Arts/Robot/range_robot");

        description[0] = "produce a mine robot";
        description[1] = "produce a Artillery Robot";
        description[2] = "produce a Shooter Robot";
        name = "Robot Factory";
    }

    private string Info
 = "This is a Robot Factory which produces several types of robots, including Mine Robot, Artillery Robot, Shooter Robot.";
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
        RpcSpawnUnit(prefab1, Runner.LocalPlayer);
    }
    public override void Effect2()
    {
        RpcSpawnUnit(prefab2, Runner.LocalPlayer);

    }
    public override void Effect3()
    {
        RpcSpawnUnit(prefab3, Runner.LocalPlayer);

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

    
}