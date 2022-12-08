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
                    AbortUpcomingNodes();
                    currentIndex = 0;
                    return NodeStatus.Running;

                // Succeed whenever one of the children succeeds
                case NodeStatus.Success:
                    AbortUpcomingNodes();
                    currentIndex = 0;
                    return NodeStatus.Success;
                
                // Move to next node when current node fails
                case NodeStatus.Failed:
                    break;
            }
        }

        // Fail when all children fail
        currentIndex = 0;
        return NodeStatus.Failed;
    }

    public override void Abort()
    {
        /*foreach (BTBaseNode child in children)
        {
            child.Abort();
        }*/
    }

    private void AbortUpcomingNodes()
    {
        currentIndex++;
        for (; currentIndex < children.Length; currentIndex++)
        {
            BTBaseNode node = children[currentIndex];
            node.Abort();
        }
    }
}
