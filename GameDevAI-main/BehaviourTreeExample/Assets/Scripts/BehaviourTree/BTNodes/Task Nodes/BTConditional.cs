using System;

public class BTConditional : BTBaseNode
{
    public delegate bool conditionCheck();
    public override string displayName => "Condition";

    private Func<bool> condition;

    public BTConditional(BlackBoard _blackboard, Func<bool> _condition) : base(_blackboard)
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
