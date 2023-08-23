using Fusion;
using UnityEngine;
using TMPro;
public class RobotBase : Building
{
    
    private int[] requireresource1 = { 100, 250, 150, 500, 500, 0 };
    int[] requiretime1 = { 5, 5, 5, 5, 5, 0 };
    public RobotBaseBuildingUnit self;
    [SerializeField] private BuildingUI ui;
    private bool page1 = true;
    private int whichBuilding = 0;
    private GameUIButton button;
    public static int solarPanelCount = 0;
    private int solarPanelMax = 5;
    public GameObject popupText;
    // Start is called before the first frame update
    protected void Start()
    {
        self = transform.GetChild(transform.childCount-1).GetChild(0).GetComponent<RobotBaseBuildingUnit>();
        hp = 1000;
        currenthp = hp;
        defensive = 1.0;
        setrequireresource(requireresource1);
        setrequiretime(requiretime1);        
        icon = Resources.Load<Sprite>("Icons/RobotBuilding/robotbase");        
        name = "Command Center";
        ui = GameObject.Find("ProgressUI").GetComponent<BuildingUI>();
        button = GameObject.Find("SkillButtons").GetComponent<GameUIButton>();
        popupText = Resources.Load<GameObject>("Prefab/PopUpText");        
        Page1();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Effect1()
    {
    }   

    public override void Effect2()
    {
    }
    public bool BuildingProgress() {
      return  button.RobotBaseProgress(whichBuilding);
    }
    public void Skill1()
    {
        whichBuilding = 0;
        if (page1)
        {
            self.Skill1();
        }
        else
        {
            whichBuilding = 6;
            self.Skill7();
                 }
    }
    public void Skill2()
    {
        whichBuilding = 1;
        if (page1)
        {
            self.Skill2();
        }
        else
        {
            whichBuilding = 7;
            self.Skill8();
        }
    }
    public void Skill3()
    {
        whichBuilding =2;
        if (page1)
        {
            if (solarPanelCount >= solarPanelMax)
                Instantiate(popupText, GameObject.Find("Canvas").transform).GetComponent<TMP_Text>().SetText("Cannot build more than 5");
            else
                self.Skill3();
        }
        else
        {
            Page1();
        }
    }
    public void Skill4()
    { 
        whichBuilding = 3;
        if (page1)
        {
            self.Skill4();
           
        }
    }
    public void Skill5()
    {
        
        whichBuilding = 4;
        if (page1)
        {
            self.Skill5();
            
        }
    }
    public void Skill6()
    {
        if (page1)
        {
            Page2();
        }
       
    }
    private void Page1() {
        sprites[0] = Resources.Load<Sprite>("Icons/RobotBuilding/robotfactory");
        sprites[1] = Resources.Load<Sprite>("Icons/RobotBuilding/herofactory");
        sprites[2] = Resources.Load<Sprite>("Icons/RobotBuilding/solarpanel");
        sprites[3] = Resources.Load<Sprite>("Icons/RobotBuilding/repairfactory");
        sprites[4] = Resources.Load<Sprite>("Icons/RobotBuilding/healthcenter");
        sprites[5] = Resources.Load<Sprite>("Icons/RobotBuilding/next");
        description[0] = "Build a robot factory to produce robots";
        description[1] = "Build a hero factory to produce heros";
        description[2] = "Build a transport panel to generate resources";
        description[3] = "Build a repair factory to repair robots";
        description[4] = "Build a health center to increase health center";
        description[5] = "Next Page";
        requireresource[2] = 50;
        page1 = true;
        ui.Refresh(this);
    }
    private void Page2() {
        sprites[0] = Resources.Load<Sprite>("Icons/RobotBuilding/turret");
        sprites[1] = Resources.Load<Sprite>("Icons/RobotBuilding/cannon");
        sprites[2] = Resources.Load<Sprite>("Icons/RobotBuilding/previous");
        description[0] = "Build a turrets to attack enemy";
        description[1] = "Build a cannon to attack enemy with area damage";
        description[2] = "Previous page";
        description[3] = null;
        description[4] = null;
        description[5] = null;
        requireresource[2] = 0;
        page1 = false;
        ui.Refresh(this);
    }

            private string Info
 ="This is Robot Base, which you can build factories which are used to produce robots, and defensive buildings including turrets, cannons. ";

    public override string getInfo()
    {
        return Info;
    }

    public float BuildingDelay() {
        float x = 0;
        for (var i = 0; i < currentTask - 1; i++) {
            x += currentTime[i];
        }
        return x;
    }
    public override void setUI(UIProgress x)
    {
        uiprogress = x;
        for (var i = 0; i < currentTask; i++) {
            if (whichfunc[i] == 6)
            {
                uiprogress.addQueue(Resources.Load<Sprite>("Icons/RobotBuilding/turret"));
            }
            else if (whichfunc[i] == 7)
                uiprogress.addQueue(Resources.Load<Sprite>("Icons/RobotBuilding/cannon"));
            else if (whichfunc[i] == 0)
                uiprogress.addQueue(Resources.Load<Sprite>("Icons/RobotBuilding/robotfactory"));
            else if (whichfunc[i] == 1)
                uiprogress.addQueue(Resources.Load<Sprite>("Icons/RobotBuilding/herofactory"));
            else if (whichfunc[i] == 2)
                uiprogress.addQueue(Resources.Load<Sprite>("Icons/RobotBuilding/solarpanel"));
            else
            uiprogress.addQueue(sprites[whichfunc[i]]);
                
                }
    }
}