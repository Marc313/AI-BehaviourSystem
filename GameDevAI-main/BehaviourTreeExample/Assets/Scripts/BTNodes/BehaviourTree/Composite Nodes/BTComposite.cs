public abstract class BTComposite : BTBaseNode
{
    protected BTBaseNode[] children;
    protected int currentIndex;

    public BTComposite(BlackBoard _blackboard, params BTBaseNode[] _children) : base(_blackboard)
    {
        children = _children;
    }
}
