using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Movement : MonoBehaviour
{
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = (Vector3.one * 0.2f + new Vector3(Mathf.Sin(Mathf.Sin(Time.time / 2f) * Mathf.PI),
                                                           (Mathf.Cos(Mathf.Sin(Time.time / 3f) * Mathf.PI) + 1f) / 2f,
                                                            Mathf.Sin(Mathf.Sin(Time.time / 5f) * Mathf.PI)) / 3f) + startPos;
    }
}
