using UnityEngine;
using UnityEngine.AI;

public class BTFollowTarget : BTBaseNode
{
    public override string displayName => "Follow Target";
    private Transform controllerTransform;
    private Transform target;
    private NavMeshAgent agent;
    private float minDistance;
    private float distanceToTarget;
    private bool shouldKeepDistance;

    public BTFollowTarget(BlackBoard _blackboard, Transform _target, float _minDistance, bool _shouldKeepDistance) : base(_blackboard)
    {
        target = _target;
        shouldKeepDistance= _shouldKeepDistance;

        minDistance = _minDistance;
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        agent = blackboard.Get<NavMeshAgent>("Agent");
    }

    public override NodeStatus OnUpdate()
    {
        agent.enabled = true;
        agent.SetDestination(target.position);

        distanceToTarget = Vector3.Distance(controllerTransform.position, target.position);
        if (distanceToTarget > minDistance)
        {
            return NodeStatus.Running;
        }
        else
        {
            if (shouldKeepDistance) { agent.enabled= false; }

            return NodeStatus.Success;
        }
    }
}