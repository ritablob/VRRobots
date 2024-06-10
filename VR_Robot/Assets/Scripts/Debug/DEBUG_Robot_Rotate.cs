using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_Robot_Rotate : MonoBehaviour
{
    public AnimationCurve curve;

    void Start() {
        Director.instance.grabItem += SetTarget;
    }

    private void SetTarget(string _tag) {
        GameObject target = GameObject.FindGameObjectWithTag(_tag);

        if (target == null) { return; }

        StartCoroutine(RotateToTarget(target.transform));
    }

    private IEnumerator RotateToTarget(Transform _target) {
        Quaternion startRotation = transform.rotation;
        Vector3 directionToTarget = _target.position - transform.position;
        directionToTarget.y = 0; // Keep the direction only on the Y axis
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        float timer = 0;

        while (timer < 1) {
            timer += Time.deltaTime;

            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, curve.Evaluate(timer));

            yield return null;
        }
    }
}
