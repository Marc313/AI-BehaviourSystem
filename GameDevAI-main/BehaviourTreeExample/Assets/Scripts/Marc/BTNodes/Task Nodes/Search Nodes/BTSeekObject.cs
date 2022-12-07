using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class BTSeekObject<T> : BTBaseNode where T : MonoBehaviour
{
    public override string displayName => "Seek " + typeName;

    protected List<UtilityEvaluator> evaluators;

    protected Transform controllerTransform;
    protected NavMeshAgent agent;
    
    protected string typeName;
    protected float maxDistance;

    public BTSeekObject(BlackBoard _blackboard, float _maxDistance) : base(_blackboard)
    {
        typeName = typeof(T).Name;
        maxDistance = _maxDistance;
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        agent = blackboard.Get<NavMeshAgent>("Agent");
    }

    public abstract void SetupEvaluators();

    public override NodeStatus OnEnter()
    {
        // Find objects in range
        Collider[] collider = Physics.OverlapSphere(controllerTransform.position, maxDistance);
        T[] objectsInRange = collider.Select(collider => collider.GetComponent<T>())
                                    .Where(t => t != null)
                                    .ToArray();

        SetupEvaluators();

        // Find Object with best evaluated Score
        float bestScore = 0f;
        T bestObject = null;
        foreach (T foundObject in objectsInRange)
        {
            blackboard.AddOrUpdate($"CurrentSearchItem", foundObject);
            float normalizedScore = evaluators.Sum(e => e.GetNormalizedScore()) / evaluators.Count;

            if (normalizedScore > bestScore)
            {
                bestScore = normalizedScore;
                bestObject = foundObject;
            }
        }
        blackboard.RemoveEntry($"CurrentSearchItem");

        // Put the result in blackboard, remove entry if no cover is found
        if (bestObject == null)
        {
            blackboard.RemoveEntry($"Best{typeName}Position");
            blackboard.RemoveEntry($"Best{typeName}");

            return NodeStatus.Failed;
        }
        else
        {
            Debug.Log($"Set new {typeName} position: " + bestObject.transform.position);
            blackboard.AddOrUpdate($"Best{typeName}Position", bestObject.transform.position);
            blackboard.AddOrUpdate($"Best{typeName}", bestObject);
            return NodeStatus.Success;
        }
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }
}
