using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTInvert : BTDecorator
{
    public override string displayName => child.displayName;

    public BTInvert(BlackBoard _blackBoard, BTBaseNode _node) : base(_blackBoard, _node)
    {
    }

    public override NodeStatus OnUpdate()
    {
        switch (child.Evaluate())
        {
            case NodeStatus.Success: return NodeStatus.Failed;
            case NodeStatus.Failed: return NodeStatus.Success;
            case NodeStatus.Running: return NodeStatus.Running;
            default: return NodeStatus.Success;
        }
    }
}
