using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class LobbyUI : MonoBehaviour, IDisabledUI
{
    public GameObject textPrefab;
    public Transform parent;
    public ToggleGroup mission;
    public Toggle mission01;
    public Toggle mission02;
    public UnityEngine.UI.Button readyUp;
    public UnityEngine.UI.Button startButton;
    public Text lobbyNameText;

    private static readonly Dictionary<RoomPlayer, LobbyItemUI> ListItems = new Dictionary<RoomPlayer, LobbyItemUI>();
    private static bool IsSubscribed;

    private void Awake()
    {
        mission01.onValueChanged.AddListener(x =>
        {
            var gm = GameManager.Instance;
            if (x) gm.MissionId = 0;
            Debug.Log($"MissionId is {gm.MissionId}, mission01 is {x}");
        });

        mission02.onValueChanged.AddListener(x =>
        {
            var gm = GameManager.Instance;
            if (x) gm.MissionId = 1;
            Debug.Log($"MissionId is {gm.MissionId}, mission02 is {x}");
        });

        GameManager.OnLobbyDetailsUpdated += UpdateDetails;

        RoomPlayer.PlayerChanged += (player) =>
        {
            var isLeader = RoomPlayer.Local.IsLeader;
            startButton.gameObject.SetActive(isLeader);
            mission01.interactable = isLeader;
            mission02.interactable = isLeader;
        };
    }

    void UpdateDetails(GameManager manager)
    {
        lobbyNameText.text = "Room Code: " + manager.LobbyName;
        if (ResourcesManager.Instance.missions[GameManager.Instance.MissionId].missionName == "01")
        {
            mission01.SetIsOnWithoutNotify(true);
            Debug.Log("Check, 01");
        }
        else
        {
            mission02.SetIsOnWithoutNotify(true);
            Debug.Log("Check, 02");
        }
    }

    public void Setup()
    {
        if (IsSubscribed) return;
        RoomPlayer.PlayerJoined += AddPlayer;
        RoomPlayer.PlayerLeft += RemovePlayer;

        RoomPlayer.PlayerChanged += EnsureAllPlayersReady;

        readyUp.onClick.AddListener(ReadyUpListener);

        IsSubscribed = true;
    }

    private void OnDestroy()
    {
        if (!IsSubscribed) return;

        RoomPlayer.PlayerJoined -= AddPlayer;
        RoomPlayer.PlayerLeft -= RemovePlayer;

        readyUp.onClick.RemoveListener(ReadyUpListener);

        IsSubscribed = false;
    }

    private void AddPlayer(RoomPlayer player)
    {
        if (ListItems.ContainsKey(player))
        {
            var toRemove = ListItems[player];
            Destroy(toRemove.gameObject);

            ListItems.Remove(player);
        }

        var obj = Instantiate(textPrefab, parent).GetComponent<LobbyItemUI>();
        obj.SetPlayer(player);

        ListItems.Add(player, obj);

        UpdateDetails(GameManager.Instance);
    }

    private void RemovePlayer(RoomPlayer player)
    {
        if (!ListItems.ContainsKey(player))
            return;

        var obj = ListItems[player];
        if (obj != null)
        {
            Destroy(obj.gameObject);
            ListItems.Remove(player);
        }
    }

    public void OnDestruction() { }

    private void ReadyUpListener()
    {
        var local = RoomPlayer.Local;
        if (local && local.Object && local.Object.IsValid)
        {
            local.RPC_ChangeReadyState(!local.IsReady);
        }
    }

    private void EnsureAllPlayersReady(RoomPlayer lobbyPlayer)
    {
        if (!RoomPlayer.Local.IsLeader)
            return;

        if (IsAllReady())
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    private static bool IsAllReady() => RoomPlayer.Players.Count > 0 && RoomPlayer.Players.TrueForAll(player => player.IsReady);

    public static void StartMission()
    {
        int scene = ResourcesManager.Instance.missions[GameManager.Instance.MissionId].buildIndex;
        LevelManager.LoadMission(scene);
    }
}
