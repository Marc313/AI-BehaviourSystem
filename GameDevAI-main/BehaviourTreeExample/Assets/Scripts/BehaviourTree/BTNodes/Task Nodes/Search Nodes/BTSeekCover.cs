using System.Collections.Generic;
using UnityEngine;

public class BTSeekCover : BTSeekObject<Cover>
{
    public override string displayName => "Seek Cover";

    private GameObject coverObject;
    private float inSightRange;
    private float maxCoverDistance;

    private List<Guard> guards = new List<Guard>();
    private Guard closestGuard;

    public BTSeekCover(BlackBoard _blackboard, float _inSightRange, float _maxDistance) : base(_blackboard, _maxDistance)
    {
        inSightRange = _inSightRange;
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }

    public override void SetupEvaluators()
    {
        evaluators = new List<UtilityEvaluator> 
        { 
            new DistanceToTargetEvaluator(blackboard, maxDistance)
        };
    }
}
