using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ForUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // to prevent clicking on UI count as clicking the space behind UI
    public PlayerControl pc;

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.Find("Game Manager").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    public void OnPointerEnter(PointerEventData eventData)
    {
        pc.HitUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pc.HitUI(false);
    }
}
