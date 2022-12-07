using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Evaluators/Float Evaluator")]
public class DistanceToTargetEvaluator : UtilityEvaluator
{
    private Transform target;
    private Transform controllerTransform;
    private NavMeshAgent agent;
    private float distanceToTarget;
    private float maxDistance;

    public DistanceToTargetEvaluator(BlackBoard _blackBoard, float _maxDistance, AnimationCurve _curve = null) : base(_blackBoard)
    {
        maxDistance = _maxDistance;
        controllerTransform = _blackBoard.Get<Transform>("ControllerTransform");
        agent = _blackBoard.Get<NavMeshAgent>("Agent");
        curve = _curve;
    }

    protected override AnimationCurve GetCurve()
    {
        if (curve == null)
        {
            return blackBoard.Get<AnimationCurve>("CoverDistanceCurve");
        }

        return curve;
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
        Vector3 targetObjectPos = blackBoard.Get<MonoBehaviour>("CurrentSearchItem").transform.position;

        float distanceToTarget = agent.CalculateDistanceToTarget(targetObjectPos);

        return distanceToTarget;
    }
}
