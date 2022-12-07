using UnityEngine;

public class BTRegisterCooldownTime : BTBaseNode
{
    public override string displayName => "Register Cooldown";

    private string blackBoardID;

    public BTRegisterCooldownTime(BlackBoard _blackboard, string _blackBoardID) : base(_blackboard)
    {
        blackBoardID = _blackBoardID;
    }

    public override NodeStatus OnEnter()
    {
        float currentTime = Time.time;
        blackboard.AddOrUpdate(blackBoardID, currentTime);   
        return NodeStatus.Success;
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }
}
