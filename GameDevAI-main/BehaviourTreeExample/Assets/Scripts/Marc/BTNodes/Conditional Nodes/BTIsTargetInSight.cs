using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTIsTargetInSight : BTCondition<bool>
{
    public override string displayName => "Check Target in Sight";

    private RaycastHit hitInSight;
    private Transform target;
    private Transform controllerTransform;
    private float inSightRange;

    private Collider targetCollider;

    public BTIsTargetInSight(BlackBoard _blackboard, Transform _target, float _inSightRange) : base(_blackboard)
    {
        target = _target;
        inSightRange = _inSightRange;
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        targetCollider = blackboard.Get<Collider>("TargetCollider");

        condition = (bool isInSight) => isInSight;
    }

    // Returns true when target is within angle range and distance is not obstructed by wall
    protected override bool GetComparedValue()
    {
        Vector3 directionToOther = (target.position - controllerTransform.position).normalized;
        if (Vector3.Dot(controllerTransform.forward, directionToOther) < inSightRange)
        {
            return false;
        }

        if (Physics.Raycast(controllerTransform.position, directionToOther, out hitInSight, 50))
        {
            if (hitInSight.collider.gameObject.transform != target)
            {
                // If there is no raycasthit, try to find a boxcasthit, since it is possible that only a part of the target is hidden.
                Vector3 boxExtends = targetCollider.bounds.extents;
                if (Physics.BoxCast(controllerTransform.position, directionToOther, boxExtends, out hitInSight, Quaternion.identity, 50))
                {
                    if (hitInSight.collider.gameObject.transform != target)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}
