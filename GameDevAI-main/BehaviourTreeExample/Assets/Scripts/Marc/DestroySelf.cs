using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] float invokeTime = 2;

    private void Start()
    {
        Invoke(nameof(DestroyingSelf), invokeTime);
    }

    private void DestroyingSelf()
    {
        Destroy(gameObject);
    }
}
