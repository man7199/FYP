using Fusion;
using UnityEngine;

public class BoneMarrow2 : Building
{
    [Networked] public NetworkPrefabRef whiteBloodCell { get; set; }
    [Networked] public NetworkPrefabRef Macrophage { get; set; }
    public Transform spawnPoint;
    int[] requireresource1 = { 50, 200, 10, 10, 10, 0 };
    int[] requiretime1 = { 5, 5, 1, 1, 1, 3 };

    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        currenthp = hp;
        defensive = 1.0;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        description[0] = "Produce a white blood cell";
        sprites[0] = Resources.Load<Sprite>("Icons/Cell/whitebloodcell");
        
        description[1] = "Produce a macrophage";
        sprites[1] = Resources.Load<Sprite>("Icons/Cell/macrophage");
        icon =  Resources.Load<Sprite>("Arts/UI/Building/bonemarrow2");
        name = "Bone Marrow II";

    }

    private string Info 
        = "This is bone marrow II, which can produce white blood cell and macrophage.";
    public override string getInfo()
    {
        return Info;
    }


    public override void Effect1()
    {
        RpcSpawnUnit(whiteBloodCell, Runner.LocalPlayer);
    }
    public override void Effect2()
    {
        RpcSpawnUnit(Macrophage, Runner.LocalPlayer);
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