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

    private List<Guard> guards = new List<Guard>();
    private Guard closestGuard;

    public BTSeekCover(BlackBoard _blackboard) : base(_blackboard)
    {
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        agent = blackboard.Get<NavMeshAgent>("Agent");
    }

    /*    public override NodeStatus OnUpdate()
        {
            Collider[] colliders = Physics.OverlapSphere(controllerTransform.position, range);

            Collider closestCollider = colliders.OrderBy(collider => Vector3.Distance(collider.transform.position, controllerTransform.position)).First();
            blackboard.AddOrUpdate("Cover", closestCollider.gameObject);

            return NodeStatus.Success;
        }*/

    public override NodeStatus OnUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(controllerTransform.position, range);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (NavMesh.SamplePosition(colliders[i].transform.position, out NavMeshHit hit, range, agent.areaMask))
            {
                if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                {
                    Debug.LogError("Did not find closest edge");
                }

                if (guards.Count == 0)
                {
                    blackboard.AddOrUpdate("CoverPosition", hit.position);
                }

                // Als er een enemy is
                else if (Vector3.Dot(hit.normal, (closestGuard.transform.position - hit.position).normalized) < 0)
                {

                }

                else return NodeStatus.Failed;
            }
        }
        
        //blackboard.AddOrUpdate("Cover", closestCollider.gameObject);

        return NodeStatus.Success;
    }
}
