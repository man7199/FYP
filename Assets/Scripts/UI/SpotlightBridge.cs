using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBridge : MonoBehaviour
{
    private void OnEnable()
    {
        SelectUnit(0);
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
