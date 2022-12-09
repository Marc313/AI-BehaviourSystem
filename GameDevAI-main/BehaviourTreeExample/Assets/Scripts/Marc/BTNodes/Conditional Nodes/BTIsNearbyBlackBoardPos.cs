using UnityEngine;
using UnityEngine.AI;

public class BTIsNearbyBlackBoardPos : BTCondition<bool>
{
    public override string displayName => "Moving to Position";
    private Transform controllerTransform;

    private Vector3 targetPos;
    private float minDistance;
    private string waypointBlackboardID;

    public BTIsNearbyBlackBoardPos(BlackBoard _blackboard, string _waypointBlackboardID, float _minDistance) : base(_blackboard)
    {
        waypointBlackboardID = _waypointBlackboardID;
        minDistance = _minDistance;
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
    }

    public override NodeStatus OnEnter()
    {
        if (blackboard.Contains(waypointBlackboardID))
        {
            targetPos = blackboard.Get<Vector3>(waypointBlackboardID);
            return NodeStatus.Success;
        }
        else
        {
            return NodeStatus.Failed;
        }
    }

    protected override bool GetComparedValue()
    {
        return Vector3.Distance(controllerTransform.position, targetPos) < minDistance;
    }
}
