using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using Utils;

public class PlayerControl : MonoBehaviour
{
    public static PlayerRef myPlayerRef;

    private readonly float dragThreshold = 10f;
    private bool isDrag;

    private Vector3 mouseStartPoint, mouseEndPoint;
    private Vector3 patrolStartPoint, patrolEndPoint;
    private bool isCurrentPatrolStartPoint = true;
    [SerializeField] int xoffset;
    [SerializeField] int zoffset;
    [SerializeField] Camera cam;
    [SerializeField] Camera cam2;
    [SerializeField]
    LayerMask mask;
    RaycastHit hit;
    MeshCollider meshCollider;
    private UnitController unitController;


    private List<Unit> selectedUnits;

    private bool hitUI;
    private bool unitSkill;
    private UnitUI UI;
    private BuildingGhost bg;
    private BuildingUI BuildUI;

    public void UnitSkill(bool x)
    {
        unitSkill = x;
    }

    public void HitUI(bool x)
    {
        hitUI = x;
    }

    private void Awake()
    {
        unitController = GetComponent<UnitController>();
        UI = GameObject.Find("UnitUI").GetComponent<UnitUI>();
        bg = GetComponentInChildren<BuildingGhost>();
        BuildUI = GameObject.Find("ProgressUI").GetComponent<BuildingUI>();
        cam = GameObject.Find("Minimap Camera (1)").GetComponent<Camera>();
        cam2 = GameObject.Find("Minimap Camera").GetComponent<Camera>();
        cam2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!hitUI)
        {
            if (!UI.checkClicked())
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (bg.GetIsBuildingGhost())
                    {
                        var mousePosition = UtilsClass.GetMouseWorldPosition();
                        if (UI.selected() == null) // Use building to build
                        {
                            if (BuildUI.selected().GetComponent<RobotBase>().BuildingProgress())
                            {
                                BuildBuilding(mousePosition, null);
                            }
                        }
                        else // Use cell to build
                        {
                            if (BuildingController.Instance.CheckIsPositionValid(mousePosition,
                                    bg.GetCurrentBuilding(), UI.selected()))
                            {
                                if (Vector3.Distance(UI.selected().transform.position, mousePosition) < 10)
                                {
                                    UI.selected().Build(mousePosition, bg.GetCurrentBuilding());
                                }
                                else
                                {
                                    UnitController.Instance.RPCMoveCommand(
                                        UnitSelection.Instance.selectedUnits.ToArray(),
                                        mousePosition, true);
                                    UI.selected().Build(mousePosition, bg.GetCurrentBuilding());
                                }
                            }
                            else
                            {
                                UtilsClass.CreateWorldTextPopup("You cannot place here", mousePosition,
                                    color: Color.red);
                            }
                        }

                        
                        bg.StopBuildingGhost();
                        return;
                    }

                    UI.setClicked(true);
                }
            }
            else
            {
                // handle dragging
                if (Input.GetMouseButtonDown(0))
                {
                    mouseStartPoint = Input.mousePosition;
                    isDrag = false;
                }

                if (!isDrag && Input.GetMouseButton(0) &&
                    (mouseStartPoint - Input.mousePosition).magnitude > dragThreshold)
                    isDrag = true;

                if (Input.GetMouseButtonUp(0))
                {
                    mouseEndPoint = Input.mousePosition;
                    if (!isDrag) //when isDrag is false
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                            UnitSelection.Instance.MouseClickUnitSelection(mouseEndPoint, true);
                        else
                            UnitSelection.Instance.MouseClickUnitSelection(mouseEndPoint);
                    }
                    else //when isDrag is true
                    {
                        isDrag = false;
                        if (Input.GetKey(KeyCode.LeftShift))
                            UnitSelection.Instance.MouseDrag(mouseStartPoint, mouseEndPoint, true);
                        else
                            UnitSelection.Instance.MouseDrag(mouseStartPoint, mouseEndPoint);
                    }
                }

                if (Input.GetMouseButtonDown(0))
                {
                    BuildingController.Instance.SelectBuildingCommand(Input.mousePosition);
                }
            }

            //Going to the point once click a place, command units to move OR attack unit
            if (!Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
            {
                if (!UI.checkClicked())
                {
                    UI.setClickedCancel();
                }


                ClearPatrolState();
                // Check if is attack command
                Unit targetUnit = UnitSelection.Instance.GetUnitByWorldPosition(Input.mousePosition);
                if (targetUnit != null&&targetUnit.teamType==Unit.TeamType.Enemy)
                {
                    UnitController.Instance.RPCAttackCommand(UnitSelection.Instance.selectedUnits.ToArray(),
                        targetUnit);
                }
                else
                {
                    UnitController.Instance.RPCMoveCommand(UnitSelection.Instance.selectedUnits.ToArray(),
                        UtilsClass.GetMouseWorldPosition(), true);
                }
            }

            //Patrol command, Holding shift and right click
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(1))
            {
                if (isCurrentPatrolStartPoint)
                {
                    patrolStartPoint = UtilsClass.GetMouseWorldPosition();
                    isCurrentPatrolStartPoint = false;
                    Debug.Log("getting start position");
                }
                else
                {
                    patrolEndPoint = UtilsClass.GetMouseWorldPosition();
                    isCurrentPatrolStartPoint = true;
                    Debug.Log("getting end position");
                }

                if (patrolStartPoint != Vector3.zero && patrolEndPoint != Vector3.zero)
                {
                    if (isCurrentPatrolStartPoint)
                    {
                        unitController.PatrolCommand(patrolStartPoint, patrolEndPoint);
                    }
                    else
                    {
                        unitController.PatrolCommand(patrolEndPoint, patrolStartPoint);
                    }
                }
            }
           
            //Select building
        }
        else
        {
            //if (Input.GetMouseButtonDown(1))
            //{
            //    Ray ray;
            //    if (cam.isActiveAndEnabled)
            //    {                    
            //        ray = cam.ScreenPointToRay(Input.mousePosition);
            //        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            //        {
            //            Vector3 movePoint = new Vector3(hit.point.x + xoffset, 0, hit.point.z + zoffset);
            //            UnitController.Instance.RPCMoveCommand(UnitSelection.Instance.selectedUnits.ToArray(),
            //            movePoint, true);
            //        }
            //    }
            //    else {                  
            //        ray = cam2.ScreenPointToRay(Input.mousePosition);
            //        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            //        {
            //            Vector3 movePoint = new Vector3(hit.point.x + xoffset - 20, 0, hit.point.z);
            //            UnitController.Instance.RPCMoveCommand(UnitSelection.Instance.selectedUnits.ToArray(),
            //            movePoint, true);
            //        }
            //  }
               
            //}
        }
        //Constructing Building
        if (Input.GetKeyDown("c"))
        {
            var mousePosition = UtilsClass.GetMouseWorldPosition();
            if (mousePosition != Vector3.zero)
            {
                var result = (bool)BuildingController.Instance?.PlacingBuildingCommand(mousePosition, null);
                if (!result)
                    UtilsClass.CreateWorldTextPopup("You cannot place here", mousePosition, color: Color.red);
            }
        }

        //Destroying Building
        //if (Input.GetKeyDown("d"))
        //{
        //    var mousePosition = UtilsClass.GetMouseWorldPosition();
        //    if (mousePosition != Vector3.zero) BuildingController.Instance?.DestroyBuilding(mousePosition);
        //}

        //Rotation
        if (Input.GetKeyDown(KeyCode.R)) BuildingController.Instance?.Rotate();

        //Switch Building Type
        if (Input.GetKeyDown(KeyCode.E)) BuildingController.Instance?.SwitchBuilding();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.setClickedCancel();
            if (bg.GetIsBuildingGhost())
                bg.StopBuildingGhost();
        }


        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                int numberPressed = i + 1;
                // Select Unit in team
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelection.Instance.SelectUnitsInTeam(numberPressed);
                }
                // Assign Units to team
                else
                {
                    UnitSelection.Instance.AssignUnitsToTeam(numberPressed);
                }
            }
        }
    }

    private KeyCode[] keyCodes =
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
    };

    private void ClearPatrolState()
    {
        patrolEndPoint = Vector3.zero;
        patrolStartPoint = Vector3.zero;
        isCurrentPatrolStartPoint = true;
    }

    private void OnGUI()
    {
        if (isDrag)
        {
            var rect = UtilsClass.GetScreenRect(mouseStartPoint, Input.mousePosition);
            UtilsClass.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            UtilsClass.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    public void Move()
    {
        ClearPatrolState();
        UnitController.Instance.RPCMoveCommand(UnitSelection.Instance.selectedUnits.ToArray(),
            UtilsClass.GetMouseWorldPosition(), true);
    }
    public void Move(Vector3 pos)
    {
        ClearPatrolState();
        UnitController.Instance.RPCMoveCommand(UnitSelection.Instance.selectedUnits.ToArray(),
            pos, true);
    }

    public void Patrol()
    {
        if (isCurrentPatrolStartPoint)
        {
            patrolStartPoint = UtilsClass.GetMouseWorldPosition();
            isCurrentPatrolStartPoint = false;
            Debug.Log("getting start position");
        }
        else
        {
            patrolEndPoint = UtilsClass.GetMouseWorldPosition();
            isCurrentPatrolStartPoint = true;
            Debug.Log("getting end position");
        }

        if (patrolStartPoint != Vector3.zero && patrolEndPoint != Vector3.zero)
        {
            if (isCurrentPatrolStartPoint)
            {
                unitController.PatrolCommand(patrolStartPoint, patrolEndPoint);
            }
            else
            {
                unitController.PatrolCommand(patrolEndPoint, patrolStartPoint);
            }
        }
    }

    public bool BuildBuilding(Vector3 pos, Building bui)
    {
        var result = (bool)BuildingController.Instance?.PlacingBuildingCommand(pos, bui);
        if (!result)
            UtilsClass.CreateWorldTextPopup("You cannot place here", pos, color: Color.red);
        return result;
    }

    //remove the dead unit from the selectedUnits list
    // public void UpdateSelectedUnits(string input)
    // {
    //     Debug.Log("Being called");
    //     for (int i = 0; i < selectedUnits.Count; i++)
    //     {
    //         if (selectedUnits[i].name == input)
    //         {
    //             Debug.Log("removed ");
    //             selectedUnits.RemoveAt(i);
    //         }
    //     }
    // }
}