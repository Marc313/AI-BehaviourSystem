using Codice.Client.BaseCommands.BranchExplorer.ExplorerTree;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Guard : AICharacter
{
    [HideInInspector] public Weapon weapon;

    [Header("Patrol Behaviour")]
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private float minDistance;

    [Header("Chase and Attack")]
    [SerializeField] private float chaseRange = 5;
    [SerializeField] private float attackRange = 2;

    [Header("Finding Weapons")]
    [SerializeField] private float maxWeaponDistance = 25;
    [SerializeField] private AnimationCurve damageEvaluator;
    [SerializeField] private AnimationCurve distanceEvaluator;

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
                                                    new BTIsTargetInSight(blackBoard, player, inSightRange),
                                                    new BTSequence(blackBoard,
                                                        new BTSpotTarget(blackBoard, player.GetComponent<ISpottable>(), true),
                                                        new BTFollowTarget(blackBoard, player, attackRange),
                                                        new BTIsTargetInRange(blackBoard, player, attackRange),
                                                        new BTSequence(blackBoard,
                                                            new BTAnimate(blackBoard, "Kick", animationFadeTime),
                                                            new BTAttackTarget(blackBoard, player.gameObject, 10)
                                                            )
                                                        )
                                                    );*/
        ISpottable playerSpottable = player.GetComponent<ISpottable>();



        BTBaseNode attackSequence = new BTSequence(blackBoard,
                                            new BTIsTargetInRange(blackBoard, player, attackRange),
                                            new BTSequence(blackBoard,
                                                new BTAnimate(blackBoard, "Kick"),
                                                new BTWaitTillAnimEnd(blackBoard)
                                                )
                                        );

        BTBaseNode chasingTree = new BTParallel(blackBoard,
                                    new BTFollowTarget(blackBoard, player, attackRange),
                                    attackSequence
                                    );

        BTBaseNode whileInSight = new BTConditionDecorator(blackBoard,
                                        new BTIsTargetInSight(blackBoard, player, inSightRange),
                                        chasingTree
                                    );

        BTBaseNode findWeaponSequence = new BTSequence(blackBoard,
                                            new BTIsTargetInRange(blackBoard, player, chaseRange),
                                            new BTIsTargetInSight(blackBoard, player, inSightRange),
                                            new BTSpotTarget(blackBoard, playerSpottable, true),
                                            new BTSelector(blackBoard,
                                                new BTSequence(blackBoard,
                                                    new BTInvert(blackBoard, 
                                                        new BTHasWeapon(blackBoard, this)
                                                    ),
                                                    new BTSeekWeapon(blackBoard, maxWeaponDistance),
                                                    new BTMoveToBlackboardPos(blackBoard, "BestWeaponPosition", minDistance),
                                                    new BTPickupWeapon(blackBoard),
                                                    whileInSight
                                                ) 
                                            ));

        BTBaseNode patrolTree = new BTSequence(blackBoard,
                                    new BTSpotTarget(blackBoard, playerSpottable, false),
                                    new BTAnimate(blackBoard, "Rifle Walk", animationFadeTime),
                                    new BTPatrolNode(blackBoard, minDistance, patrolPositions.ToArray()));

        tree = new BTSelector(blackBoard,
                    findWeaponSequence,
                    patrolTree
                );
        /*
                BTBaseNode SightTest = new BTSequence(blackBoard,
                            new BTIsTargetInSight(blackBoard, player, inSightRange),
                            new BTDebugTask(blackBoard, "In Sight!")
                                );

                tree = SightTest;*/

        tree = attackSequence;
    }

    protected override void InitializeBlackboard()
    {
        blackBoard = new BlackBoard();

        // Components
        blackBoard.AddOrUpdate("Base", this);
        blackBoard.AddOrUpdate("ControllerTransform", transform);
        blackBoard.AddOrUpdate("Agent", agent);
        blackBoard.AddOrUpdate("Animator", animator);

        // Curves
        blackBoard.AddOrUpdate("DistanceCurve", distanceEvaluator);
        blackBoard.AddOrUpdate("DamageCurve", damageEvaluator);

        // Floats
        blackBoard.AddOrUpdate("AttackRange", attackRange);
    }
}
