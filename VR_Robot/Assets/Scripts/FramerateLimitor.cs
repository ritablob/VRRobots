using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateLimitor : MonoBehaviour
{
    public bool limitFrameRate;

    void Start()
    {
        if (limitFrameRate) { Application.targetFrameRate = 60; } else { Application.targetFrameRate = -1; }
    }
    void OnDestroy()
    {
        if (limitFrameRate) { Application.targetFrameRate = -1; }
    }
}
