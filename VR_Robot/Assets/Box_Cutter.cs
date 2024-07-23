using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box_Cutter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cuttable_Box>(out Cuttable_Box box)) {
            box.Cut();
        }
    }
}
