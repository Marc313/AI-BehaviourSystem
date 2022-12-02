using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Evaluators/Float Evaluator")]
public class DistanceToTargetEvaluator : UtilityEvaluator
{
    private Transform target;
    private Transform controllerTransform;
    private float distanceToTarget;
    private float maxDistance;

    public DistanceToTargetEvaluator(BlackBoard _blackBoard, float _maxDistance) : base(_blackBoard)
    {
        maxDistance = _maxDistance;
        controllerTransform = blackBoard.Get<Transform>("ControllerTransform");
    }

    protected override AnimationCurve GetCurve()
    {
        return blackBoard.Get<AnimationCurve>("DistanceCurve");
    }

    protected override float GetMinValue()
    {
        return 0;
    }

    protected override float GetMaxValue()
    {
        return maxDistance;
    }

    protected override float GetValue()
    {
        Vector3 coverPos = blackBoard.Get<Vector3>("CurrentCoverPos");
        float distanceToCover = Vector3.Distance(controllerTransform.position, coverPos);

        return distanceToCover;
    }
}
