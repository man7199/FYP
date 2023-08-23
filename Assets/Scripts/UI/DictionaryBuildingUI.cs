using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryBuildingUI : MonoBehaviour
{
    public GameObject[] BuildingPrefab;

    public Text BuildingName;

    [Header("Stat")]
    public Text HP;
    public Text Width;
    public Text Height;
    public Text Cost;
    public Text BuildTime;
    public Text Type;

    [Header("Desciption")]
    public Text Info;

    public UnityEngine.UI.Button[] BuildingsIcon;

    public UnityEngine.UI.Button previous;
    public UnityEngine.UI.Button Next;

    public void Start()
    {
        for (int i = 0; i < BuildingsIcon.Length; i++)
        {
            int temp = i;
            BuildingsIcon[i].onClick.AddListener(() => updateContent(temp));
        }
    }

    public void OnEnable()
    {
        updateContent(0);
    }

    private void updateContent(int i)
    {
        //Debug.Log("test" + i);
        if (i < BuildingPrefab.Length)
        {
            //Update Name
            BuildingName.text = BuildingPrefab[i].GetComponent<Building>().getName().ToString();
            //Update Stats
            HP.text = BuildingPrefab[i].GetComponent<Building>().getHP().ToString();
            Width.text = BuildingPrefab[i].GetComponent<Building>().getWidth().ToString();
            Height.text = BuildingPrefab[i].GetComponent<Building>().getHeight().ToString();
            Cost.text = BuildingPrefab[i].GetComponent<Building>().getCost().ToString();
            BuildTime.text = BuildingPrefab[i].GetComponent<Building>().getBuildTime().ToString();
            Type.text = BuildingPrefab[i].GetComponent<Building>().getType().ToString();
            //Update Info
            Info.text = BuildingPrefab[i].GetComponent<Building>().getInfo();

        }
    }
}
