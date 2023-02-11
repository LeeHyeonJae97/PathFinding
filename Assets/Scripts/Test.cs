using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private MeshRenderer _mr;
    [SerializeField] private Collider _coll;

    private void OnDrawGizmos()
    {
        var bounds = _mr.bounds;
        var min = bounds.min;
        var max = bounds.max;

        var center = (min + max) / 2;
        var size = max - min;

        Gizmos.DrawWireRect(center, size, Color.red);
    }
}
