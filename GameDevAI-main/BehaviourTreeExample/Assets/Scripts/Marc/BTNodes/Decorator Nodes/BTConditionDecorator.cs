using UnityEngine;

public class BTConditionDecorator : BTBaseNode
{
    public override string displayName => childNode.displayName;

    private BTBaseNode conditionNode;
    private BTBaseNode childNode;

    public BTConditionDecorator(BlackBoard _blackboard, BTBaseNode _conditionNode, BTBaseNode _childNode) : base(_blackboard)
    {
        conditionNode = _conditionNode;
        childNode = _childNode;
    }

    public override NodeStatus OnUpdate()
    {
        NodeStatus conditionStatus = conditionNode.Evaluate();
        if (conditionStatus == NodeStatus.Success)
        {
            //Debug.Log("Condition Still True");
            return childNode.Evaluate();
        }

        return conditionStatus;
    }
}
