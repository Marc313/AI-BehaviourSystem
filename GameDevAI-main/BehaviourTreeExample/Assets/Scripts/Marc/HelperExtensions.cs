using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static Codice.Client.Common.WebApi.WebApiEndpoints;

public static class HelperExtensions
{
    public static float GetPathLength(this NavMeshPath _path)
    {
        float totalLength = 0f;

        if ((_path.status != NavMeshPathStatus.PathInvalid) && (_path.corners.Length > 1))
        {
            for (int i = 1; i < _path.corners.Length; ++i)
            {
                totalLength += Vector3.Distance(_path.corners[i - 1], _path.corners[i]);
            }
        }

        return totalLength;
    }

    public static float CalculateDistanceToTarget(this NavMeshAgent _agent, Vector3 _targetPos)
    {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(_targetPos, path);
        return path.GetPathLength();
    }
}
