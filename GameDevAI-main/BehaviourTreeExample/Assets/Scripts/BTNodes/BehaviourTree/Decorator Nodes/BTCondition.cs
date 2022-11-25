using UnityEngine;

public abstract class BTCondition<T> : BTDecorator
{
    public override string displayName => child.displayName;
    protected System.Predicate<T> condition;
    protected T comparedValue;

    public BTCondition(BlackBoard _blackBoard, BTBaseNode _node) : base(_blackBoard, _node)
    {
    }

    public override NodeStatus OnUpdate()
    {
        comparedValue = GetComparedValue();
        bool? conditionResultNullable = condition?.Invoke(comparedValue);
        bool conditionResult = conditionResultNullable ?? false;            // When null, bool is false

        if (conditionResult)
        {
            return child.Evaluate();
        }
        else
        {
            return NodeStatus.Failed;
        }
    }

    protected abstract T GetComparedValue();
}
