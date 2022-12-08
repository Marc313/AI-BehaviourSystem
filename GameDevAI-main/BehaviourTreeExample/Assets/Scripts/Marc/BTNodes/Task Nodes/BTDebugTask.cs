using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTDebugTask : BTBaseNode
{
    public override string displayName => "Debugging";
    private string message;

    public BTDebugTask(BlackBoard _blackboard, string _message) : base(_blackboard)
    {
        message= _message;
    }

    public override NodeStatus OnEnter()
    {
        return NodeStatus.Success;
    }
    public override NodeStatus OnExit()
    {
        return NodeStatus.Success;
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }
}
