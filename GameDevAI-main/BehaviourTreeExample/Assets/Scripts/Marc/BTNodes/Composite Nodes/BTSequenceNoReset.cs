using UnityEngine;

public class BTSequenceNoReset : BTSequence
{
    public BTSequenceNoReset(BlackBoard _blackboard, params BTBaseNode[] _children) : base(_blackboard, _children)
    {
    }

    public override NodeStatus OnUpdate()
    {
        for (; currentIndex < children.Length; currentIndex++)
        {
            BTBaseNode node = children[currentIndex];
            switch (node.Evaluate())
            {
                case NodeStatus.Running: 
                    return NodeStatus.Running;

                case NodeStatus.Failed:
                    currentIndex = 0;
                    return NodeStatus.Failed;

                case NodeStatus.Success: 
                    break;
            }
        }

        currentIndex = 0;
        return NodeStatus.Success;
    }

    public override void Abort()
    {
        base.Abort();

        foreach (BTBaseNode child in children)
        {
            child.Abort();
        }
    }
}
