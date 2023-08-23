

public class CheckUnitIsMine : Node
{
    private bool _unitIsMine;

    public CheckUnitIsMine(Unit unit) : base()
    {
        _unitIsMine = unit.Owner == Player.Instance.MyPlayerRef();
    }

    public override NodeState Evaluate()
    {
        _state = _unitIsMine ? NodeState.SUCCESS : NodeState.FAILURE;
        return _state;
    }
} 