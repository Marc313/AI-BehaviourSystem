public class BTSelector : BTComposite
{
    public BTSelector(BlackBoard _blackboard, params BTBaseNode[] _children) : base(_blackboard, _children)
    {
    }

    public override NodeStatus OnUpdate()
    {
        for (currentIndex = 0; currentIndex < children.Length; currentIndex++)
        {
            BTBaseNode node = children[currentIndex];
            switch (node.Evaluate())
            {
                case NodeStatus.Running: 
                    return NodeStatus.Running;

                // Succeed whenever one of the children succeeds
                case NodeStatus.Success:
                    currentIndex = 0;
                    return NodeStatus.Success;
                
                // Fail when all children fail
                case NodeStatus.Failed:
                    break;
            }
        }

        currentIndex = 0;
        return NodeStatus.Failed;
    }
}
