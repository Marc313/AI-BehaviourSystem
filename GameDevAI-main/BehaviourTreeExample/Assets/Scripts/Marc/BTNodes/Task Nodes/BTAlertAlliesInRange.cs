using System.Linq;
using UnityEngine;

// Voor nu is T altijd een guard, maar dit zou een IAlertable interface kunnen zijn
public class BTAlertAlliesInRange<T> : BTBaseNode where T : IAlertable
{
    public override string displayName => "Alerting";

    private Transform controllerTransform;
    private float alertRange;

    public BTAlertAlliesInRange(BlackBoard _blackboard, float _alertRange) : base(_blackboard)
    {
        alertRange = _alertRange;
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
    }

    public override NodeStatus OnEnter()
    {
        Collider[] colliders = Physics.OverlapSphere(controllerTransform.position, alertRange);
        T[] allies = colliders.Select(collider => collider.GetComponent<T>())
                                .Where(ally => ally != null)
                                .ToArray();

        foreach (T ally in allies)
        {
            ally.Alert();
        }

        /*        foreach (Collider collider in colliders)
                {
                    T ally = collider.GetComponent<T>();
                    if (ally != null)
                    {
                        ally.Alert();
                    }
                }*/

        return NodeStatus.Success;
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }
}
