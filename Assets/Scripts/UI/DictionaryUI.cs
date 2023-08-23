using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictionaryUI : MonoBehaviour
{
    public GameObject[] UnitPrefab;

    public Text UnitName;

    [Header("Stat")]
    public Text HP;
    public Text Attack;
    public Text Defense;
    public Text Movement;
    public Text AtkSpeed;
    public Text Range;

    [Header("Desciption")]
    public Text Info;

    public UnityEngine.UI.Button[] UnitsIcon;

    public UnityEngine.UI.Button previous;
    public UnityEngine.UI.Button Next;

    public void Start()
    {
        for (int i = 0; i < UnitsIcon.Length; i++)
        {
            int temp = i;
            UnitsIcon[i].onClick.AddListener(() => updateContent(temp));
        }
    }

    public void OnEnable()
    {
        updateContent(0);
    }

    private void updateContent(int i)
    {
        //Debug.Log("test" + i);
        if (i < UnitPrefab.Length)
        {
            //Update Name
            UnitName.text = UnitPrefab[i].GetComponent<Unit>().getName().ToString();
            //Update Stats
            HP.text = UnitPrefab[i].GetComponent<Unit>().getInitialHP().ToString();
            Attack.text = UnitPrefab[i].GetComponent<Unit>().getATK_Dict().ToString();
            Defense.text = UnitPrefab[i].GetComponent<Unit>().getDef_Dict().ToString();
            Movement.text = UnitPrefab[i].GetComponent<Unit>().getMovement_Dict().ToString();
            AtkSpeed.text = UnitPrefab[i].GetComponent<Unit>().getATKSpeed().ToString();
            Range.text = UnitPrefab[i].GetComponent<Unit>().getATKRange().ToString();
            //Update Info
            Info.text = UnitPrefab[i].GetComponent<Unit>().getInfo();

        }
    }
}
