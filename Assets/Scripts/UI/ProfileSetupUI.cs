using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSetupUI : MonoBehaviour
{
    public InputField nicknameInput;
    public UnityEngine.UI.Button confirmButton;

    // Start is called before the first frame update
    private void Awake()
    {
        confirmButton.interactable = !string.IsNullOrEmpty(nicknameInput.text);

        nicknameInput.onValueChanged.AddListener(x =>
        {
            confirmButton.interactable = !string.IsNullOrEmpty(x);
        });

        nicknameInput.text = ClientInfo.Username;
    }

    private void OnEnable()
    {
        Debug.Log(ClientInfo.Username);
        nicknameInput.text = ClientInfo.Username;
    }

    public void UpdateUsername()
    {
        ClientInfo.Username = nicknameInput.text;
    }

    public void AssertProfileSetup()
    {
        if (string.IsNullOrEmpty(ClientInfo.Username))
            UIScreen.Focus(GetComponent<UIScreen>());
    }
}
