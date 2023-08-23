using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintLog : Node
{
    private string input;
    public PrintLog(string input) : base()
    {
        this.input = input;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(input);
        return NodeState.SUCCESS;
    }
}
