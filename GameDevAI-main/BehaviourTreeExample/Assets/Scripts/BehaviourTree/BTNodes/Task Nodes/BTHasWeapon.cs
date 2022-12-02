using UnityEngine;

public class BTHasWeapon : BTCondition<bool>
{
    public Guard guard;

    public BTHasWeapon(BlackBoard _blackBoard, Guard _guard) : base(_blackBoard)
    {
        guard = _guard;
    }

    protected override bool GetComparedValue()
    {
        return guard.weapon != null;
    }
}
