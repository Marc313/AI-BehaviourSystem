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

    [Header("Sight")]
    [Range(-0.5f, 1f)]
    [SerializeField] private float inSightRange;

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

        /*        BTBaseNode chasingSequence = new BTSequence(blackBoard,
                                                    new BTIsTargetInRange(blackBoard, player, chaseRange),
                                                    new BTSequence(blackBoard,
                                                        new BTSpotTarget(blackBoard, player.GetComponent<ISpottable>(), true),
                                                        new BTFollowTarget(blackBoard, player, attackRange),
                                                        new BTIsTargetInRange(blackBoard, player, attackRange),
                                                        new BTSequence(blackBoard,
                                                            new BTAnimate(blackBoard, "Kick", animationFadeTime),
                                                            new BTAttackTarget(blackBoard, player.gameObject, 10)
                                                            )
                                                        )
                                                    );

                BTBaseNode patrolTree = new BTSequence(blackBoard,
                                            new BTAnimate(blackBoard, "Rifle Walk", animationFadeTime),
                                            new BTPatrolNode(blackBoard, minDistance, patrolPositions.ToArray()));

                tree = new BTSelector(blackBoard,
                            chasingSequence,
                            patrolTree
                        );*/

        tree = new BTSequence(blackBoard,
                    new BTIsTargetInSight(blackBoard, player, inSightRange),
                    new BTDebugTask(blackBoard, "In Sight!")
                        );
    }

    protected override void InitializeBlackboard()
    {
        blackBoard = new BlackBoard();

        blackBoard.AddOrUpdate("ControllerTransform", transform);
        blackBoard.AddOrUpdate("Agent", agent);
        blackBoard.AddOrUpdate("Animator", animator);
    }
}
