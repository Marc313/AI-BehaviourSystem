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

    public BTFollowTarget(BlackBoard _blackboard, Transform _target, float _minDistance) : base(_blackboard)
    {
        target = _target;
        //shouldKeepDistance= _shouldKeepDistance;

        minDistance = _minDistance;
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        agent = blackboard.Get<NavMeshAgent>("Agent");

    }

    public override NodeStatus OnEnter()
    {
        agent.enabled = true;
        agent.isStopped = false;
        return NodeStatus.Success;
    }

    public override NodeStatus OnUpdate()
    {
        agent.SetDestination(target.position);

        distanceToTarget = Vector3.Distance(controllerTransform.position, target.position);
        if (distanceToTarget > minDistance)
        {
            return NodeStatus.Running;
        }
        else
        {
            return NodeStatus.Success;
        }
    }

    public override NodeStatus OnExit()
    {
        agent.SetDestination(controllerTransform.position);
        agent.isStopped = true;
        return NodeStatus.Success;
    }

/*    public override void Abort()
    {
        agent.SetDestination(controllerTransform.position);
    }*/
}