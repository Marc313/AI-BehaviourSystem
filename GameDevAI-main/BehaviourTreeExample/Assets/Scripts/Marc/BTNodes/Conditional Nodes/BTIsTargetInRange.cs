using UnityEngine;

public class BTIsTargetInRange : BTCondition<float>
{
    private Transform controllerTransform;
    private Transform target;

    public BTIsTargetInRange(BlackBoard _blackBoard, Transform _target, float _range) : base(_blackBoard)
    {
        target= _target;
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");

        condition = (float distanceToTarget) => { if (target == null) return false; else return distanceToTarget <= _range; };
    }

    protected override float GetComparedValue()
    {
        return Vector3.Distance(target.position, controllerTransform.position);
    }
}
