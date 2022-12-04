using UnityEngine;

public class BTWaitTillAnimEnd : BTBaseNode
{
    public override string displayName => "Waiting on Animation End";

    private Animator animator;
    private AnimationClip clip;
    private string clipName;
    private BTWaitTask waitNode;

    public BTWaitTillAnimEnd(BlackBoard _blackboard) : base(_blackboard)
    {
        animator = blackboard.Get<Animator>("Animator");
    }

    public override NodeStatus OnEnter()
    {
        float currentClipLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        waitNode = new BTWaitTask(blackboard, currentClipLength);

        return NodeStatus.Success;
    }

    public override NodeStatus OnUpdate()
    {
        return waitNode.Evaluate();
    }
}
