using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;
using UnityEngine.Serialization;

public class WorkerSubtree
{
    public static Node WorkerSubtree_2(RedBloodCell unit)
    {
        Node root = new Selector();


        Sequence moveSequence = new Sequence(new List<Node> //this would remove the targets
        {
            new CheckHasDestination(unit),
            new TaskMoveToDestination(unit),
            new PrintLog("moveSequence ")
        });


        Selector occupyResourcePointSelector = new Selector(new List<Node> 
        //check if OccupyingResourcePoint, if not, go find one
        {
            new PrintLog("occupyResourcePointSelector"),

            new CheckOccupyingResourcePoint(unit),
            new PrintLog("CheckOccupyingResourcePoint"),

            new UpdateResourcePoint(unit),
            new PrintLog("UpdateResourcePoint"),

        });

        Selector checkStopMineResourceSelector = new Selector(new List<Node> //the sequence of worker mining
        {
            new PrintLog("checkStopMineResourceSelector"),

            new CheckWorkerNearResourcePoint(unit),
            new PrintLog("CheckWorkerNearResourcePoint"),


            //new StopMining(unit),
            new PrintLog("StopMining"),

        });

        Sequence checkMineResourceSequence = new Sequence(new List<Node> //the sequence of worker mining
        {
            new PrintLog("checkMineResourceSequence"),

            new CheckWorkerNearResourcePoint(unit),
            new PrintLog("CheckWorkerNearResourcePoint"),

            //new StartMining(unit),
            new PrintLog("StartMining"),

        });

        Sequence checkSubmitResourceSequence = new Sequence(new List<Node> //the sequence of worker mining
        {
            new PrintLog("checkSubmitResourceSequence"),

            //new CheckWorkerNearBase(unit),
            new PrintLog("CheckWorkerNearBase"),

            new SubmitResources(unit),
            new PrintLog("SubmitResources"),

        });

        Selector submitResourceOrMineSelector = new Selector(new List<Node>
        //check if near base (then submit resources) or resource point (then mine)
        {
            checkSubmitResourceSequence,
            checkStopMineResourceSelector,
            checkMineResourceSequence
        });

        Sequence miningSequence = new Sequence(new List<Node> //the sequence of worker mining
        {
            new CheckWorkerMining(unit),
            occupyResourcePointSelector,
            new PrintLog("occupyResourcePointSelector"),

            new SetWorkerMoveToMineOrBase(unit),
            new PrintLog("SetWorkerMoveToMineOrBase"),

            submitResourceOrMineSelector
            ,new PrintLog("submitResourceOrMineSelector")
        });

        Selector workingSelector = new Selector(new List<Node> //inside there are mining or building job
        {
            miningSequence
        });

        Sequence workSequence = new Sequence(new List<Node>
        {
            new CheckWorkerWorking(unit),
            workingSelector
        });



        root.Attach(moveSequence);
        root.Attach(workingSelector);


        return root;
    }
}
