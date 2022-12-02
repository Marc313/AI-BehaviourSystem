using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTInvokeAction : BTBaseNode
{
    public override string displayName => "Invoked Action";

    private Action action;

    public BTInvokeAction(BlackBoard _blackboard, System.Action _action) : base(_blackboard)
    {
        action = _action;
    }

    public override NodeStatus OnEnter()
    {
        action.Invoke();
        return base.OnEnter();
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }
}
