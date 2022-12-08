using UnityEngine;
using UnityEngine.AI;

public class BTMoveToBlackboardPos : BTBaseNode
{
    public override string displayName => "Moving to Position";
    private NavMeshAgent agent;
    private Transform controllerTransform;

    private Vector3 targetPos;
    private float minDistance;
    private float distanceToDestination;
    private string waypointBlackboardID;

    public BTMoveToBlackboardPos(BlackBoard _blackboard, string _waypointBlackboardID, float _minDistance) : base(_blackboard)
    {
        minDistance = _minDistance;
        waypointBlackboardID = _waypointBlackboardID;

        GetBlackboardValues();
    }

    public override NodeStatus OnEnter()
    {
        if (blackboard.Contains(waypointBlackboardID))
        {
            targetPos = blackboard.Get<Vector3>(waypointBlackboardID);

            agent.enabled = true;
            agent.isStopped = false;
            agent.SetDestination(targetPos);

            return NodeStatus.Success;
        }
        else
        {
            return NodeStatus.Failed;
        }
    }

    public override NodeStatus OnUpdate()
    {
        if (agent.destination != targetPos)
        {
            agent.SetDestination(targetPos);
        }

        distanceToDestination = Vector3.Distance(controllerTransform.position, targetPos);
        if (distanceToDestination > minDistance)
        {
            return NodeStatus.Running;
        }

        return NodeStatus.Success;
    }

    public override NodeStatus OnExit()
    {
        agent.SetDestination(controllerTransform.position);
        return NodeStatus.Success;
    }

/*    public override void Abort()
    {
        agent.SetDestination(controllerTransform.position);
    }*/

    private void GetBlackboardValues()
    {
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        agent = blackboard.Get<NavMeshAgent>("Agent");
    }
}
