using UnityEngine;

public class BTPatrolNode : BTBaseNode
{
    public override string displayName => "Patrolling";
    private Vector3[] waypoints;
    private float minDistance;

    private BTSequence sequenceNode;

    public BTPatrolNode(BlackBoard _blackboard, float _minDistance, params Vector3[] _waypoints) : base(_blackboard)
    {
        minDistance = _minDistance;
        waypoints = _waypoints;
    }

    public override NodeStatus OnEnter()
    {
        BTMoveToPosNode[] children = new BTMoveToPosNode[waypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            children[i] = new BTMoveToPosNode(blackboard, waypoints[i], minDistance);
        }

        sequenceNode = new BTSequence(blackboard, children);

        return sequenceNode.OnEnter();
    }

    public override NodeStatus OnUpdate()
    {
        return sequenceNode.OnUpdate();
    }

    public override NodeStatus OnExit()
    {
        return sequenceNode.OnExit();
    }
}
