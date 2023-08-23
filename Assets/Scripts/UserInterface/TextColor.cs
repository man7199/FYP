using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class TextColor : MonoBehaviour
{
    [FormerlySerializedAs("NormalColor")] public Color normalColor = Color.black;
    [FormerlySerializedAs("HoverColor")] public Color hoverColor = Color.black;
    [FormerlySerializedAs("ClickColor")] public Color clickColor = Color.black;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Point()
    {
        GetComponent<TextMeshProUGUI>().color = hoverColor;
    }

    public void Leave()
    {
        GetComponent<TextMeshProUGUI>().color = normalColor;
    }

    public void Click()
    {
        GetComponent<TextMeshProUGUI>().color = clickColor;
    }
}