using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public EndingUI endingScreen;
    public Text victory;
    public Text defeat;

    public void OpenPauseMenu()
    {
        InterfaceManager.Instance.OpenPauseMenu();
    }

    public void ShowVictory()
    {
        endingScreen.gameObject.SetActive(true);
        victory.gameObject.SetActive(true);
    }

    public void ShowDefeat()
    {
        endingScreen.gameObject.SetActive(true);
        defeat.gameObject.SetActive(true);
    }
}
