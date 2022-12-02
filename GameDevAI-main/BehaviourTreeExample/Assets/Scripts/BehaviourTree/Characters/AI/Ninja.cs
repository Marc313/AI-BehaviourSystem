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
    [SerializeField] private AnimationCurve distanceEvaluator;
    [SerializeField] private AnimationCurve lineOfSightEvaluator;

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

        Transform player = FindObjectOfType<Player>().transform;    // TODO: Service Locator
        Transform guard = FindObjectOfType<Guard>().transform;    // TODO: Service Locator

        ISpottable playerSpottable = player.GetComponent<ISpottable>();

        /*        BTBaseNode followSequence = new BTSequence(blackBoard,
                                                new BTAnimate(blackBoard, "Crouch Idle", animationFadeTime),
                                                new BTInvert(blackBoard,
                                                                new BTIsTargetInRange(blackBoard, player, distanceToTarget)
                                                             ),
                                                new BTAnimate(blackBoard, "Walk Crouch"),
                                                new BTFollowTarget(blackBoard, player, distanceToTarget));

                BTBaseNode playerSpottedSequence = new BTSequence(blackBoard,
                                                        new BTIsTargetSpotted(blackBoard, playerSpottable),
                                                        new BTSeekCover(blackBoard, inSightRange, maxCoverDistance),
                                                        new BTAnimate(blackBoard, "Walk Crouch"),
                                                        new BTInvokeAction(blackBoard, () => agent.speed = coverMoveSpeed),
                                                        new BTMoveToBlackboardPos(blackBoard, "CoverPosition", minCoverDistance),
                                                        new BTInvokeAction(blackBoard, () => agent.speed = defaultSpeed),
                                                        new BTAnimate(blackBoard, "Crouch Idle"),
                                                        new BTThrowObjectAtTarget(blackBoard, smokeBombPrefab, player),
                                                        new BTWaitTask(blackBoard, smokeBombCooldown)
                                                    );

                blackBoard.AddOrUpdate("TestPos", new Vector3(5, 0, 5));

                tree = new BTSelector(blackBoard,
                                        new BTMoveToBlackboardPos(blackBoard, "TestPos", minCoverDistance),
                                        playerSpottedSequence,
                                        followSequence
                                        );*/

        tree = new BTSequence(blackBoard,
                new BTConditional(blackBoard, () => Vector3.Distance(transform.position, player.position) < distanceToTarget),
                new BTSequence(blackBoard,
                    new BTDebugTask(blackBoard, "Test1"),
                    new BTWaitTask(blackBoard, 2f),
                    new BTDebugTask(blackBoard, "Test2"),
                    new BTWaitTask(blackBoard, 2f),
                    new BTDebugTask(blackBoard, "Test3")

                    )
                );
    }

    protected override void InitializeBlackboard()
    {
        blackBoard = new BlackBoard();

        // Components
        blackBoard.AddOrUpdate("ControllerTransform", transform);
        blackBoard.AddOrUpdate("Agent", agent);
        blackBoard.AddOrUpdate("Animator", animator);

        // Animation Curves
        blackBoard.AddOrUpdate("DistanceCurve", distanceEvaluator);
        blackBoard.AddOrUpdate("SightCurve", lineOfSightEvaluator);
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
