using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FusionHelper.Utility;

public class ResourcesManager : MonoBehaviour
{
    public MissionDefinition[] missions;

    public static ResourcesManager Instance => Singleton<ResourcesManager>.Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
