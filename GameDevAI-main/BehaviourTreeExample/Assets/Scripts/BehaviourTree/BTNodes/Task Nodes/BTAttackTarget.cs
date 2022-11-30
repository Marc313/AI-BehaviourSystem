using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Graphs;
using UnityEngine;

public class BTAttackTarget : BTBaseNode
{
    public override string displayName => "Attacking";

    private IHealthUser playerTarget;
    private Transform controllerTransform;

    private int damage;

    // TODO: Make ITarget/ICharacter/IHealthUser
    public BTAttackTarget(BlackBoard _blackboard, GameObject _target, float _damage) : base(_blackboard)
    {
        playerTarget = _target.GetComponent<IHealthUser>();
        controllerTransform = _blackboard.Get<Transform>("ControllerTransform");
    }

    public override NodeStatus OnUpdate()
    {
        if (playerTarget == null) return NodeStatus.Failed;

        playerTarget.TakeDamage(damage);
        return NodeStatus.Success;
    }
}
