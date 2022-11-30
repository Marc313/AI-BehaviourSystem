using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Guard : AICharacter
{
    [Header("Patrol Behaviour")]
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private float minDistance;

    [Header("Chase and Attack")]
    [SerializeField] private float chaseRange = 5;
    [SerializeField] private float attackRange = 2;

    [Header("Animator")]
    [SerializeField] private float animationFadeTime;

    // Awake is not in AICharacter, to add flexibility for the gameobject structure of the character
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        List<Vector3> patrolPositions = patrolPoints.Select(t => t.position).ToList();
        Transform player = FindObjectOfType<Player>().transform;

        /*        tree = new BTSelector(blackBoard,
                    new BTSequence(blackBoard, 
                        new BTIsTargetInRange(blackBoard, player, chaseRange),
                        new BTSequence(blackBoard,
                            new BTFollowTarget(blackBoard, player, attackRange),
                            new BTIsTargetInRange(blackBoard, player, attackRange),
                            new BTSequence(blackBoard,
                                new BTAnimate(blackBoard, "Kick", animationFadeTime),
                                new BTAttackTarget(blackBoard, player.gameObject, 10)
                                )
                            )
                        ),
                    new BTSequence(blackBoard,
                        new BTAnimate(blackBoard, "Rifle Walk", animationFadeTime),
                        new BTPatrolNode(blackBoard, minDistance, patrolPositions.ToArray()))
                    );*/

        tree = new BTPatrolNode(blackBoard, minDistance, patrolPositions.ToArray());
        
    }

    protected override void InitializeBlackboard()
    {
        blackBoard = new BlackBoard();

        blackBoard.AddOrUpdate("ControllerTransform", transform);
        blackBoard.AddOrUpdate("Agent", agent);
        blackBoard.AddOrUpdate("Animator", animator);
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
