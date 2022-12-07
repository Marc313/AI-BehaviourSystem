using UnityEngine;

public class BTIsCooldownOver : BTCondition<float>
{
    private string blackBoardID;
    private float cooldown;

    public BTIsCooldownOver(BlackBoard _blackBoard, string _blackBoardID, float _cooldown) : base(_blackBoard)
    {
        blackBoardID = _blackBoardID;
        cooldown = _cooldown;
        condition = (float lastUseTimeDifference) => cooldown < lastUseTimeDifference;
    }

    protected override float GetComparedValue()
    {
        if (blackboard.Contains(blackBoardID))
        {
            return Time.time - blackboard.Get<float>(blackBoardID);
        }
        else
        {
            return float.MaxValue;
        }
    }
}
