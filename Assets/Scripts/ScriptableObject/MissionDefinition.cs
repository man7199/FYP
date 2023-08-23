using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mission", menuName = "ScriptableObjects/Mission Definition")]
public class MissionDefinition : ScriptableObject
{
    public string missionName;
    public int buildIndex;
}
