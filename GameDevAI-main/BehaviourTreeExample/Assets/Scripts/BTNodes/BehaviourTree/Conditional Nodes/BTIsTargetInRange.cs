using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTIsTargetInRange : BTCondition<float>
{
    private Transform controllerTransform;
    private Transform target;
    //private float range;

    public BTIsTargetInRange(BlackBoard _blackBoard, Transform _target, float _range, BTBaseNode _node) : base(_blackBoard, _node)
    {
        target= _target;
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");

        condition = (float distanceToTarget) => distanceToTarget < _range;
    }

    protected override float GetComparedValue()
    {
        return Vector3.Distance(target.position, controllerTransform.position);
    }
}
