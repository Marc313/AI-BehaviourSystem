using System;

public class BTCustomCondition : BTBaseNode
{
    public delegate bool conditionCheck();
    public override string displayName => "Condition";

    private Func<bool> condition;

    public BTCustomCondition(BlackBoard _blackboard, Func<bool> _condition) : base(_blackboard)
    {
        condition = _condition;
    }

    public override NodeStatus OnUpdate()
    {
        if ((bool)condition?.Invoke())
        {
            return NodeStatus.Success;
        }

        return NodeStatus.Failed;
    }
}
