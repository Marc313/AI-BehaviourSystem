using UnityEngine;
using UnityEngine.AI;

public class BTMoveToPosNode : BTBaseNode
{
    public override string displayName => "Patrolling";
    private NavMeshAgent agent;
    private Transform controllerTransform;
    private Animator anim;

    private Vector3 targetPos;
    private float minDistance;  // Rename this variable
    private float distanceToDestination;

    private string waypointBlackboardID;

    public BTMoveToPosNode(BlackBoard _blackboard, Vector3 _waypoint, float _minDistance) : base(_blackboard)
    {
        targetPos = _waypoint;
        minDistance = _minDistance;

        GetBlackboardValues();
    }

    public BTMoveToPosNode(BlackBoard _blackboard, string _waypointBlackboardID, float _minDistance) : base(_blackboard)
    {
        minDistance = _minDistance;
        waypointBlackboardID = _waypointBlackboardID;

        GetBlackboardValues();
    }

    private void GetBlackboardValues()
    {
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        agent = blackboard.Get<NavMeshAgent>("Agent");
    }

    public override NodeStatus OnEnter()
    {
        if (waypointBlackboardID != default && targetPos == default)
        {
            if (blackboard.Contains(waypointBlackboardID))
            {
                targetPos = blackboard.Get<Vector3>(waypointBlackboardID);
                Debug.Log("Cover: " + targetPos);
            }
            else
            {
                return NodeStatus.Failed;
            }
        }


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

    public override NodeStatus OnExit()
    {
        return base.OnExit();
    }
}
