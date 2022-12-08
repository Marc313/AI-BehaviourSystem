using UnityEngine;

public class BTSequence : BTComposite
{
    public BTSequence(BlackBoard _blackboard, params BTBaseNode[] _children) : base(_blackboard, _children)
    {
    }

    public override NodeStatus OnEnter()
    {
        currentIndex= 0;
        Debug.Log("Enter Sequence");
        return NodeStatus.Success;
    }

    public override NodeStatus OnUpdate()
    {
        for (; currentIndex < children.Length; currentIndex++)
        {
            //Debug.Log("SequenceIndex: " + currentIndex);
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

    public override NodeStatus OnExit()
    {
        currentIndex = 0;

        return NodeStatus.Success;
    }

    public override void Abort()
    {
        base.Abort();

        currentIndex = 0;

        foreach (BTBaseNode child in children)
        {
            child.Abort();
        }
    }
}
