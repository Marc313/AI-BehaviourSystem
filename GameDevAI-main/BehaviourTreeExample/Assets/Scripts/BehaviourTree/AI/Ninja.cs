using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Ninja : MonoBehaviour
{
    [Header("Follow State")]
    [SerializeField] private float distanceToTarget;
    [SerializeField] private float minCoverDistance;

    [Header("Animator")]
    [SerializeField] private float animationFadeTime;

    [Header("Information")]
    [SerializeField] private TMP_Text stateText;

    private BTBaseNode tree;
    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //TODO: Create your Behaviour tree here

        /* Nodes to make:
         * 
         * DistanceToPlayerCondition
         * FollowPlayer (tot bepaalde afstand)
         * Seek Cover
         * Throw Smoke Bomb near Player
         * 
         */
        BlackBoard blackBoard = new BlackBoard();
        AddBlackBoardValues(blackBoard);

        Transform player = FindObjectOfType<Player>().transform;    // TODO: Service Locator
        Transform guard = FindObjectOfType<Guard>().transform;    // TODO: Service Locator
        /*        tree = new BTSelector(blackBoard,
                                            new BTFollowTarget(blackBoard, target, distanceToTarget, true),
                                            new BTSequence(blackBoard,
                                                new BTSeekCover(blackBoard),
                                                new BTMoveToPosNode(blackBoard, "CoverPosition", minCoverDistance))
                                            );*/

        BTBaseNode baseTree = new BTSelector(blackBoard,
                            new BTIsTargetInRange(blackBoard, guard, distanceToTarget,
                                new BTSequence(blackBoard,
                                    new BTSeekCover(blackBoard),
                                   new BTMoveToPosNode(blackBoard, "CoverPosition", minCoverDistance))),
                            new BTFollowTarget(blackBoard, player, distanceToTarget, true)
                            );

        tree = new BTSequence(blackBoard,
                new BTAnimate(blackBoard, "Rifle Walk", animationFadeTime),
                baseTree
                );
    }

    private void AddBlackBoardValues(BlackBoard _blackBoard)
    {
        _blackBoard.AddOrUpdate("ControllerTransform", transform);
        _blackBoard.AddOrUpdate("Agent", agent);
        _blackBoard.AddOrUpdate("Animator", animator);
    }

    private void FixedUpdate()
    {
        tree?.Evaluate();
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
