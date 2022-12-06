﻿public enum NodeStatus { Success, Failed, Running }
public abstract class BTBaseNode
{
    public abstract string displayName { get; }

    protected BlackBoard blackboard;
    public float utilityScore;  // Vervang met GetNormalizedUtilityScore method
    private bool isInitialized;

    public BTBaseNode(BlackBoard _blackboard)
    {
        blackboard = _blackboard;
    }

    public virtual NodeStatus OnEnter() { return NodeStatus.Success; }
    public abstract NodeStatus OnUpdate();
    public virtual NodeStatus OnExit() { return NodeStatus.Success; }

    public NodeStatus Evaluate()
    {
        if (!isInitialized)
        {
            OnEnter();
            isInitialized = true;
        }
        NodeStatus status = OnUpdate();
        if (status != NodeStatus.Running)
        {
            OnExit();
            isInitialized = false;
        }
        return status;
    }
}