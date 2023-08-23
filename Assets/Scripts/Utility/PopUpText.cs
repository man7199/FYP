using UnityEngine;

public class PopUpText : MonoBehaviour
{
    private float currentTime;
    private bool startTimer;
    private float time;

    // Update is called once per frame
    private void Update()
    {
        if (startTimer)
        {
            currentTime += Time.deltaTime;
            if (currentTime > time) Destroy(gameObject);
        }
    }

    public void SetUpTimer(float time)
    {
        this.time = time;
        startTimer = true;
    }
}