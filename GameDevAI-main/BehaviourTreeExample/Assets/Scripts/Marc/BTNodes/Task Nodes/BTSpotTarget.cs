using UnityEngine;

public class BTSpotTarget : BTBaseNode
{
    public override string displayName => "Spotted Target";

    private ISpottable spottable;
    private bool spotted;

    public BTSpotTarget(BlackBoard _blackBoard, ISpottable _spottable, bool _spotted) : base(_blackBoard)
    {
        spottable = _spottable;
        spotted = _spotted;
    }

    public override NodeStatus OnEnter()
    {
        if (spottable == null) return NodeStatus.Failed;

        spottable.isSpotted = spotted;
        return base.OnEnter();
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }
}