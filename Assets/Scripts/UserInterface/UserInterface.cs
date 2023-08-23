using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UserInterface : MonoBehaviour
{

    [SerializeField] protected TMP_Text hpText;
    [SerializeField] protected UIType uiType = UIType.none;
    [SerializeField] protected GameUIButton button;
    [SerializeField] protected Texture2D aim;
    [SerializeField] protected Texture2D aoe;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;
    private bool clicked = false;
    private PlayerControl playerControl;
    private bool cancel = false;
    protected Image icon;
    protected Image Buildingicon;

    public enum UIType
    {
        building,
        unit,
        none
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        icon = GameObject.Find("UnitIcon").GetComponent<Image>();
        Buildingicon = GameObject.Find("BuildingIcon").GetComponent<Image>();
        aim = Resources.Load<Texture2D>("Arts/Cursor/aim");
        aoe = Resources.Load<Texture2D>("Arts/Cursor/aoe");
        button = GameObject.Find("SkillButtons").GetComponent<GameUIButton>();
        playerControl = GameObject.Find("Game Manager").GetComponent<PlayerControl>();
        hpText = GameObject.Find("hpUI").GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (uiType != UIType.none)
        {
            hpText.SetText(hpUI());
        }

    }
    

    public virtual void changeUI(Unit u)
    {
        uiType = UIType.unit;
        hpText.GetComponentInParent<Image>().enabled = true;
        hpText.SetText("");
        icon.enabled = true;
        icon.sprite = u.icon;
    }

    public virtual void changeUI()
    {
        uiType = UIType.none;
        hpText.SetText("");
        hpText.GetComponentInParent<Image>().enabled = false;
        icon.enabled = false;
    }

    public virtual void changeUI(Building b)
    {
        uiType = UIType.building;
        hpText.GetComponentInParent<Image>().enabled = true;
        Buildingicon.enabled = true;
        Buildingicon.sprite = b.icon;
    }

    public virtual void otherchangeUI(Building b)
    {
        uiType = UIType.building;
        hpText.GetComponentInParent<Image>().enabled = true;
    }

    public virtual string hpUI()
    {
        return "";
    }
    public virtual void cursorAim()
    {
        Cursor.SetCursor(aim, hotSpot, cursorMode);
    }
    public virtual void cursorAoe()
    {
        Cursor.SetCursor(aoe, hotSpot, CursorMode.ForceSoftware);
    }
    public virtual void cursorDef()
    {
        Cursor.SetCursor(null, hotSpot, cursorMode);
    }
    public virtual bool checkClicked()
    {
        return clicked;
    }
    public virtual bool checkCancel()
    {
        return cancel;
    }

    public virtual void setClicked(bool x)
    {
        clicked = x;
    }
    public virtual void setClickedCancel()
    {
        cancel = true;
        clicked = true;
    }

    public virtual void resetCancel()
    {
        cancel = false;
        clicked = false;        
    }
    public virtual void Refresh(Unit u) {         
    }
    public virtual void Refresh(Building b)
    {
    }
}
