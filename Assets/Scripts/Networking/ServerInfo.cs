using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerInfo
{
    public const int UserCapacity = 2;

    public static string LobbyName;
    public static string MissionName => ResourcesManager.Instance.missions[MissionId].missionName;

    public static int GameMode
    {
        get => PlayerPrefs.GetInt("S_GameMode", 0);
        set => PlayerPrefs.SetInt("S_GameMode", value);
    }

    public static int MissionId
    {
        get => PlayerPrefs.GetInt("S_MissionId", 0);
        set => PlayerPrefs.SetInt("S_MissionId", value);
    }
}
