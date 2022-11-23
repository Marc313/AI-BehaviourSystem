using UnityEngine;

public class BTWaitTask : BTBaseNode
{
    private float waitTime;
    private float timer;

    public BTWaitTask(BlackBoard _blackboard, float _waitTime) : base(_blackboard)
    {
        waitTime = _waitTime;
        timer = _waitTime;
    }

    public override NodeStatus OnEnter()
    {
        return NodeStatus.Success;
    }

    public override NodeStatus OnExit()
    {
        timer = waitTime;
        return NodeStatus.Success;
    }

    public override NodeStatus OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            return NodeStatus.Success;
        }
        return NodeStatus.Running;
    }
}
