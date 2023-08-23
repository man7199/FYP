using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{

    [FormerlySerializedAs("cost")] [SerializeField]
    public byte pathCost = 1;

    [SerializeField] public bool buildable;
    [SerializeField] public bool isObstacle;

    private void Awake()
    {
        if (isObstacle)
        {
            pathCost = 255;
        }
    }
}