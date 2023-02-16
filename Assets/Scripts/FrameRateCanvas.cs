using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrameRateCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        _text.text = $"{(int)(1 / Time.deltaTime)}";
    }
}
