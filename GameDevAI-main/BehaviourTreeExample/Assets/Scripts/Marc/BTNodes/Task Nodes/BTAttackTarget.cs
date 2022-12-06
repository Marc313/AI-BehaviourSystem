using UnityEngine;

public class BTAttackTarget : BTBaseNode
{
    public override string displayName => "Attacking";

    private IHealthUser playerTarget;
    private Transform controllerTransform;

    private int damage;

    // TODO: Make ITarget/ICharacter/IHealthUser
    public BTAttackTarget(BlackBoard _blackboard, GameObject _target, float _damage) : base(_blackboard)
    {
        playerTarget = _target.GetComponent<IHealthUser>();
        controllerTransform = _blackboard.Get<Transform>("ControllerTransform");
    }

    public override NodeStatus OnUpdate()
    {
        if (playerTarget == null) return NodeStatus.Failed;

        ((MonoBehaviour)playerTarget).gameObject.GetComponent<Player>().TakeDamage(controllerTransform.gameObject, damage);
        return NodeStatus.Success;
    }
}
