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
    private float inSightRange;
    private float maxCoverDistance;

    private List<Guard> guards = new List<Guard>();
    private Guard closestGuard;

    public BTSeekCover(BlackBoard _blackboard, float _inSightRange, float _maxCoverDistance) : base(_blackboard)
    {
        inSightRange= _inSightRange;
        maxCoverDistance = _maxCoverDistance;
        controllerTransform = blackboard.Get<Transform>("ControllerTransform");
        agent = blackboard.Get<NavMeshAgent>("Agent");
    }

    public override NodeStatus OnEnter()
    {
        Collider[] collider = Physics.OverlapSphere(controllerTransform.position, maxCoverDistance);
        Cover[] covers = collider.Select(collider => collider.GetComponent<Cover>())
                                    .Where(cover => cover != null)
                                    .ToArray();

        List<UtilityEvaluator> evaluators = new List<UtilityEvaluator>();
        evaluators.Add(new DistanceToTargetEvaluator(blackboard, maxCoverDistance));

        float bestScore = 0f;
        Cover bestCover = null;
        foreach (Cover cover in covers)
        {
            blackboard.AddOrUpdate("CurrentCoverPos", cover.pos);
            float normalizedScore = evaluators.Sum(e => e.GetNormalizedScore()) / evaluators.Count;

            if (normalizedScore > bestScore)
            {
                bestScore = normalizedScore;
                bestCover = cover;
            }
        }

        blackboard.RemoveEntry("CurrentCoverPos");
        if (bestCover == null)
        {
            blackboard.RemoveEntry("CoverPosition");
            return NodeStatus.Failed;
        }
        else
        {
            Debug.Log("Set new cover position: " + bestCover.pos);
            blackboard.AddOrUpdate("CoverPosition", bestCover.pos);
            return NodeStatus.Success;
        }
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }

    /*    public override NodeStatus OnUpdate()
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
        }*/
}
