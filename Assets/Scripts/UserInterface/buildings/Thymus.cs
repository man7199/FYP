using Fusion;
using UnityEngine;

public class Thymus : Building
{
    [Networked] public NetworkPrefabRef killercell { get; set; }
    [Networked] public NetworkPrefabRef TCell { get; set; }
    public Transform spawnPoint;

    int[] requireresource1 = { 250, 250, 10, 10, 10, 0 };
    int[] requiretime1 = { 5, 5, 1, 1, 1, 3 };
    private int count = 2;

    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        currenthp = hp;
        defensive = 1.0;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        
        description[0] = "Produce a killer cell";
        sprites[0] = Resources.Load<Sprite>("Arts/UI/Unit/killerCell");

        description[1] = "Produce a Tcell";
        sprites[1] = Resources.Load<Sprite>("Arts/UI/Unit/TCell");
        icon = Resources.Load<Sprite>("Arts/UI/Building/thymus");
        name = "Thymus";


    }


    private string Info
    = "This is Thymus, which produces killer cell and Tcell.";
    public override string getInfo()
    {
        return Info;
    }

    public override void Effect1()
    {
        RpcSpawnUnit(killercell, Runner.LocalPlayer);
    }
    public override void Effect2()
    {
        RpcSpawnUnit(TCell, Runner.LocalPlayer);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RpcSpawnUnit(NetworkPrefabRef prefabRef, PlayerRef playerRef)
    {
        Vector3 position = new Vector3(spawnPoint.position.x + Random.value * 3,
            spawnPoint.position.y, spawnPoint.position.z + Random.value * 3);
        NetworkObject newObject = Runner.Spawn(prefabRef, position, Quaternion.identity);
        Unit unit = newObject.GetComponent<Unit>();
        unit.Owner = playerRef;
    }


   
}