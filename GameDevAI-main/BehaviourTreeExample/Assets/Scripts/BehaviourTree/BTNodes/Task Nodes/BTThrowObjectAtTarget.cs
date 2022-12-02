using UnityEngine;

public class BTThrowObjectAtTarget : BTBaseNode
{
    public override string displayName => "Throwing Bomb";

    private GameObject objectPrefab;
    private Transform target;

    public BTThrowObjectAtTarget(BlackBoard _blackboard, GameObject _objectPrefab, Transform _target) : base(_blackboard)
    {
        objectPrefab = _objectPrefab;
        target = _target;
    }

    public override NodeStatus OnEnter()
    {
        Debug.Log("Throw");
        GameObject.Instantiate(objectPrefab, target.position, Quaternion.identity);
        return base.OnEnter();
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }
}
