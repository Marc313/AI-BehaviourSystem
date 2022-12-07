using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class InSightEvaluator : UtilityEvaluator
{
    private float maxDistance;
    private Transform controllerTransform;
    private NavMeshAgent agent;

    public InSightEvaluator(BlackBoard _blackBoard, float _maxDistance) : base(_blackBoard)
    {
        maxDistance = _maxDistance;
        controllerTransform = _blackBoard.Get<Transform>("ControllerTransform");
        agent = _blackBoard.Get<NavMeshAgent>("Agent");
    }

    protected override AnimationCurve GetCurve()
    {
        return blackBoard.Get<AnimationCurve>("SightCurve");
    }

    protected override float GetMaxValue()
    {
        return 1;
    }

    protected override float GetMinValue()
    {
        return -1;
    }

    protected override float GetValue()
    {
        Collider[] collider = Physics.OverlapSphere(controllerTransform.position, maxDistance);
        Guard[] guards = collider.Select(collider => collider.GetComponent<Guard>())
                                    .Where(t => t != null)
                                    .ToArray();

        float highestScore = GetMinValue();
        foreach (Guard guard in guards)
        {
            float score = Vector3.Dot(guard.transform.forward, controllerTransform.forward);
            if (score > highestScore) highestScore = score;
        }

        return highestScore;
    }
}
