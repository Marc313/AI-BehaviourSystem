public class BTSequence : BTComposite
{
    public BTSequence(BlackBoard _blackboard, params BTBaseNode[] _children) : base(_blackboard, _children)
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
}
