using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitUI : UserInterface
{
    [SerializeField] private Unit unit = null;
    private BuildingUI buildUI;
    private TMP_Text[] text = { null, null, null, null, null };
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 posUp;

    [SerializeField] private UnitStatusUI status;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hpText.SetText("");
        hpText.GetComponentInParent<Image>().enabled = false;
        text[0] = GameObject.Find("UnitAttr1").GetComponent<TMP_Text>();
        text[1] = GameObject.Find("UnitAttr2").GetComponent<TMP_Text>();
        text[2] = GameObject.Find("UnitAttr3").GetComponent<TMP_Text>();
        text[3] = GameObject.Find("UnitAttr4").GetComponent<TMP_Text>();
        text[4] = GameObject.Find("UnitAttr5").GetComponent<TMP_Text>();
        buildUI = GameObject.Find("ProgressUI").GetComponent<BuildingUI>();
        status = GameObject.Find("UnitAttr6").GetComponent<UnitStatusUI>();
        icon.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (unit != null)
        {
            text[1].SetText("Atk:" + unit.getATK().ToString());
            text[2].SetText("Atk Speed:" + unit.getATKSpeed().ToString());
            text[3].SetText("Defensive:" + unit.getDefensive().ToString());
            text[4].SetText("Atk Range:" + unit.getATKRange().ToString());
            if (unit.isAoe())
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                }


                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.gameObject != this.gameObject)
                    {
                        posUp = new Vector3(hit.point.x, 10f, hit.point.z);
                        position = hit.point;
                    }
                }

                if (unit.AOEfixed() != null)
                {
                    Quaternion transRot = Quaternion.LookRotation(unit.transform.position - position);
                    transRot.eulerAngles = new Vector3(0, transRot.eulerAngles.y, transRot.eulerAngles.z);
                    unit.AOEfixed().transform.rotation =
                        Quaternion.Lerp(transRot, unit.AOEfixed().transform.rotation, 0f);
                }

                if (unit.AOEcircle() != null)
                {
                    var hitPosDir = (hit.point - unit.transform.position).normalized;
                    float distance = Vector3.Distance(hit.point, unit.transform.position);
                    distance = Mathf.Min(distance, unit.AOEoutsidelimit());

                    var newHitPos = unit.transform.position + hitPosDir * distance;
                    Vector3 temp = new Vector3(newHitPos.x, unit.AOEcircle().transform.position.y, newHitPos.z);
                    unit.AOEcircle().transform.position = temp;
                }
            }
        }
    }

    public override void changeUI(Unit u)
    {
        if (!u.GetComponent<BuildingUnit>())
        {
            buildUI.changeUI();
            if (u.Owner == Player.Instance.MyPlayerRef())
            {
                button.setunit(u);
            }
            else
            {
                button.setunit();
            }

            base.changeUI(u);
            GetComponent<Image>().enabled = true;
            unit = u;
            text[0].SetText(unit.getName());
            text[1].SetText("Atk:" + unit.getATK().ToString());
            text[2].SetText("Atk Speed:" + unit.getATKSpeed().ToString());
            text[3].SetText("Defensive:" + unit.getDefensive().ToString());
            text[4].SetText("Evasion:");
            status.setUnit(u);
        }
    }

    public Unit selected()
    {
        return unit;
    }

    public override void changeUI()
    {
        base.changeUI();
        GetComponent<Image>().enabled = false;
        if (unit != null)
        {
            unit.ResetUI();
        }

        unit = null;
        button.setunit();
        text[0].SetText("");
        text[1].SetText("");
        text[2].SetText("");
        text[3].SetText("");
        text[4].SetText("");
        status.setUnit();
    }

    public override string hpUI()
    {
        return "hp:" + unit.getHP().ToString() + "/" + unit.maxHP.ToString();
    }

    public override void Refresh(Unit u)
    {
        if (unit == u)
        {
            changeUI();
            changeUI(u);
        }
    }
}