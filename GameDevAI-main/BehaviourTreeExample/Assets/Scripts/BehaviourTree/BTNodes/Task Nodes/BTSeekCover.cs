using Codice.Client.BaseCommands;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BTSeekCover : BTBaseNode
{
    public override string displayName => "Seek Cover";

    private GameObject coverObject;
    private Transform controllerTransform;
    private NavMeshAgent agent;
    private float range = 20;
    private float inSightRange;

    private List<Guard> guards = new List<Guard>();
    private Guard closestGuard;

    public BTSeekCover(BlackBoard _blackboard, float _inSightRange) : base(_blackboard)
    {
        inSightRange= _inSightRange;
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        agent = blackboard.Get<NavMeshAgent>("Agent");
    }

    public override NodeStatus OnUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(controllerTransform.position, range);
        closestGuard = colliders.Select(c => c.GetComponent<Guard>())
                                .Where(g => g != null)
                                .OrderBy(g => Vector3.Distance(g.transform.position, controllerTransform.position))
                                .First();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, range, agent.areaMask))
            {
                if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                {
                    Debug.LogError("Did not find closest edge");
                    return NodeStatus.Failed;
                }

                if (guards.Count == 0 || closestGuard == null
                    || Vector3.Dot(hit.normal, (closestGuard.transform.position - hit.position).normalized) < inSightRange)
                {
                    blackboard.AddOrUpdate("CoverPosition", hit.position);
                    return NodeStatus.Success;
                }
                else
                {
                    Vector3 awayFromTargetDirection = (colliders[i].transform.position - controllerTransform.position).normalized * 2;
                    Vector3 newSamplePosition = colliders[i].transform.position - (awayFromTargetDirection);
                    if (NavMesh.SamplePosition(newSamplePosition , out NavMeshHit hit2, range, agent.areaMask))
                    {
                        if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                        {
                            Debug.LogError("Did not find closest edge");
                            return NodeStatus.Failed;
                        }

                        if (Vector3.Dot(hit.normal, (closestGuard.transform.position - hit.position).normalized) < inSightRange)
                        {
                            blackboard.AddOrUpdate("CoverPosition", hit.position);
                            return NodeStatus.Success;
                        }
                    }
                }
            }
        }
        
        //blackboard.AddOrUpdate("Cover", closestCollider.gameObject);

        return NodeStatus.Success;
    }
}
