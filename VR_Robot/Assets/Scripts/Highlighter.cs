using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public GameObject highlightObj;


    private void Start()
    {
        Director.instance.highlightObjects += Highlight;

        // If no object (or first element is null), automatically create it
        if (highlightObj == null)
        {
            GameObject highlight = Instantiate(Director.instance.highlightObject, transform);
            highlightObj = highlight.gameObject;
            highlightObj.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
            highlightObj.transform.localScale *= 1.2f;
            highlightObj.SetActive(false);
            return;
        }

            VFXApplicationHelper.instance.ApplyHighlightParticleOnStart(highlightObj);
    }

    private void OnDestroy()
    {
        Director.instance.highlightObjects -= Highlight;
    }

    public void HoverHighlight(bool state)
    {
        highlightObj.SetActive(state);

        if (!state) {
            Director.instance.SetCurrentHighlighted(highlightObj.transform, false);
        } else {
            Director.instance.SetCurrentHighlighted(highlightObj.transform, true);
        }     
    }

    public void Highlight(bool state)
    {
        StopAllCoroutines();
        StartCoroutine(HighlightDelay(state));
    }

    private void ToggleHighlight(bool state)
    {
        highlightObj.SetActive(state);

        if (state)
        {
            StartCoroutine(UnHighlightDelay());
        }
    }

    private IEnumerator HighlightDelay(bool state)
    {
        yield return new WaitForSeconds(Vector3.Distance(Director.instance.robot.position, transform.position) / 10);
        ToggleHighlight(state);
    }

    private IEnumerator UnHighlightDelay()
    {
        yield return new WaitForSeconds(5f);

        ToggleHighlight(false);
    }
}