public class BTSucceeder : BTDecorator
{
    public override string displayName => child.displayName;

    public BTSucceeder(BlackBoard _blackBoard, BTBaseNode _node) : base(_blackBoard, _node)
    {
    }

    public override NodeStatus OnUpdate()
    {
        child.Evaluate();
        return NodeStatus.Success;
    }
}
