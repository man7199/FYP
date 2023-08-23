using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class EndingUI : MonoBehaviour
{
    public UnityEngine.UI.Button continueEndButton;

    public void Awake()
    {
        continueEndButton.onClick.AddListener(() => LevelManager.LoadMenu());
    }
}