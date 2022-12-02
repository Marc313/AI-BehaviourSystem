using UnityEngine;
using UnityEngine.AI;

public class BTMoveToPosition : BTBaseNode
{
    public override string displayName => "Moving to Position";
    private NavMeshAgent agent;
    private Transform controllerTransform;

    private Vector3 targetPos;
    private float minDistance;
    private float distanceToDestination;


    public BTMoveToPosition(BlackBoard _blackboard, Vector3 _waypoint, float _minDistance) : base(_blackboard)
    {
        targetPos = _waypoint;
        minDistance = _minDistance;

        GetBlackboardValues();
    }

    private void GetBlackboardValues()
    {
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        agent = blackboard.Get<NavMeshAgent>("Agent");
    }

    public override NodeStatus OnEnter()
    {
        agent.enabled = true;
        agent.SetDestination(targetPos);

        return base.OnEnter();
    }

    public override NodeStatus OnUpdate()
    {
        distanceToDestination = Vector3.Distance(controllerTransform.position, targetPos);
        if (distanceToDestination > minDistance)
        {
            return NodeStatus.Running;
        }

        return NodeStatus.Success;
    }
}
