using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatusUI : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private int current=0;
    [SerializeField] private Image[] dowait = { null, null, null, null, null };
    [SerializeField] private TMP_Text[] descrption= { null, null, null, null, null };
    private GameObject UI;
    [SerializeField] private Buff[] _buff = { null, null, null, null, null };
    // Start is called before the first frame update
    private void Start()
    {
       
        dowait[0] = GameObject.Find("Status1").GetComponent<Image>();
        dowait[1] = GameObject.Find("Status2").GetComponent<Image>();
        dowait[2] = GameObject.Find("Status3").GetComponent<Image>();
        dowait[3] = GameObject.Find("Status4").GetComponent<Image>();
        dowait[4] = GameObject.Find("Status5").GetComponent<Image>();
        for (var i = 0; i < 5; i++)
        {
            descrption[i] = dowait[i].GetComponent<Transform>().GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            descrption[i].SetText("");
        }
        GetComponent<TMP_Text>().SetText("");
        for (var i = 0; i < 5; i++) dowait[i].enabled = false;
    }
    
    // Update is called once per frame
    private void Update()
    {
        for (var i = 0; i < current; i++)
        {
            descrption[i].SetText(_buff[i].description + "\n" + "Remaining: " + _buff[i].getDuration().ToString("F2")+"s");
        }
        
    }

    public void addQueue(Buff buff)
    {
        dowait[current].sprite = buff.image;
        dowait[current].enabled = true;
        _buff[current] = buff;
        current++;
    }
  
    public void deQueue(Buff buff)
    {
        var x = 0;
        for (var i = 0; i < current; i++) {
            if (_buff[i] == buff) x = i;        }
        for (var i = x; i < current && i < 4; i++) { 
            dowait[i].sprite = dowait[i + 1].sprite;
            dowait[i].enabled = dowait[i + 1].enabled;
            _buff[i] = _buff[i + 1];
        }

        dowait[current].sprite =null;
        dowait[current].enabled = false;
        _buff[current] = buff;
        current--;
        
    }

    public void setUnit(Unit u)
    {
        unit = u;
        GetComponent<TMP_Text>().SetText("Status:");
        current = 0;
        for (var i = current; i < 5; i++)
        {
            _buff[i] = null;
            dowait[i].enabled = false;
        }
        if (u.GetComponent<BuffableEntity>() != null)
        {
            u.GetComponent<BuffableEntity>().ShowBuff();
        }
    }
    public void setUnit()
    {
        unit = null;        
        GetComponent<TMP_Text>().SetText("");
        for (var i = 0; i < 5; i++) dowait[i].enabled = false;
    }
    
}