using UnityEngine;


public abstract class Hero : Robot
{
    protected int maxMp;

    protected int currentMp;

    //cool down
    protected float timer4;
    protected float timer5;
    protected float timer6;
    protected float CD4;
    protected float CD5;
    protected float CD6;

    protected override void Update()
    {
        base.Update();
        if (timer4 > 0)
            timer4 -= Time.deltaTime;
        if (timer5 > 0)
            timer5 -= Time.deltaTime;
        if (timer6 > 0)
            timer6 -= Time.deltaTime;
    }

    protected void StartCD4()
    {
        timer4 = CD4;
    }

    protected void StartCD5()
    {
        timer5 = CD5;
    }

    protected void StartCD6()
    {
        timer6 = CD6;
    }
    public float GetCD4Percent() {
        if (timer4 <= 0) {
            return 1;
        }
            return 1 - (timer4 / CD4);
    }
    public float GetCD5Percent()
    {
        if (timer5 <= 0)
        {
            return 1;
        }
        return 1-(timer5 / CD5);
    }
    public float GetCD6Percent()
    {
        if (timer6 <= 0)
        {
            return 1;
        }
        return 1 - (timer6 / CD6);
    }
}