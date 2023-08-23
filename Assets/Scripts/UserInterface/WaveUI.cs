using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    private int wave=1;
    private int time;
    private int minute;
    private int second;
    private TMP_Text text;
    private LevelOne levelOne;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(transform.childCount - 1).GetComponent<TMP_Text>();
        levelOne = GameObject.Find("Network Game Manager").GetComponent<LevelOne>();
        if (levelOne!=null)
        {
            int previous = 0;
            if (wave != 1)
            {
                previous = (int)levelOne.waves[wave - 2].waveInterval;
            }
        time = (int)levelOne.waves[wave - 1].waveInterval -previous;
        StartCoroutine(Count());
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator Count() {
        yield return new WaitForSeconds(1);
        time--;
        if (time < 0)
        {
            wave++;
            time = (int)levelOne.waves[wave-1].waveInterval;
        }
        if (wave <= levelOne.waves.Length)
        {
            
            text.SetText("Wave " + wave  + " : " + time.ToString()+"s");
            StartCoroutine(Count());
        }
    }
}
