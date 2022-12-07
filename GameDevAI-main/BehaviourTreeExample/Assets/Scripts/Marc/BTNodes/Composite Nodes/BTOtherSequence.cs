using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTOtherSequence : BTComposite
{
    public BTOtherSequence(BlackBoard _blackboard, params BTBaseNode[] _children) : base(_blackboard, _children)
    {
    }

    public override NodeStatus OnUpdate()
    {
        bool anyChildIsRunning = false;

        for(; currentIndex < children.Length; currentIndex++)
        {
            //Debug.Log("SequenceIndex: " + currentIndex);
            BTBaseNode node = children[currentIndex];
            switch (node.Evaluate())
            {
                case NodeStatus.Running:
                    anyChildIsRunning = true;
                    continue;

                case NodeStatus.Failed:
                    currentIndex = 0;
                    return NodeStatus.Failed;

                case NodeStatus.Success:
                    break;
            }
        }

        currentIndex = 0;
        return anyChildIsRunning ? NodeStatus.Running : NodeStatus.Success;
        return NodeStatus.Success;

    }
}
