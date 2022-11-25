using Codice.CM.WorkspaceServer;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Guard : MonoBehaviour
{
    [Header("Patrol Behaviour")]
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private float minDistance;

    [Header("Chase and Attack")]
    [SerializeField] private float chaseRange = 5;
    [SerializeField] private float attackRange = 2;

    [Header("Information")]
    [SerializeField] private TMP_Text stateText;

    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;

    private float timer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        BlackBoard blackBoard = new BlackBoard();
        blackBoard.AddOrUpdate("ControllerTransform", transform);
        blackBoard.AddOrUpdate("Agent", agent);
        blackBoard.AddOrUpdate("Animator", animator);

        //blackBoard.AddOrUpdate("MoveToPoint", () => MoveTowards());

        List<Vector3> patrolPositions = patrolPoints.Select(t => t.position).ToList();

        Transform target = FindObjectOfType<Player>().transform;
        tree = new BTSelector(blackBoard,
                                new BTIsTargetInRange(blackBoard, target, chaseRange,
                                    new BTFollowTarget(blackBoard, target, minDistance)
                                    ),
                                new BTPatrolNode(blackBoard, minDistance, patrolPositions.ToArray())
                                );

/*        tree = new BTIsTargetInRange(blackBoard, target, chaseRange,
                    new BTDebugTask(blackBoard, "In Range!")
                );*/

        /*        tree = new BTParallel(blackBoard,
                    new BTWaitTask(blackBoard, 2f),
                    new BTWaitTask(blackBoard, 1f)
                );*/
    }

    private void FixedUpdate()
    {
        NodeStatus? status = tree?.Evaluate();
        if (tree != null) SetStateText(tree.displayName);
    }

    private void SetStateText(string _message)
    {
        stateText.text = _message;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Handles.color = Color.yellow;
    //    Vector3 endPointLeft = viewTransform.position + (Quaternion.Euler(0, -ViewAngleInDegrees.Value, 0) * viewTransform.transform.forward).normalized * SightRange.Value;
    //    Vector3 endPointRight = viewTransform.position + (Quaternion.Euler(0, ViewAngleInDegrees.Value, 0) * viewTransform.transform.forward).normalized * SightRange.Value;

    //    Handles.DrawWireArc(viewTransform.position, Vector3.up, Quaternion.Euler(0, -ViewAngleInDegrees.Value, 0) * viewTransform.transform.forward, ViewAngleInDegrees.Value * 2, SightRange.Value);
    //    Gizmos.DrawLine(viewTransform.position, endPointLeft);
    //    Gizmos.DrawLine(viewTransform.position, endPointRight);

    //}
}
