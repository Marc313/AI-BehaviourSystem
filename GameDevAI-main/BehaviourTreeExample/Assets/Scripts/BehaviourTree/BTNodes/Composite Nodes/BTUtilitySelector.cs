using System.Linq;

public class BTUtilitySelector : BTSelector
{
    public BTUtilitySelector(BlackBoard _blackboard, params BTBaseNode[] _children) : base(_blackboard, _children)
    {
    }

    public override NodeStatus OnUpdate()
    {
        // Sort children by State Score
        children = children.OrderByDescending(node => node.utilityScore).ToArray();
        return base.OnUpdate();
    }
}
