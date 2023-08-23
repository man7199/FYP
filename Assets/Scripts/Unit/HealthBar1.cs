using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class HealthBar1 : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image healthFill;

    [SerializeField] private Transform canvasTransform;

    // Start is called before the first frame update
    void Start()
    {
        //slider.value = 1;
        //UpdateHealthBar(1f);
    }

    public void UpdateHealthBar(float healthProportion) // input should be between 0 to 1
    {
        if (slider != null)
        {
            slider.value = healthProportion;

            healthFill.color = gradient.Evaluate(healthProportion);
            ////change the color of the healthbar to green, yellow and red at different proportions, 100%, 50%, 23%
        }
        else
        {
            Debug.Log(this.gameObject);
        }
    }

    public void UpdateHealthBarWithoutSlider(float healthProportion)
    {
        healthFill.fillAmount = healthProportion;
        healthFill.color = gradient.Evaluate(healthProportion);
    }

    private void LateUpdate()
    {
        if (canvasTransform != null)
        {
            canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
            //the healthbar face at the same angle as camera, parallel to camaera

            //canvasTransform.LookAt(Camera.main.transform); //the healthbar always face at the camera as a sunflower
        }
    }
}
