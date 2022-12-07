using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BlockingEnemySightEvaluator : UtilityEvaluator
{
    private float maxDistance;
    private Transform controllerTransform;
    private NavMeshAgent agent;

    public BlockingEnemySightEvaluator(BlackBoard _blackBoard, float _maxDistance) : base(_blackBoard)
    {
        maxDistance = _maxDistance;
        controllerTransform = _blackBoard.Get<Transform>("ControllerTransform");
        agent = _blackBoard.Get<NavMeshAgent>("Agent");
    }

    protected override AnimationCurve GetCurve()
    {
        return blackBoard.Get<AnimationCurve>("BlockingEnemySightCurve");
    }

    protected override float GetMaxValue()
    {
        return 1;
    }

    protected override float GetMinValue()
    {
        return 0;
    }

    protected override float GetValue()
    {
        Collider[] collider = Physics.OverlapSphere(controllerTransform.position, maxDistance);
        Guard[] guards = collider.Select(collider => collider.GetComponent<Guard>())
                                    .Where(t => t != null)
                                    .ToArray();

        MonoBehaviour targetObject = blackBoard.Get<MonoBehaviour>("CurrentSearchItem");
        Vector3 targetObjectPos = targetObject.transform.position;

        float totalScore = 0;
        foreach (Guard guard in guards)
        {
            float distanceToTarget = Vector3.Distance(targetObjectPos, guard.transform.position);
            Vector3 direction = targetObjectPos - guard.transform.position;

            RaycastHit hit;
            if (Physics.Raycast(guard.transform.position, direction, out hit, distanceToTarget))
            {
                if (hit.collider.gameObject != guard.gameObject && hit.collider.gameObject != targetObject.gameObject)
                {
                    totalScore++;
                }
            }
        }

        return totalScore / guards.Length;
    }
}
