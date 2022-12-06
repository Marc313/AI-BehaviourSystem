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



        BTBaseNode attackSequence = new BTConditionDecorator(blackBoard,
                                            new BTIsTargetInRange(blackBoard, player, attackRange),
                                            new BTSequence(blackBoard,
                                                new BTAnimate(blackBoard, "Kick"),
                                                new BTWaitTillAnimEnd(blackBoard),
                                                new BTDebugTask(blackBoard, "AnimationEnd"),
                                                new BTAnimate(blackBoard, "Idle")
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

        BTBaseNode patrolNode = GeneratePatrolNode();

        BTBaseNode patrolTree = new BTSequence(blackBoard,
                                    /*new BTSpotTarget(blackBoard, playerSpottable, false),*/
                                    /*new BTAnimate(blackBoard, "Rifle Walk", animationFadeTime),*/
                                    patrolNode);

        tree = new BTSelector(blackBoard,
                    new BTConditionDecorator(blackBoard,
                        new BTIsTargetInRange(blackBoard, player, chaseRange),
                        new BTFollowTarget(blackBoard, player, minDistance)),
                    patrolTree
                );
        /*
                BTBaseNode SightTest = new BTSequence(blackBoard,
                            new BTIsTargetInSight(blackBoard, player, inSightRange),
                            new BTDebugTask(blackBoard, "In Sight!")
                                );

                tree = SightTest;*/

        //tree = attackSequence;
    }

    private BTBaseNode GeneratePatrolNode() 
    {
        List<Vector3> patrolPositions = patrolPoints.Select(t => t.position).ToList();

        BTMoveToPosition[] children = new BTMoveToPosition[patrolPositions.Count];
        for (int i = 0; i < patrolPositions.Count; i++)
        {
            children[i] = new BTMoveToPosition(blackBoard, patrolPositions[i], minDistance);
        }

        return new BTSequence(blackBoard, children);
        /*
                BTSequence s = new BTSequence(blackBoard,
                                new BTMoveToPosition(blackBoard, patrolPositions[0], minDistance),
                                new BTMoveToPosition(blackBoard, patrolPositions[1], minDistance),
                                new BTMoveToPosition(blackBoard, patrolPositions[2], minDistance),
                                new BTMoveToPosition(blackBoard, patrolPositions[3], minDistance),
                                new BTMoveToPosition(blackBoard, patrolPositions[4], minDistance)
                                );

                return s;*/
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
