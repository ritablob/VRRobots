using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadPat : MonoBehaviour
{
    bool pat = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hand") { 
            if (pat) {
                StopAllCoroutines();
                pat = false;
                Director.instance.robot.GetComponent<Robot_Interaction_State_Machine>().Headpat();
            }
            else {
                StartCoroutine(HeadpatTimer());
                pat = true;
            }
        }
    }

    IEnumerator HeadpatTimer() {
        yield return new WaitForSeconds(1);

        pat = false;
    }
}
