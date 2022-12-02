using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTPickupWeapon : BTBaseNode
{
    public override string displayName => "Pickup Weapon";

    private Guard guardBase;

    public BTPickupWeapon(BlackBoard _blackboard) : base(_blackboard)
    {
        guardBase = blackboard.Get<Guard>("Base");
    }

    public override NodeStatus OnEnter()
    {
        Weapon weapon = blackboard.Get<Weapon>("BestWeapon");
        guardBase.weapon = weapon;

        if (weapon == null) return NodeStatus.Failed;
        else return NodeStatus.Success;
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }
}
