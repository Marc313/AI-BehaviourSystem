using System.Collections.Generic;
using UnityEngine;

public class BTSeekCover : BTSeekObject<Cover>
{
    public override string displayName => "Seek Cover";

    public BTSeekCover(BlackBoard _blackboard, float _maxDistance) : base(_blackboard, _maxDistance)
    {
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }

    public override void SetupEvaluators()
    {
        evaluators = new List<UtilityEvaluator>
        {
            // Instead of evaluating distance from ninja to cover, do it from cover to target (ninja, guard, etc.)
            new DistanceToTargetEvaluator(blackboard, maxDistance),
            new DistanceToEnemyEvaluator(blackboard, maxDistance),
            new InSightEvaluator(blackboard, maxDistance),
            new BlockingEnemySightEvaluator(blackboard, maxDistance) 
        };
    }
}
