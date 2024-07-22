using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public GameObject[] highlightObjs;


    private void Start()
    {
        Director.instance.highlightObjects += Highlight;

        // If no object, automatically create it
        if (highlightObjs.Length == 0)
        {
            GameObject highlight = Instantiate(Director.instance.highlightObject, transform);
            highlight.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
            highlight.transform.localScale *= 1.2f;
            highlight.SetActive(false);
            highlightObjs = new GameObject[1];
            highlightObjs[0] = highlight;
            return;
        }

        for (int i = 0; i < highlightObjs.Length; i++)
        {
            VFXApplicationHelper.instance.ApplyHighlightParticleOnStart(highlightObjs[i]);
        }
    }

    private void OnDestroy()
    {
        Director.instance.highlightObjects -= Highlight;
    }

    public void HoverHighlight(bool state)
    {
        for (int i = 0; i < highlightObjs.Length; i++)
        {
            highlightObjs[i].SetActive(state);
        }
    }

    public void Highlight(bool state)
    {
        StopAllCoroutines();
        StartCoroutine(HighlightDelay(state));
    }

    private void ToggleHighlight(bool state)
    {
        for (int i = 0; i < highlightObjs.Length; i++)
        {
            highlightObjs[i].SetActive(state);
        }

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