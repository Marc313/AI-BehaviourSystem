using UnityEngine;

public class BTAnimate : BTBaseNode
{
    public override string displayName => $"Animation ({clip})";

    private Animator animator;
    private string clip;
    private float fadeTime;

    public BTAnimate(BlackBoard _blackboard, string _clip, float _fadeTime = 0f) : base(_blackboard)
    {
        clip = _clip;
        fadeTime = _fadeTime;
        animator = blackboard.Get<Animator>("Animator");
    }

    public override NodeStatus OnEnter()
    {

        return base.OnEnter();
    }

    private void SwitchAnimation(string _clipName, float _fadeTime)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(_clipName) /*&& !animator.IsInTransition(0)*/)
        {
            animator.CrossFade(_clipName, _fadeTime);
        }
    }

    public override NodeStatus OnUpdate()
    {
        SwitchAnimation(clip, fadeTime);

        return NodeStatus.Success;
    }

/*    public override NodeStatus OnExit()
    {
        Debug.Log("Animation Exit!!");

        //SwitchAnimation("Walk Crouch", fadeTime);

        return base.OnExit();
    }*/
}
