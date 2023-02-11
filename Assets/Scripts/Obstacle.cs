using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Bounds Bounds { get { return _renderer.bounds; } }

    [SerializeField] private Renderer _renderer;
}
