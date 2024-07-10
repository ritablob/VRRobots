using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbagecan : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8 && !other.GetComponent<Rigidbody>().isKinematic) {
            other.transform.localScale -= Vector3.one * Time.deltaTime * 100;
            if (other.TryGetComponent<Custom_Interactable>(out Custom_Interactable interactable)) { Destroy(interactable); }
            if (other.transform.localScale.x <= 0) { Destroy(other.gameObject); }
        }
    }
}
