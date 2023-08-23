using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreateGameUI : MonoBehaviour
{
    public InputField lobbyName;
    public UnityEngine.UI.Button createButton;
    public UnityEngine.UI.Button joinButton;

    // Start is called before the first frame update
    void Start()
    {
        lobbyName.onValueChanged.AddListener(x =>
        {
            ServerInfo.LobbyName = x;
            ClientInfo.LobbyName = x;
            createButton.interactable = !string.IsNullOrEmpty(x);
            joinButton.interactable = !string.IsNullOrEmpty(x);
        });

        NewRoomCode();
    }

    public void OnEnable()
    {
        NewRoomCode();
    }

    private void NewRoomCode()
    {
        lobbyName.text = ServerInfo.LobbyName = ClientInfo.LobbyName = "Session" + Random.Range(0, 1000);
    }

    // UI Hooks
    private bool _lobbyIsValid;

    public void ValidateLobby()
    {
        _lobbyIsValid = string.IsNullOrEmpty(ServerInfo.LobbyName) == false;
    }

    public void TryFocusScreen(UIScreen screen)
    {
        if (_lobbyIsValid)
        {
            UIScreen.Focus(screen);
        }
    }

    public void TryCreateLobby(GameLauncher launcher)
    {
        if (_lobbyIsValid)
        {
            launcher.JoinOrCreateLobby();
            _lobbyIsValid = false;
        }
    }
}
