public class BTConditionDecorator : BTBaseNode
{
    public override string displayName => throw new System.NotImplementedException();

    private BTConditional conditionNode;
    private BTBaseNode childNode;

    public BTConditionDecorator(BlackBoard _blackboard, BTConditional _conditionNode, BTBaseNode _childNode) : base(_blackboard)
    {
        conditionNode = _conditionNode;
        childNode = _childNode;
    }

    public override NodeStatus OnUpdate()
    {
        NodeStatus conditionStatus = conditionNode.Evaluate();
        if (conditionStatus == NodeStatus.Success)
        {
            return childNode.Evaluate();
        }

        return conditionStatus;
    }
}
