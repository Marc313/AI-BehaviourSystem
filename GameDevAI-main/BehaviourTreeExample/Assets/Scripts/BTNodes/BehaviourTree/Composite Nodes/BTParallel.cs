using UnityEngine;

public class BTParallel : BTComposite
{
    public BTParallel(BlackBoard _blackboard, params BTBaseNode[] _children) : base(_blackboard, _children)
    {
    }

    public override NodeStatus OnUpdate()
    {
        foreach (BTBaseNode node in children)
        {
            switch (node.Evaluate())
            {
                case NodeStatus.Running:
                    return NodeStatus.Running;

                case NodeStatus.Failed:
                    return NodeStatus.Failed;

                case NodeStatus.Success:
                    break;
            }
        }

        Debug.Log("Parallel Success");
        return NodeStatus.Success;
    }
}
