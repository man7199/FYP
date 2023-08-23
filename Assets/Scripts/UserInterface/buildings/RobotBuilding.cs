using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBuilding :Building
{
    protected int buildingTime=5;
    private RobotBase robotBase;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        foreach (var rend in GetComponentsInChildren<Renderer>(true))
        {
            ChangeAlpha(rend.GetComponent<Renderer>().materials, 0.3f);
        }
        robotBase = GameObject.Find("Command Center").GetComponent<RobotBase>();
        Invoke("ActivateGameObject", buildingTime+robotBase.BuildingDelay());
        this.enabled = false;
    }
    protected virtual void ActivateGameObject()
    {
        this.enabled = true;
        if (GameObject.Find("ProgressUI").GetComponent<BuildingUI>().selected() == this)
            Invoke("RefreshUI", 0.05f);


        foreach (var rend in GetComponentsInChildren<Renderer>(true))
        {
           ChangeAlpha(rend.GetComponent<Renderer>().materials, 1f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void ChangeAlpha(Material[] mat, float alphaValue) {

        for (int i = 0; i < mat.Length; i++)
        {
            Color oldColor = mat[i].color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
            mat[i].SetColor("_Color", newColor);
        }
    }
    private void RefreshUI() {
        GameObject.Find("ProgressUI").GetComponent<BuildingUI>().changeUI(this);
    }

}
