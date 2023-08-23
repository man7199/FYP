using UnityEngine;

public class testMoveStop : Robot
{
    public float launchVelocity = 750f;
    public float launchSpeed = 15f;
    protected float rotationSpeed = 5;

    protected float timer=0;
    protected bool aiming = false;


    protected override Node SetupBehaviorTree()
    {
        return null;
    }


    protected override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.K))
        {
            transform.Rotate(new Vector3(0, 10, 0));
        }
        if (Input.GetKey(KeyCode.L))
        {
            transform.Rotate(new Vector3(0, -10, 0));
        }

        if (Input.GetKey(KeyCode.O))
        {
            Debug.Log(target.ToString());
        }
        if (Input.GetKey(KeyCode.P))
        {
            target = null;
            MoveForward();
        }

        if (Input.GetKey(KeyCode.I))
        {
            // UnitController.Instance.RpcMoveCommand(this, new Vector3(0,0,0));
        }
        if (Input.GetKey(KeyCode.Y))
        {
            UnitController.Instance.StopUnit(this);
        }
    }

    private void MoveForward()
    {
        this.GetComponent<Rigidbody>().velocity = transform.forward * 10;
    }

    //Setup Unit's Info
    private string UnitInfo = "Hello! I'm someone who helps testing this game";
    public override string getInfo()
    {
        return UnitInfo;
    }

}
