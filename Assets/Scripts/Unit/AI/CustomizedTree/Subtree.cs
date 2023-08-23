using System.Collections.Generic;

public class Subtree
{
    public static Node MoveOnlySubtree(Unit unit)
    {
        Node root = new Selector();
        Sequence moveSequence = new Sequence(new List<Node>
        {
            new CheckHasDestination(unit),
            new TaskMoveToDestination(unit)
        });

        root.Attach(moveSequence);

        return root;
    }

    public static Node BasicSubtree(Unit unit)
    {
        Node root = new Selector();




        root.Attach(StunSubtree(unit));
        root.Attach(MovementSubTree(unit));

        return root;
    }

    public static Node ShooterActionSubtree<T>(T unit) where T : Unit, IShooter
    {
        Sequence sequence = new Sequence();

        // Sequence recessionSequen = new Sequence(new List<Node>{new CheckTargetInRecessionRange(unit),new TaskRece});
        Sequence attackSequence = new Sequence(new List<Node>
            { new CheckTargetInAttackRange(unit), new FixedTimer(1 / unit.attackFreq, new TaskShootAttack<T>(unit)) });
        Selector selector = new Selector(new List<Node> { attackSequence, new TaskChaseTarget(unit) });
        sequence.Attach(new CheckHasTarget(unit));
        sequence.Attach(selector);
        return sequence;
    }
    


    public static Node ShooterSubtree<T>(T unit) where T : Unit, IShooter
    {
        Node root = new Selector();
        root.Attach(StunSubtree(unit));
        root.Attach(new TaskScout(unit));
        Selector selector = new Selector();
        selector.Attach(MovementSubTree(unit));
        selector.Attach(ShooterActionSubtree(unit));
        selector.Attach(new TaskIdle(unit));
        root.Attach(selector);
        return root;
    }
    public static Node RecessionShooterActionSubtree<T>(T unit) where T : Unit, IShooter
    {
        Sequence sequence = new Sequence();

        // Sequence recessionSequen = new Sequence(new List<Node>{new CheckTargetInRecessionRange(unit),new TaskRece});
        Sequence attackSequence = new Sequence(new List<Node>
            { new CheckTargetInAttackRange(unit), new FixedTimer(1 / unit.attackFreq, new TaskShootAttack<T>(unit)) });
        Selector selector = new Selector();
        Sequence sequence1 = new Sequence(new List<Node>{new CheckEnemyInRecessionRange(unit),new TaskRecession(unit)});
        selector.Attach(sequence1);
        selector.Attach(attackSequence);
        selector.Attach(new TaskChaseTarget(unit));
        
        sequence.Attach(new CheckHasTarget(unit));
        sequence.Attach(selector);
        return sequence;
    }
    
    public static Node RecessionShooterSubtree<T>(T unit) where T : Unit, IShooter
    {
        Node root = new Selector();
        root.Attach(StunSubtree(unit));
        root.Attach(new TaskScout(unit));
        Selector selector = new Selector();
        selector.Attach(MovementSubTree(unit));
        selector.Attach(RecessionShooterActionSubtree(unit));
        selector.Attach(new TaskIdle(unit));
        root.Attach(selector);
        return root;
    }
    
    public static Node MeleeActionSubtree<T>(T unit) where T : Unit, IMelee
    {
        Sequence sequence = new Sequence();

        // Sequence recessionSequen = new Sequence(new List<Node>{new CheckTargetInRecessionRange(unit),new TaskRece});
        Sequence attackSequence = new Sequence(new List<Node>
            { new CheckTargetInAttackRange(unit), new FixedTimer(1 / unit.attackFreq, new TaskMeleeAttack<T>(unit)) });
        Selector selector = new Selector(new List<Node> { attackSequence, new TaskChaseTarget(unit) });
        sequence.Attach(new CheckHasTarget(unit));
        sequence.Attach(selector);
        return sequence;
    }

    public static Node MeleeSubtree<T>(T unit) where T : Unit, IMelee
    {
        Node root = new Selector();
        root.Attach(StunSubtree(unit));
        root.Attach(new TaskScout(unit));
        Selector selector = new Selector();
        selector.Attach(MovementSubTree(unit));
        selector.Attach(MeleeActionSubtree(unit));
        selector.Attach(new TaskIdle(unit));
        root.Attach(selector);
        return root;
    }

    private static Node StunSubtree<T>(T unit) where T : Unit
    {
        Sequence stunSequence = new Sequence(new List<Node> { new CheckIsStunned(unit), new TaskStunning(unit) });
        return stunSequence;
    }

    private static Node MovementSubTree<T>(T unit) where T : Unit
    {
        Sequence movementSequence = new Sequence();
        Sequence sequence1 = new Sequence(new List<Node>
            { new CheckIsForcedMovement(unit), new TaskMoveToDestination(unit) });
        Inverter inverter = new Inverter(new List<Node> { new CheckHasTarget(unit) });
        Sequence sequence2 = new Sequence(new List<Node> { inverter, new TaskMoveToDestination(unit) });
        Selector selector = new Selector(new List<Node> { sequence1, sequence2 });
        movementSequence.Attach(new CheckHasDestination(unit));
        movementSequence.Attach(selector);
        return movementSequence;
    }
}