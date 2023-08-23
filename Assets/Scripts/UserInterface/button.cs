using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public UnityEngine.UI.Button butt;
    public Color wantedColor;
    private ColorBlock cb;

    private Color originalColor;

    // Start is called before the first frame update
    private void Start()
    {
        cb = butt.colors;
        originalColor = cb.selectedColor;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Hover()
    {
        cb.selectedColor = wantedColor;
        butt.colors = cb;
    }

    public void Leave()
    {
        cb.selectedColor = originalColor;
        butt.colors = cb;
    }
}