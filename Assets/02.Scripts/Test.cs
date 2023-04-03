using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private MinHeap<int> _heap = new MinHeap<int>(20);

    private void Start()
    {
        _heap.Add(10);
        _heap.Add(20);
        _heap.Add(30);
        _heap.Add(40);
    }
}
