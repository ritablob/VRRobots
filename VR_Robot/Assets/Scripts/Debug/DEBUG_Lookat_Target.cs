using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_Lookat_Target : MonoBehaviour
{
    private Transform target;

    private void Start() {
        Director.instance.grabItem += SetTarget;
    }

    void Update() {
        if (target == null) { return; }

        transform.LookAt(target);
    }

    private void SetTarget(string _itemName) {
        target = GameObject.FindGameObjectWithTag(_itemName).GetComponent<Transform>();
    }
}
