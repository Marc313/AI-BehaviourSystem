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

    public DistanceToTargetEvaluator(BlackBoard _blackBoard, float _maxDistance) : base(_blackBoard)
    {
        maxDistance = _maxDistance;
        controllerTransform = _blackBoard.Get<Transform>("ControllerTransform");
        agent = _blackBoard.Get<NavMeshAgent>("Agent");
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
        Vector3 objectPos = blackBoard.Get<MonoBehaviour>("CurrentSearchItem").transform.position;
        float distanceToObject = Vector3.Distance(controllerTransform.position, objectPos);

        //float distanceToObject = agent.CalculatePath(objectPos);

        return distanceToObject;
    }
}
