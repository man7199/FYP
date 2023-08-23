using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAudio : MonoBehaviour, ISelectHandler, IPointerEnterHandler/*, IPointerClickHandler*/
{
    private UnityEngine.UI.Button btn;
    private Toggle toggle;
    private void Awake()
    {
        if (TryGetComponent(out btn))
        {
            btn.onClick.AddListener(() => AudioManager.Play("clickUI", AudioManager.MixerTarget.UI));
        }

        if (TryGetComponent(out toggle))
        {
            toggle.onValueChanged.AddListener(x => AudioManager.Play("clickUI", AudioManager.MixerTarget.UI));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!btn || btn.interactable)
            AudioManager.Play("hoverUI", AudioManager.MixerTarget.UI);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData is PointerEventData) return;

        if (!btn || btn.interactable)
            AudioManager.Play("hoverUI", AudioManager.MixerTarget.UI);
    }
}
