using UnityEngine;

public class OptionImage : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SetWidth(int x)
    {
        var rectTransform = transform as RectTransform;
        rectTransform.sizeDelta = new Vector2(x, rectTransform.sizeDelta.y);
    }
}