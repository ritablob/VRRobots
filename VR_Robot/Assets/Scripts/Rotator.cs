using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float speed;

    void Update() {
        transform.eulerAngles += new Vector3(0, Time.deltaTime * speed, 0);
    }
}
