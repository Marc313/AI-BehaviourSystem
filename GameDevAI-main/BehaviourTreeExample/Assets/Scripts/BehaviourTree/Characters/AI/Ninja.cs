using UnityEngine;
using UnityEngine.AI;

public class Ninja : AICharacter
{
    [Header("Follow State")]
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float minCoverDistance;

    [Header("Cover")]
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

        Transform player = FindObjectOfType<Player>().transform;    // TODO: Service Locator
        Transform guard = FindObjectOfType<Guard>().transform;    // TODO: Service Locator

        BTBaseNode followSequence = new BTSequence(blackBoard,
                                        new BTAnimate(blackBoard, "Crouch Idle", animationFadeTime),
                                        new BTInvert(blackBoard,
                                                        new BTIsTargetInRange(blackBoard, player, distanceToTarget)
                                                     ),
                                        new BTAnimate(blackBoard, "Walk Crouch"),
                                        new BTFollowTarget(blackBoard, player, distanceToTarget));

        tree = followSequence;

/*        BTBaseNode baseTree = new BTSelector(blackBoard,
                                new BTSequence(blackBoard,
                                        new BTIsTargetInRange(blackBoard, guard, distanceToTarget),
                                        new BTSeekCover(blackBoard, inSightRange),
                                       new BTMoveToPosNode(blackBoard, "CoverPosition", minCoverDistance)),
                                new BTFollowTarget(blackBoard, player, distanceToTarget, true)
                            );*/

/*        tree = new BTSelector(blackBoard,
                            new BTInvert(blackBoard, tree),
                            new BTAnimate(blackBoard, "Idle")
                            );*/
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
