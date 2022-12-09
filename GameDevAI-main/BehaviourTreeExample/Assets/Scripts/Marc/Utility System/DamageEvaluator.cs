using UnityEngine;

public class DamageEvaluator : UtilityEvaluator
{
    public DamageEvaluator(BlackBoard _blackBoard) : base(_blackBoard)
    {
    }

    protected override AnimationCurve GetCurve()
    {
        return blackBoard.Get<AnimationCurve>("DamageCurve");
    }

    protected override float GetMinValue()
    {
        return 0;
    }

    protected override float GetMaxValue()
    {
        return 100;
    }

    protected override float GetValue()
    {
        // More Modular: IDamager
        Weapon weapon = blackBoard.Get<Weapon>("CurrentSearchItem");
        if (weapon != null)
        {
            return weapon.damage;
        }
        return 0;
    }
}
