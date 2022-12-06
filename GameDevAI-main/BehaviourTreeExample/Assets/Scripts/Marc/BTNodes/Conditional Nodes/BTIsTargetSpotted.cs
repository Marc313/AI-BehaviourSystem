using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTIsTargetSpotted : BTCondition<bool>
{
    private ISpottable spottable;

    public BTIsTargetSpotted(BlackBoard _blackBoard, ISpottable _spottable) : base(_blackBoard)
    {
        spottable = _spottable;
        condition = (bool isSpotted) => isSpotted;
    }

    protected override bool GetComparedValue()
    {
        return spottable.isSpotted;
    }
}
