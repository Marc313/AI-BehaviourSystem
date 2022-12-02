using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class BTSeekWeapon : BTSeekObject<Weapon>
{
    public override string displayName => "Seeking Weapon";

    public BTSeekWeapon(BlackBoard _blackboard, float _maxDistance) : base(_blackboard, _maxDistance)
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
            new DistanceToTargetEvaluator(blackboard, maxDistance),
            new DamageEvaluator(blackboard)
        };
    }
}
