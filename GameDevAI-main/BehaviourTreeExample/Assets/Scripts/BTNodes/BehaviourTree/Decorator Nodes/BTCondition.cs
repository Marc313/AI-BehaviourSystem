public class BTCondition : BTDecorator
{
    private bool condition;

    public BTCondition(BlackBoard _blackBoard, BTBaseNode _node, bool _condition) : base(_blackBoard, _node)
    {
        condition = _condition;
    }

    public override NodeStatus OnUpdate()
    {
        if (condition)
        {
            return child.Evaluate();
        }
        else
        {
            return NodeStatus.Failed;
        }
    }
}
