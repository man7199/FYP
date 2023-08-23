using Fusion;
using UnityEngine;

public class BoneMarrow1 : Building
{
    [Networked] public NetworkPrefabRef whiteBloodCell { get; set; }
    public Building upgradeVersion;
    int[] requireresource1 = { 50, 100, 10, 10, 10, 0 };
    int[] requiretime1 = { 5, 0, 1, 1, 1, 3 };
    public Transform spawnPoint;
    public Transform buildingPosition;

    private GameLevel gameLevel;

    // Start is called before the first frame update
    void Start()
    {
        gameLevel = GameLevel.Instance;
        hp = 10;
        currenthp = hp;
        defensive = 1.0;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);
        description[0] = "Produce a white blood cell";
        sprites[0] = Resources.Load<Sprite>("Icons/Cell/whitebloodcell");
        Unlock();
        name = "Bone Marrow I";
        icon = Resources.Load<Sprite>("Arts/UI/Building/bonemarrow1");
    }

        private string Info
 = "This is bone marrow I, which can produce white blood cell and upgrade to bone marrow II.";
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
        if (gameLevel.isMarrow2Unlock)
        {
            
            if (GameObject.Find("ProgressUI").GetComponent<BuildingUI>().selected() == this)
                GameObject.Find("ProgressUI").GetComponent<BuildingUI>().changeUI();

            BuildingController buildingController = BuildingController.Instance;
            buildingController.CalculateTransform(buildingPosition.position, out Vector3 instantiatePosition,
                upgradeVersion);
            Vector2Int i = buildingController.tileGrid.GetGridPosition(buildingPosition.position);
            buildingController.RPCReplaceBuildingCommand(buildingPosition.position, upgradeVersion.prefabRef,
                buildingController.tileGrid.GetWorldPosition(i.x, i.y));
        }
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

    public void Unlock()
    {
        if (gameLevel.isMarrow2Unlock)
        {
            description[1] = "Upgrade";
            sprites[1] = Resources.Load<Sprite>("Arts/UI/Building/bonemarrow2");
            requireresource[1] = 100;
            requiretime[1] = 0;
        }
        else
        {
            description[1] = "Unlock in mother base to upgrade";
            sprites[1] = Resources.Load<Sprite>("Arts/UI/Building/cannotbonemarrow2");
            requireresource[1] = 0;
            requiretime[1] = 0;
        }
    }
}