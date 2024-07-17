using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public GameObject[] highlightObjs;

    private void Start()
    {
        Director.instance.highlightObjects += Highlight;
    }

    private void OnDestroy()
    {
        Director.instance.highlightObjects -= Highlight;
    }

    public void HoverHighlight(bool state) {
        for (int i = 0; i < highlightObjs.Length; i++) {
            highlightObjs[i].SetActive(state);
        }
    }

    public void Highlight(bool state) {
        StopAllCoroutines();
        StartCoroutine(HighlightDelay(state));
    }

    private void ToggleHighlight(bool state) {
        for (int i = 0; i < highlightObjs.Length; i++) {
            highlightObjs[i].SetActive(state);
        }

        if (state) { StartCoroutine(UnHighlightDelay()); }
    }

    private IEnumerator HighlightDelay(bool state) {
        yield return new WaitForSeconds(Vector3.Distance(Director.instance.robot.position, transform.position) / 10);
        ToggleHighlight(state);
    }

    private IEnumerator UnHighlightDelay() {
        yield return new WaitForSeconds(5f);

        ToggleHighlight(false);
    }
}
