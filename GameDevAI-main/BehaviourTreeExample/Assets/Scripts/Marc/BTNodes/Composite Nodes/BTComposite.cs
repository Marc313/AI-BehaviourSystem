public abstract class BTComposite : BTBaseNode
{
    public override string displayName => children[currentIndex].displayName;
    protected BTBaseNode[] children;
    protected int currentIndex;

    public BTComposite(BlackBoard _blackboard, params BTBaseNode[] _children) : base(_blackboard)
    {
        children = _children;
    }
}
