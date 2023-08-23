using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProgress : MonoBehaviour
{
    public Building building;
    public int testing = 1000;
    private int current;
    public Image[] dowait = { null, null, null, null, null };

    private Sprite nothing;
    private Image self;
    private GameObject UI;

    private TMP_Text unitName;

    // Start is called before the first frame update
    private void Start()
    {
        self = GetComponent<Image>();
        transform.parent.GetComponent<Image>().enabled = false;
        transform.parent.parent.GetComponent<Image>().enabled = false;
        dowait[0] = GameObject.Find("ProgressDoing").GetComponent<Image>();
        dowait[1] = GameObject.Find("ProgressWaiting").GetComponent<Image>();
        dowait[2] = GameObject.Find("ProgressWaiting1").GetComponent<Image>();
        dowait[3] = GameObject.Find("ProgressWaiting2").GetComponent<Image>();
        dowait[4] = GameObject.Find("ProgressWaiting3").GetComponent<Image>();
        nothing = dowait[0].sprite;
        unitName = GameObject.Find("UnitName").GetComponent<TMP_Text>();
        unitName.SetText("");
        for (var i = 0; i < 5; i++) dowait[i].enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (building == null)
        {
        }
        else
        {
            self.fillAmount = building.GetComponent<Building>().progressUI();
        }
    }

    public void addQueue(Image x)
    {
        dowait[current].sprite = x.sprite;
        current++;
    }

    public void addQueue(Sprite x)
    {
        dowait[current].sprite = x;
        current++;
    }

    public void deQueue(Building choosen)
    {
        if (choosen == building)
        {
            for (var i = 0; i < current && i < 4; i++) dowait[i].sprite = dowait[i + 1].sprite;
            if (current == 5) dowait[current - 1].sprite = nothing;

            current--;
        }
    }

    public void setbuilding(Building bui)
    {
        building = bui;
        change();
    }
    public void setbuilding()
    {
        building = null;
        transform.parent.GetComponent<Image>().enabled = false;
        transform.parent.parent.GetComponent<Image>().enabled = false;
        self.enabled = false;
        unitName.SetText("");
        for (var i = 0; i < 5; i++) dowait[i].enabled = false;
    }
    public void change()
    {
        transform.parent.GetComponent<Image>().enabled = true;
        transform.parent.parent.GetComponent<Image>().enabled = true;
        self.enabled = true;
        unitName.SetText(building.name);
        current = 0;
        for (var i = current; i < 5; i++)
        {
            dowait[i].enabled = true;
            dowait[i].sprite = nothing;
        }

    }
    public void ChangeBuildingName(Building b) {
        transform.parent.parent.GetComponent<Image>().enabled = true;
        unitName.SetText(b.name);
    }

}