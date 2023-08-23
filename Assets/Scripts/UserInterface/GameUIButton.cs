using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIButton : MonoBehaviour
{
    [SerializeField] private Building building;
    [SerializeField] private Unit unit;
    [SerializeField] private UIProgress uiprogress;
    [SerializeField] private GameObject[] image = { null, null, null, null, null, null };
    private Sprite[] robotBase = { null, null };
    public Button[] buttons;
    public TMP_Text guiText;
    private BuildingUI buildingUI;

    // Start is called before the first frame update
    private void Start()
    {
        robotBase[0] = Resources.Load<Sprite>("Icons/RobotBuilding/turret");
        robotBase[1] = Resources.Load<Sprite>("Icons/RobotBuilding/cannon");
        uiprogress = GameObject.Find("Progress").GetComponent<UIProgress>();
        buildingUI = GameObject.Find("ProgressUI").GetComponent<BuildingUI>();
        image[0] = GameObject.Find("SkillButton");
        image[1] = GameObject.Find("SkillButton1");
        image[2] = GameObject.Find("SkillButton2");
        image[3] = GameObject.Find("SkillButton3");
        image[4] = GameObject.Find("SkillButton4");
        image[5] = GameObject.Find("SkillButton5");
        guiText = GameObject.Find("Error Message").GetComponent<TMP_Text>();
        for (var i = 0; i < 6; i++) image[i].SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (unit != null && unit.GetComponent<Hero>() != null)
        {
            image[3].GetComponent<Image>().fillAmount = unit.GetComponent<Hero>().GetCD4Percent();
            image[4].GetComponent<Image>().fillAmount = unit.GetComponent<Hero>().GetCD5Percent();
            image[5].GetComponent<Image>().fillAmount = unit.GetComponent<Hero>().GetCD6Percent();
        }
    }

    public void Button0()
    {
        if (building != null)
        {
            if (!building.fullprogress())
            {
                if (building.GetComponent<RobotBase>())
                {
                    building.GetComponent<RobotBase>().Skill1();
                }

                else if (buildingUI.Skill1())
                {
                    uiprogress.addQueue(image[0].GetComponent<Image>());
                }
                else
                {
                    //Debug.Log("not enough resource!");
                    //StartCoroutine(ShowMessage("not enough resource", 2));
                    FailSoundEffect();
                }
            }
            else
            {
                Debug.Log("progress full(maximum 5)");
                StartCoroutine(ShowMessage("progress full(maximum 5)", 2));
                FailSoundEffect();
            }
        }
        else if (unit != null)
        {
            unit.Skill1();
        }
    }

    public void Button1()
    {
        if (building != null)
        {
            if (!building.fullprogress())
            {
                if (building.GetComponent<RobotBase>())
                {
                    building.GetComponent<RobotBase>().Skill2();
                }
                else if (buildingUI.Skill2())
                {
                    uiprogress.addQueue(image[1].GetComponent<Image>());
                }
                else
                {
                    //Debug.Log("not enough resource!");
                    //StartCoroutine(ShowMessage("not enough resource", 2));
                    FailSoundEffect();
                }
            }
            else
            {
                Debug.Log("progress full(maximum 5)");
                StartCoroutine(ShowMessage("progress full(maximum 5)", 2));
                FailSoundEffect();
            }
        }
        else if (unit != null)
        {
            unit.Skill2();
        }
    }

    public void Button2()
    {
        if (building != null)
        {
            if (!building.fullprogress())
            {
                if (building.GetComponent<RobotBase>())
                {
                    building.GetComponent<RobotBase>().Skill3();
                }
                else if (buildingUI.Skill3())
                {
                    uiprogress.addQueue(image[2].GetComponent<Image>());
                }
                else
                {
                    //Debug.Log("not enough resource!");
                    //StartCoroutine(ShowMessage("not enough resource", 2));
                    FailSoundEffect();
                }
            }
            else
            {
                Debug.Log("progress full(maximum 5)");
                StartCoroutine(ShowMessage("progress full(maximum 5)", 2));
                FailSoundEffect();
            }
        }
        else if (unit != null)
        {
            unit.Skill3();
        }
    }

    public void Button3()
    {
        if (building != null)
        {
            if (!building.fullprogress())
            {
                if (building.GetComponent<RobotBase>())
                {
                    building.GetComponent<RobotBase>().Skill4();
                }
                else if (buildingUI.Skill4())
                {
                    uiprogress.addQueue(image[3].GetComponent<Image>());
                }
                else
                {
                    //Debug.Log("not enough resource!");
                    //StartCoroutine(ShowMessage("not enough resource", 2));
                    FailSoundEffect();
                }
            }
            else
            {
                Debug.Log("progress full(maximum 5)");
                StartCoroutine(ShowMessage("progress full(maximum 5)", 2));
                FailSoundEffect();
            }
        }
        else if (unit != null)
        {
            unit.Skill4();
        }
    }

    public void Button4()
    {
        if (building != null)
        {
            if (!building.fullprogress())
            {
                if (building.GetComponent<RobotBase>())
                {
                    building.GetComponent<RobotBase>().Skill5();
                }
                else if (buildingUI.Skill5())
                {
                    uiprogress.addQueue(image[4].GetComponent<Image>());
                }
                else
                {
                    //Debug.Log("not enough resource!");
                    //StartCoroutine(ShowMessage("not enough resource", 2));
                    FailSoundEffect();
                }
            }
            else
            {
                Debug.Log("progress full(maximum 5)");
                StartCoroutine(ShowMessage("progress full(maximum 5)", 2));
                FailSoundEffect();
            }
        }
        else if (unit != null)
        {
            unit.Skill5();
        }
    }

    public void Button5()
    {
        if (building != null)
        {
            if (!building.fullprogress())
            {
                if (building.GetComponent<RobotBase>())
                {
                    building.GetComponent<RobotBase>().Skill6();
                }
                else if (buildingUI.Skill6())
                {
                    uiprogress.addQueue(image[5].GetComponent<Image>());
                }
                else
                {
                    //Debug.Log("not enough resource!");
                    //StartCoroutine(ShowMessage("not enough resource", 2));
                    FailSoundEffect();
                }
            }
            else
            {
                Debug.Log("progress full(maximum 5)");
                StartCoroutine(ShowMessage("progress full(maximum 5)", 2));
                FailSoundEffect();
            }
        }

        else if (unit != null)
        {
            unit.Skill6();
        }
    }

    public bool RobotBaseProgress(int x)
    {
        switch (x)
        {
            case 0:
                if (buildingUI.Skill1())
                {
                    uiprogress.addQueue(image[0].GetComponent<Image>());
                }
                else {
                    return false;
                }

                break;
            case 1:
                if (buildingUI.Skill2())
                {
                    uiprogress.addQueue(image[1].GetComponent<Image>());
                }
                else
                {
                    return false;
                }

                break;
            case 2:
                if (buildingUI.Skill3())
                {
                    uiprogress.addQueue(image[2].GetComponent<Image>());
                }
                else
                {
                    return false;
                }

                break;
            case 3:
                if (buildingUI.Skill4())
                {
                    uiprogress.addQueue(image[3].GetComponent<Image>());
                }
                else
                {
                    return false;
                }

                break;
            case 4:
                if (buildingUI.Skill5())
                {
                    uiprogress.addQueue(image[4].GetComponent<Image>());
                }
                else
                {
                    return false;
                }

                break;
            case 5:
                if (buildingUI.Skill6())
                {
                    uiprogress.addQueue(image[5].GetComponent<Image>());
                }
                else
                {
                    return false;
                }
                break;
            case 6:
                if (buildingUI.Skill7())
                {
                    uiprogress.addQueue(robotBase[0]);
                }
                else
                {
                    return false;
                }
                break;
            case 7:
                if (buildingUI.Skill8())
                {
                    uiprogress.addQueue(robotBase[1]);
                }
                else
                {
                    return false;
                }
                break;
        }
        return true;
    }

    public void setbuilding(Building bui)
    {
        unit = null;
        building = bui;
        setImage(building.getSprites());
        updateDescri();
    }

    public void setbuilding()
    {
        if (building != null)
        {
            building = null;
            for (var i = 0; i < 6; i++)
            {
                image[i].SetActive(false);
            }
        }
    }

    public void setunit(Unit u)
    {
        building = null;
        unit = u;
        setImage(unit.SkillImage());
        unitupdateDescri();
    }

    public void setunit()
    {
        if (unit != null)
        {
            unit = null;
            for (var i = 0; i < 6; i++)
            {
                image[i].SetActive(false);
            }
        }
    }

    private IEnumerator ShowMessage(string message, float delay)
    {
        guiText.text = message;
        guiText.enabled = true;
        yield return new WaitForSeconds(delay);
        guiText.enabled = false;
    }

    public void setImage(Sprite[] sp)
    {
        for (var i = 0; i < 6; i++)
        {
            image[i].SetActive(true);
            image[i].GetComponent<Image>().sprite = sp[i];
        }
    }

    public void updateDescri()
    {
        for (var i = 0; i < 6; i++)
            if (building.getDescription(i) == null || building.getDescription(i).Length == 0)
            {
                image[i].SetActive(false);
            }
            else
            {
                image[i].SetActive(true);
                image[i].GetComponent<Transform>().GetChild(0).Find("Description").GetComponent<TMP_Text>()
                    .SetText(building.getDescription(i));
                image[i].GetComponent<Transform>().GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                image[i].GetComponent<Transform>().GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>()
                    .SetText(building.getrequireresource(i).ToString());
            }
    }

    public void unitupdateDescri()
    {
        for (var i = 0; i < 6; i++)
            if (unit.getDescription(i) == null || unit.getDescription(i).Length == 0)
            {
                image[i].SetActive(false);
            }
            else
            {
                image[i].SetActive(true);
                image[i].GetComponent<Transform>().GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>()
                    .SetText("");
                image[i].GetComponent<Transform>().GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
                image[i].GetComponent<Transform>().GetChild(0).Find("Description").GetComponent<TMP_Text>()
                    .SetText(unit.getDescription(i));
            }
    }

    private void FailSoundEffect()
    {
        AudioManager.Play("failUI", AudioManager.MixerTarget.UI);
    }
}