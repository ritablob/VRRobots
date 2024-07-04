using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Joint : MonoBehaviour
{
    public Vector3 RotationAxis;
    public Vector3 StartOffset;
    public char _rotationAxis;

    private void Awake()
    {
        StartOffset = transform.localPosition;
    }
}
