using UnityEngine;
using UnityEngine.AI;

public class Ninja : AICharacter
{
    [Header("General Settings")]
    [SerializeField] private float defaultSpeed = 2;

    [Header("Follow State")]
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float minCoverDistance;

    [Header("Cover")]
    [SerializeField] private float inSightRange;
    [SerializeField] private float coverMoveSpeed;
    [SerializeField] private float maxCoverDistance = 25;
    [SerializeField] private GameObject smokeBombPrefab;
    [SerializeField] private float smokeBombCooldown = 1;

    [Header("Animation Curves")]
    [SerializeField] private AnimationCurve distanceToCoverEvaluator;
    [SerializeField] private AnimationCurve distanceToEnemyEvaluator;
    [SerializeField] private AnimationCurve lineOfSightEvaluator;
    [SerializeField] private AnimationCurve BlockingEnemySightCurve;

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
        Transform guard = FindObjectOfType<Guard>().transform;  

        ISpottable playerSpottable = player.GetComponent<ISpottable>();

        BTBaseNode followSequence = new BTSequence(blackBoard,
                                        new BTChangeBlackBoardVariable(blackBoard, "StateMessage", "Idle"),
                                        new BTAnimate(blackBoard, "Crouch Idle", animationFadeTime),
                                        new BTInvert(blackBoard,
                                                        new BTIsTargetInRange(blackBoard, player, distanceToTarget)
                                                     ),
                                        new BTChangeBlackBoardVariable(blackBoard, "StateMessage", "Follow Target"),
                                        new BTAnimate(blackBoard, "Walk Crouch"),
                                        new BTFollowTarget(blackBoard, player, distanceToTarget));

        BTBaseNode seekingCover = new BTSequence(blackBoard,
                                        new BTChangeBlackBoardVariable(blackBoard, "StateMessage", "Seek Cover"),
                                        new BTSeekCover(blackBoard, maxCoverDistance),
                                        new BTAnimate(blackBoard, "Walk Crouch"),
                                        new BTInvokeAction(blackBoard, () => agent.speed = coverMoveSpeed),
                                        new BTMoveToBlackboardPos(blackBoard, "BestCoverPosition", minCoverDistance),
                                        new BTInvokeAction(blackBoard, () => agent.speed = defaultSpeed),
                                        new BTAnimate(blackBoard, "Crouch Idle")
                                    );

        BTBaseNode playerSpottedSequence = new BTSequence(blackBoard,
                                                new BTIsTargetSpotted(blackBoard, playerSpottable),
                                                seekingCover,
                                                new BTChangeBlackBoardVariable(blackBoard, "StateMessage", "Throw Smokebomb"),
                                                new BTIsCooldownOver(blackBoard, "LastSmokeBombTime", smokeBombCooldown),
                                                new BTThrowObjectAtTarget(blackBoard, smokeBombPrefab, player),
                                                new BTRegisterCooldownTime(blackBoard, "LastSmokeBombTime")
                                            );

        // When it is not possible to follow the player (e.g. player is dead), the ninja will hide.
        BTBaseNode hideSequence = new BTSequence(blackBoard,
                                        new BTChangeBlackBoardVariable(blackBoard, "StateMessage", "Hiding"),
                                        seekingCover
            );

        tree = new BTSelector(blackBoard,
                                new BTConditionDecorator(blackBoard,
                                    new BTCustomCondition(blackBoard, () => !player.gameObject.activeSelf),
                                    hideSequence
                                ),
                                new BTSelector(blackBoard,
                                        playerSpottedSequence,
                                        followSequence
                                    )
                                );



        /*
                tree = new BTSequence(blackBoard,
                        new BTConditionDecorator(blackBoard, 
                            new BTConditional(blackBoard, () => Vector3.Distance(transform.position, player.position) < distanceToTarget),
                            new BTSequence(blackBoard,
                                new BTDebugTask(blackBoard, "Test1"),
                                new BTWaitTask(blackBoard, 2f),
                                new BTDebugTask(blackBoard, "Test2"),
                                new BTWaitTask(blackBoard, 2f),
                                new BTDebugTask(blackBoard, "Test3")

                            ))
                        );*/
    }

    protected override void InitializeBlackboard()
    {
        blackBoard = new BlackBoard();

        // Components
        blackBoard.AddOrUpdate("ControllerTransform", transform);
        blackBoard.AddOrUpdate("Agent", agent);
        blackBoard.AddOrUpdate("Animator", animator);

        // Animation Curves
        blackBoard.AddOrUpdate("CoverDistanceCurve", distanceToCoverEvaluator);
        blackBoard.AddOrUpdate("EnemyDistanceCurve", distanceToEnemyEvaluator);
        blackBoard.AddOrUpdate("SightCurve", lineOfSightEvaluator);
        blackBoard.AddOrUpdate("BlockingEnemySightCurve", BlockingEnemySightCurve);
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
