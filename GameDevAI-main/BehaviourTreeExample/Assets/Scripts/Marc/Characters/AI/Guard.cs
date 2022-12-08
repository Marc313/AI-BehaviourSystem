﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Guard : AICharacter, IWeaponWielder, IAlertable
{
    public Weapon weapon { get; set; }
    public bool isAlerted { get; private set; }

    [Header("Patrol Behaviour")]
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private float minDistance;

    [Header("Chase and Attack")]
    [SerializeField] private float chaseRange = 5;
    [SerializeField] private float attackingRange = 2;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRadius = 1.5f;

    [Header("Alerting")]
    [SerializeField] private GameObject alertVFXPrefab;
    [SerializeField] private Transform vfxTransform;
    [SerializeField] private float alertedDuration = 1;
    [SerializeField] private float alertRange = 20;

    [Header("Finding Weapons")]
    [SerializeField] private float maxWeaponDistance = 25;
    [SerializeField] private AnimationCurve damageEvaluator;
    [SerializeField] private AnimationCurve distanceEvaluator;
    [SerializeField] private Transform weaponTransform;

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

        ISpottable playerSpottable = player.GetComponent<ISpottable>();

        BTBaseNode attackSequence = new BTConditionDecorator(blackBoard,
                                            new BTIsTargetInRange(blackBoard, player, attackingRange),
                                            new BTSequence(blackBoard,
                                                new BTInvokeAction(blackBoard, () => agent.isStopped = true),
                                                new BTChangeBlackBoardVariable(blackBoard, "StateMessage", "Attacking Target"),
                                                new BTAnimate(blackBoard, "Melee Attack"),
                                                new BTWaitTillAnimEnd(blackBoard)
                                                //new BTAnimate(blackBoard, "Idle")
                                                )
                                            );

        BTBaseNode chasingTree = new BTSelector(blackBoard,
                                    attackSequence,
                                    new BTSequence(blackBoard,
                                        new BTChangeBlackBoardVariable(blackBoard, "StateMessage", "Chasing Target"),
                                        new BTAnimate(blackBoard, "Rifle Walk"),
                                        new BTFollowTarget(blackBoard, player, attackingRange)
                                        )
                                    );

        BTBaseNode whileInSight = new BTConditionDecorator(blackBoard,
                                        new BTSequence(blackBoard,
                                            new BTCustomCondition(blackBoard, () => player.gameObject.activeSelf),
                                            new BTIsTargetInSight(blackBoard, player, inSightRange)
                                        ),
                                        chasingTree
                                    );

        BTBaseNode findWeaponSequence = new BTSequence(blackBoard,
                                                    new BTInvert(blackBoard,
                                                        new BTHasWeapon(blackBoard, this)
                                                    ),
                                                    new BTChangeBlackBoardVariable(blackBoard, "StateMessage", "Pickup Weapon"),
                                                    new BTSeekWeapon(blackBoard, maxWeaponDistance),
                                                    new BTMoveToBlackboardPos(blackBoard, "BestWeaponPosition", minDistance),
                                                    new BTPickupWeapon(blackBoard)
                                                );

        BTBaseNode onPlayerSpottedSequence = new BTSelector(blackBoard,
                                                findWeaponSequence,
                                                whileInSight
                                                );

        BTBaseNode checkPlayerInSight = new BTSequence(blackBoard,
                                                new BTCustomCondition(blackBoard, () => player.gameObject.activeSelf),
                                                new BTIsTargetInRange(blackBoard, player, chaseRange),
                                                new BTIsTargetInSight(blackBoard, player, inSightRange),
                                                new BTSpotTarget(blackBoard, playerSpottable, true),
                                                new BTAlertAlliesInRange<Guard>(blackBoard, alertRange),
                                                new BTSpawnObjectAtPlace(blackBoard, alertVFXPrefab, vfxTransform),
                                                onPlayerSpottedSequence
                                                );

        BTBaseNode patrolNode = GeneratePatrolNode();

        BTBaseNode patrolTree = new BTSequence(blackBoard,
                                    new BTDebugTask(blackBoard, "Patrolling"),
                                    new BTChangeBlackBoardVariable(blackBoard, "StateMessage", "Patrolling"),
                                    new BTAnimate(blackBoard, "Rifle Walk", animationFadeTime),
                                    patrolNode);

        BTBaseNode alertedTree = new BTSequence(blackBoard, 
                                    new BTIsAlerted(blackBoard, this),
                                    onPlayerSpottedSequence
                                );

        tree = new BTSelector(blackBoard,
                    alertedTree,
                    checkPlayerInSight,
                    patrolTree
                    );

        tree = new BTParallel(blackBoard,
                 tree,
                 new BTSelector(blackBoard,
                    new BTSequence(blackBoard,
                        new BTIsTargetInSight(blackBoard, player, inSightRange),
                        new BTSequence(blackBoard,
                            new BTSpotTarget(blackBoard, playerSpottable, true))
                    ),
                    new BTSpotTarget(blackBoard, playerSpottable, false)
                 )
                );

/*        tree = new BTParallel(blackBoard,
                    new BTConditionDecorator(blackBoard,
                            new BTConditional(blackBoard, () => player.gameObject.activeSelf),
                            new BTDebugTask(blackBoard, "Player active")),
                    tree
                            );*/


        /*        BTBaseNode patroltest = new BTSelector(blackBoard,
                                            new BTConditionDecorator(blackBoard,
                                                new BTIsTargetInRange(blackBoard, player, chaseRange),
                                                new BTSequence(blackBoard,
                                                    new BTChangeBlackBoardVariable(blackBoard, "StateMessage", "Chasing"),
                                                    new BTFollowTarget(blackBoard, player, minDistance))
                                                ),
                                            patrolTree
                            );*/

        /*        BTBaseNode SightTest = new BTSequence(blackBoard,
                            new BTIsTargetInSight(blackBoard, player, inSightRange),
                            new BTDebugTask(blackBoard, "In Sight!")
                                );*/

        //tree = patroltest;

        //tree = new BTDebugTask(blackBoard, "Print");
    }
/*
    protected override void Start()
    {
        base.Start();

        tree = new BTSequence(blackBoard,
                    new BTDebugTask(blackBoard, "Print1"),
                    new BTWaitTask(blackBoard, 1),
                    new BTDebugTask(blackBoard, "Print2"),
                    new BTWaitTask(blackBoard, 1),
                    new BTDebugTask(blackBoard, "Print3"),
                    new BTWaitTask(blackBoard, 1)
            );
    }*/

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (weapon != null)
        {
            weapon.transform.position = weaponTransform.position;
        }
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
        blackBoard.AddOrUpdate("CoverDistanceCurve", distanceEvaluator);
        blackBoard.AddOrUpdate("DamageCurve", damageEvaluator);

        // Floats
        blackBoard.AddOrUpdate("AttackRange", attackingRange);
    }

    public async void Alert()
    {
        isAlerted = true;
        await Task.Delay((int) (alertedDuration * 1000));
    }
}
