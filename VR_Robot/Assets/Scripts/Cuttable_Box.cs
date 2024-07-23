using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Cuttable_Box : MonoBehaviour
{
    [SerializeField] private List<Transform> parts;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Box_Cutter") {
            // Choose a random piece and break it off. 
            int r = Random.Range(0, parts.Count);

            // Detatch from parent
            parts[r].transform.parent = null;

            // Enable it's rigidbody and collider
            parts[r].GetComponent<Rigidbody>().isKinematic = false;
            parts[r].GetComponent<Collider>().enabled = true;
            parts[r].GetComponent<XRGrabInteractable>().enabled = true;
            parts[r].GetComponent<Custom_Interactable>().enabled = true;
            parts[r].GetComponent<Highlighter>().enabled = true;

            // Remove from list
            parts.RemoveAt(r);

            if (parts.Count == 0) { Destroy(gameObject); }
        }

        Debug.Log("HAND - " + other.tag, other.gameObject);
    }
}
