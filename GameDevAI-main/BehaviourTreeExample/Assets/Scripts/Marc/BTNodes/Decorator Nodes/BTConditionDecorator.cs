using UnityEngine;

public class BTConditionDecorator : BTDecorator
{
    public override string displayName => childNode.displayName;

    private BTBaseNode conditionNode;
    private BTBaseNode childNode;

    public BTConditionDecorator(BlackBoard _blackboard, BTBaseNode _conditionNode, BTBaseNode _childNode) : base(_blackboard, _childNode)
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

    public override void Abort()
    {
        base.Abort();

        conditionNode.Abort();
        childNode.Abort();
    }
}
