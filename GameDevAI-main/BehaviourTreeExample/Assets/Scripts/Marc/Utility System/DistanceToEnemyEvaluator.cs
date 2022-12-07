using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DistanceToEnemyEvaluator : UtilityEvaluator
{
    private float maxDistance;
    private Transform controllerTransform;
    private NavMeshAgent agent;

    private Guard closestGuard;

    public DistanceToEnemyEvaluator(BlackBoard _blackBoard, float _maxDistance) : base(_blackBoard)
    {
        maxDistance = _maxDistance;
        controllerTransform = _blackBoard.Get<Transform>("ControllerTransform");
        agent = _blackBoard.Get<NavMeshAgent>("Agent");
    }

    protected override AnimationCurve GetCurve()
    {
        return blackBoard.Get<AnimationCurve>("EnemyDistanceCurve");
    }

    protected override float GetMaxValue()
    {
        return maxDistance;
    }

    protected override float GetMinValue()
    {
        return 0;
    }

    protected override float GetValue()
    {
        Collider[] collider = Physics.OverlapSphere(controllerTransform.position, maxDistance);
        closestGuard = collider.Select(collider => collider.GetComponent<Guard>())
                                    .Where(t => t != null)
                                    .First();

        float distance = agent.CalculateDistanceToTarget(closestGuard.transform.position);

        return distance;
        
    }
}
