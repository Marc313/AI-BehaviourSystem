using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoManager : MonoBehaviour
{
    public static event Action OnGizmo;

    private void OnDrawGizmos()
    {
        OnGizmo.Invoke();
    }
}
