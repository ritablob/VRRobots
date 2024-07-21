using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbagecan : MonoBehaviour
{
    public GarbageType garbageType;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Garbage_Bit>(out Garbage_Bit bit) && !other.GetComponent<Rigidbody>().isKinematic) {
            other.transform.localScale -= Vector3.one * Time.deltaTime * 100;

            if (other.TryGetComponent<StartSize>(out StartSize size)) { size.StopAllCoroutines(); }
            if (other.TryGetComponent<Custom_Interactable>(out Custom_Interactable interactable)) { Destroy(interactable); }

            if (other.transform.localScale.x <= 0) {
                if (bit.type == garbageType) { Director.instance.GetPoints(5); }
                else if (garbageType == GarbageType.General) { Director.instance.GetPoints(1); }
                else { Director.instance.GetPoints(-3); }
                Destroy(other.gameObject); 
            }
        }
    }
}

// Black = General
// Blue = Paper
// Yellow = Plastic