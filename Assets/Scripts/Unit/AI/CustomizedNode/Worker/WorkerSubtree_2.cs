using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSubtree_2
{
    public static Node WorkerTree(RedBloodCell unit)
    {
        Node root = new Selector();

        Sequence moveSequence = new Sequence(new List<Node>
        {
            //new PrintLog("moveSequence 1"),

            new CheckHasDestination(unit),

            //new PrintLog("moveSequence 2"),

            new TaskMoveToDestination(unit),

            //new PrintLog("moveSequence"),

            new StopWhenBuilding(unit),

            //new PrintLog("moveSequence  end"),

        });

        //Selector minerActions = new Selector(new List<Node>
        //{
        //    new CheckNearBaseAndSubmitResource(unit),
            
        //    //new CheckNearResourcePointAndStartMining(unit),

        //    new SetWorkerMoveToMineOrBase(unit),
        //});

        Sequence mineSequence = new Sequence(new List<Node>
        {
            //new PrintLog("mineSequence1"),

            new CheckWorkerMining(unit),
            new PrintLog("mineSequence2"),

            new Selector(new List<Node> { //getResourcePoint if not having any
                new CheckOccupyingResourcePoint(unit),
                new UpdateResourcePoint(unit),
            }),
            new PrintLog("mineSequence3"),

            new SetWorkerMoveToMineOrBase(unit),

            new PrintLog("mineSequence5"),

        });

        Sequence buildSequence = new Sequence(new List<Node>
        {
            //new PrintLog("buildSequence1"),

            new CheckWorkerBuilding(unit),

            new CheckWorkerHasBuildTask(unit),

            new MoveToBuildingCoordinate(unit),

            new Build(unit),

        });

        Selector WorkerActions = new Selector(new List<Node>
        {
            mineSequence,
            buildSequence,

        });

        root.Attach(moveSequence);
        root.Attach(WorkerActions);
        return root;
    }
}
