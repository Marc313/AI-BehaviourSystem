using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTIsAlerted : BTCondition<bool>
{
    private Guard guard;

    public BTIsAlerted(BlackBoard _blackBoard, Guard _guard) : base(_blackBoard)
    {
        guard = _guard;
        condition = (bool isAlerted) => { return isAlerted; };
    }

    protected override bool GetComparedValue()
    {
        return guard.isAlerted;
    }
}
