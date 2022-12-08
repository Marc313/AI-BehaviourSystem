using UnityEngine;

public class BTHasWeapon : BTCondition<bool>
{
    public IWeaponWielder weaponWielder;

    public BTHasWeapon(BlackBoard _blackBoard, IWeaponWielder _weaponWielder) : base(_blackBoard)
    {
        weaponWielder = _weaponWielder;
        condition = (bool b) => b;
    }

    protected override bool GetComparedValue()
    {
        return weaponWielder.weapon != null;
    }
}
