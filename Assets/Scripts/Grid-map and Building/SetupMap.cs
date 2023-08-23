using Fusion;
using UnityEngine;

public class SetupMap : MonoBehaviour
{
    private static SetupMap _instance;

    [SerializeField] private Building[] buildings;
    [SerializeField] private bool map, pathfinding;
    [SerializeField] public int nodePerCell;
    [SerializeField] private int mapWidth, mapHeight;
    [SerializeField] private float cellSzie;
    [SerializeField] private Vector3 originPosition;

    private PathFindingSystem pathFinding;
    private TileSystem tileSystem;

    public static SetupMap Instance
    {
        get
        {
            // TODO: Automatic creation
            if (_instance == null) _instance = new GameObject("a", typeof(SetupMap)).GetComponent<SetupMap>();

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
            return;
        }

        _instance = this;

        Setup();
    }

    public void Setup()
    {
        // setup pathfinding system
        if (pathfinding)
        {
            pathFinding = new PathFindingSystem(mapWidth * nodePerCell, mapHeight * nodePerCell, cellSzie / nodePerCell,
                originPosition);
            pathFinding.GetPathGrid(Vector3.zero, out PathGridNormal pathGrid, out PathGridPasswall pathGridPasswall, true);
        }

        // setup map and building system
        if (map) tileSystem = new TileSystem(mapWidth, mapHeight, cellSzie, originPosition);
    }
}