using Fusion;
using UnityEngine;

public class AllyProducer : Building
{
    [Networked] public NetworkPrefabRef prefab1 { get; set; }
    [Networked] public NetworkPrefabRef prefab2 { get; set; }
    [Networked] public NetworkPrefabRef prefab3 { get; set; }

    int[] requireresource1 = { 100, 10, 10, 10, 10, 10 };
    int[] requiretime1 = { 1, 1, 1, 1, 1, 1 };

    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        currenthp = hp;
        defensive = 1.0;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        
        // description[0] = "Produce a unit";
        // description[1] = "Increase Attack damage of the unit";
        // description[2] = "Increase Defensive of the unit";
        // description[3] = "Increase Attack speed of the unit";
        // description[4] = "Increase health point of the unit";
        description[0] = "Tank";
        description[1] = "Support";
        description[2] = "Tank";
        description[3] = "Increase Attack speed of the unit";
        description[4] = "Increase health point of the unit";
        name = "Ally Producer";
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
    public override void Effect4()
    {
    }
    public override void Effect5()
    {
    }
    public override void Effect6()
    {
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RpcSpawnUnit(NetworkPrefabRef prefabRef, PlayerRef playerRef)
    {
        Vector3 position = new Vector3(transform.position.x + 5 + Random.value *3,
            transform.position.y, transform.position.z -5 + Random.value *3);
        NetworkObject newObject = Runner.Spawn(prefabRef, position, Quaternion.identity);
        Unit unit = newObject.GetComponent<Unit>();
        unit.Owner = playerRef;
    }


    
}