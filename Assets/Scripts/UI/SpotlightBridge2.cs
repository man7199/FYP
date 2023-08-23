using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBridge2 : MonoBehaviour
{
    private void OnEnable()
    {
        SelectUnit(15);
    }

    public void SelectUnit(int DictIndex)
    {
        if (SpotlightGroup.Search("Model Display", out SpotlightGroup spotlight))
        {
            spotlight.FocusIndex(DictIndex);
            Debug.Log(DictIndex);
        }
    }
}
