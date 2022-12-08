using UnityEngine;

public abstract class BTCondition<T> : BTBaseNode
{
    public override string displayName => "Check Target in Range";
    protected System.Predicate<T> condition;
    protected T comparedValue;
    private int field;

    public BTCondition(BlackBoard _blackBoard) : base(_blackBoard)
    {
    }

    public override NodeStatus OnUpdate()
    {
        comparedValue = GetComparedValue();
        bool? conditionResultNullable = condition?.Invoke(comparedValue);
        bool conditionResult = conditionResultNullable ?? false;            // When null, bool is false

        if (conditionResult)
        {
            return NodeStatus.Success;
            //return child.Evaluate();
        }
        else
        {
            return NodeStatus.Failed;
        }
    }

    protected abstract T GetComparedValue();
}
