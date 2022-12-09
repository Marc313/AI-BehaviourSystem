using UnityEngine;

public class BTSpotTarget : BTBaseNode
{
    public override string displayName => "Spotted Target";

    private ISpottable spottable;
    private bool spotted;
    private GameObject thisGameObject;

    public BTSpotTarget(BlackBoard _blackBoard, ISpottable _spottable, bool _spotted) : base(_blackBoard)
    {
        spottable = _spottable;
        spotted = _spotted;
        thisGameObject = blackboard.Get<GameObject>("ControllerObject");
    }

    public override NodeStatus OnEnter()
    {
        if (spottable == null) return NodeStatus.Failed;

        spottable.Spot(thisGameObject, spotted);
        return base.OnEnter();
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }
}