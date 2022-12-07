using UnityEngine;

public abstract class UtilityEvaluator
{
    protected BlackBoard blackBoard;
    protected AnimationCurve curve;

    public UtilityEvaluator(BlackBoard _blackBoard)
    {
        blackBoard = _blackBoard;
    }

    public virtual float GetNormalizedScore()
    {
        float normalizedValue = Mathf.InverseLerp(GetMinValue(), GetMaxValue(), GetValue());
        return Mathf.Clamp01(GetCurve().Evaluate(normalizedValue));
    }

    protected abstract float GetValue();
    protected abstract float GetMinValue();
    protected abstract float GetMaxValue();
    protected abstract AnimationCurve GetCurve();
}
