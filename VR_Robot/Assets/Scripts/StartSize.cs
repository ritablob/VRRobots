using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSize : MonoBehaviour
{
    public Vector3 startSize;
    private Vector3 lerpStartSize;

    public void ScaleUP() {
        lerpStartSize = transform.localScale;
        StartCoroutine(ScaleUpNumerator());
    }

    private IEnumerator ScaleUpNumerator() {
        float timer = 0;
        while (transform.localScale != startSize) {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(lerpStartSize, startSize, timer);
            yield return null;
        }
    }
}
