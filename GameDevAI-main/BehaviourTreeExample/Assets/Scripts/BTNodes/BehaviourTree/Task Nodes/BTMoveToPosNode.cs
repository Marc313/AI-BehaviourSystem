using UnityEngine;
using UnityEngine.AI;

public class BTMoveToPosNode : BTBaseNode
{
    private NavMeshAgent agent;
    private Transform controllerTransform;
    private Animator anim;

    private Vector3 targetPos;
    private float minDistance;  // Rename this variable
    private float distanceToDestination;

    public BTMoveToPosNode(BlackBoard _blackboard, Vector3 _waypoint, float _minDistance) : base(_blackboard)
    {
        targetPos = _waypoint;
        minDistance = _minDistance;

        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        agent = blackboard.Get<NavMeshAgent>("Agent");
        //anim = blackboard.Get<Animator>("Animator");

    }

    public override NodeStatus OnEnter()
    {
        agent.SetDestination(targetPos);
        // agent.enabled = true;

        //anim.CrossFade("RifleWalk", 0);

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

    public override NodeStatus OnExit()
    {
        //agent.isStopped = true;
        //anim.CrossFade("Idle", 0f);


        return base.OnExit();
    }
}
