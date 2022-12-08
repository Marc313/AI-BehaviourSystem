
using UnityEngine;

public class BTSpawnObjectAtPlace : BTBaseNode
{
    public override string displayName => "VFX";
    private GameObject prefab;
    private Transform place;


    public BTSpawnObjectAtPlace(BlackBoard _blackboard, GameObject _prefab, Transform _place) : base(_blackboard)
    {
        prefab = _prefab;
        place = _place;
    }

    public override NodeStatus OnEnter()
    {
        Debug.Log("VFX");
        GameObject.Instantiate(prefab, place.position, place.rotation);
        return NodeStatus.Success;
    }

    public override NodeStatus OnUpdate()
    {
        return NodeStatus.Success;
    }
}
