using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTChangeBlackBoardVariable : BTBaseNode
{
    public override string displayName => "";

    private string id;
    private object newValue;

    public BTChangeBlackBoardVariable(BlackBoard _blackboard, string _id, object _newValue) : base(_blackboard)
    {
        id = _id;
        newValue = _newValue;
    }

    public override NodeStatus OnUpdate()
    {
        blackboard.AddOrUpdate(id, newValue);
        return NodeStatus.Success;
    }
}
